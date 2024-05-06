targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Application environment')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Name of the shared resource group')
param sharedResourceGroup string

@description('Domain name')
param domainName string

@maxLength(12)
param actionGroupShortName string

@description('Azure AD B2C application client ID')
param authenticationClientId string

@description('Azure AD B2C audience')
param authenticationAudience string

@description('Name of the Azure App Configuration instance')
param appConfigurationName string

@description('Application administrator group name')
param adminGroupName string

@description('Application administrator group object ID')
param adminGroupObjectId string

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

@description('Array of allowed external IP addresses. Needs to be an array of objects with name and ipAddress properties.')
param allowedExternalIpAddresses array

@description('Container registry login server')
param containerRegistryName string

@description('Name of the API container image')
param apiImageName string

@description('Tag of the API container image')
param apiImageTag string

var tags = {
  workload: workload
  appEnv: appEnv
}

var deploymentSuffix = startsWith(deployment().name, 'main-') ? replace(deployment().name, 'main-', '-') : ''

var sqlDatabaseAllowedAzureServices = [
  {
    name: 'azure'
    ipAddress: '0.0.0.0'
  }
]

// app configuration (existing)
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-03-01' existing = {
  name: appConfigurationName
  scope: resourceGroup(sharedResourceGroup)
}

// azure sql server
resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: '${workload}-${appEnv}-sql'
  location: location
  tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: adminGroupName
      sid: adminGroupObjectId
      principalType: 'Group'
      tenantId: subscription().tenantId
    }
    minimalTlsVersion: '1.2'
    version: '12.0'
    publicNetworkAccess: 'Enabled'
  }

  // database
  resource database 'databases' = {
    name: '${workload}-${appEnv}-sqldb'
    location: location
    tags: tags
    sku: {
      name: 'S0'
      tier: 'Standard'
      capacity: 10
    }
    properties: {
      collation: 'SQL_Latin1_General_CP1_CI_AS'
    }
  }

  // firewall rules
  resource firewallRule 'firewallRules' = [
    for item in concat(sqlDatabaseAllowedAzureServices, allowedExternalIpAddresses): if (!empty(item.ipAddress)) {
      name: 'allow-${item.name}-rule'
      properties: {
        startIpAddress: item.ipAddress
        endIpAddress: item.ipAddress
      }
    }
  ]
}

// storage account
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${workload}${appEnv}'
  location: location
  tags: tags
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    allowBlobPublicAccess: true
    allowSharedKeyAccess: true
    defaultToOAuthAuthentication: true
    minimumTlsVersion: 'TLS1_2'
  }

  resource blobServices 'blobServices' = {
    name: 'default'

    resource container 'containers' = {
      name: 'animal-images'
      properties: {
        publicAccess: 'Blob'
      }
    }
  }

  resource queueServices 'queueServices' = {
    name: 'default'

    resource animalDeletedQueue 'queues' = {
      name: 'animal-deleted'
    }

    resource animalImageAddedQueue 'queues' = {
      name: 'animal-image-added'
    }
  }
}

// front end static web app
module staticWebAppModule 'staticWebApp/main.bicep' = {
  name: 'staticWebApp-frontend${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    appName: 'frontend'
    domainName: domainName
    sharedResourceGroup: sharedResourceGroup
    #disable-next-line no-hardcoded-location // static web apps have limited locations
    location: 'eastasia'
  }
}

module appInsightsModule 'appInsights.bicep' = {
  name: 'appInsights-${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    tags: tags
    actionGroupShortName: actionGroupShortName
  }
}

// container apps module
module containerAppsModule './containerApps.bicep' = {
  name: 'containerApps-backend${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    domainName: domainName
    sharedResourceGroup: sharedResourceGroup
    containerRegistryName: containerRegistryName
    apiImageName: apiImageName
    apiImageTag: apiImageTag
    logAnalyticsWorkspaceId: appInsightsModule.outputs.logAnalyticsWorkspaceId
    appConfigurationEndpoint: appConfiguration.properties.endpoint
    corsAllowedOrigins: map(staticWebAppModule.outputs.hostnames, (hostname) => 'https://${hostname}')
  }
}

// jobs function app
module jobsAppModule './functionApp.bicep' = {
  name: 'functionApp-jobs${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    appName: 'jobs'
    location: location
    appConfigEndpoint: appConfiguration.properties.endpoint
    storageAccountName: storageAccount.name
    applicationInsightsConnectionString: appInsightsModule.outputs.connectionString
  }
}

// environment-specific app configuration
module appConfigModule 'appConfig.bicep' = {
  name: 'appConfig-${workload}-${appEnv}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    appConfigurationName: appConfigurationName
    appEnv: appEnv
    applicationInsightsConnectionString: appInsightsModule.outputs.connectionString
    authenticationClientId: authenticationClientId
    authenticationAudience: authenticationAudience
    storageAccountBlobEndpoint: storageAccount.properties.primaryEndpoints.blob
    storageAccountQueueEndpoint: storageAccount.properties.primaryEndpoints.queue
    databaseConnectionString: 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname};Database=${sqlServer::database.name};Authentication="Active Directory Default";'
    containerAppPrincipalId: containerAppsModule.outputs.apiAppPrincipalId
    attemptRoleAssignments: attemptRoleAssignments
  }
}

// role assignments
module roleAssignmentsModule 'roleAssignments.bicep' =
  if (attemptRoleAssignments) {
    name: 'roleAssignments${deploymentSuffix}'
    params: {
      adminGroupObjectId: adminGroupObjectId
      apiAppPrincipalId: containerAppsModule.outputs.apiAppPrincipalId
      functionAppIdentityPrincipalId: jobsAppModule.outputs.identityPrincipalId
      storageAccountName: storageAccount.name
      applicationInsightsName: appInsightsModule.outputs.applicationInsightsName
    }
  }

// shared resource role assignments
module sharedRoleAssignmentsModule 'shared/roleAssignments.bicep' =
  if (attemptRoleAssignments) {
    name: 'roleAssignments-${appEnv}-${deploymentSuffix}'
    scope: resourceGroup(sharedResourceGroup)
    params: {
      appConfigurationName: appConfigurationName
      configurationDataReaders: [
        containerAppsModule.outputs.apiAppPrincipalId
        jobsAppModule.outputs.identityPrincipalId
      ]
    }
  }

@description('The name of the API container app')
output apiAppName string = containerAppsModule.outputs.apiAppName

@description('Name of the jobs function app')
output functionAppName string = jobsAppModule.outputs.functionAppName

@description('Name of the static web app')
output staticWebAppName string = staticWebAppModule.outputs.staticWebAppName

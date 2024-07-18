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

@description('Name of the main container image')
param mainImageName string

@description('Tag of the main container image')
param mainImageTag string

var tags = {
  workload: workload
  appEnv: appEnv
}

var deploymentSuffix = startsWith(deployment().name, 'main-') ? replace(deployment().name, 'main-', '-') : ''

var appConfigurationEndpoint = 'https://${appConfigurationName}.azconfig.io'

module databaseModule 'database.bicep' = {
  name: 'database${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    tags: tags
    adminGroupName: adminGroupName
    adminGroupObjectId: adminGroupObjectId
    allowedExternalIpAddresses: allowedExternalIpAddresses
  }
}

module storageModule 'storage.bicep' = {
  name: 'storage${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    tags: tags
  }
}

module appInsightsModule 'appInsights.bicep' = {
  name: 'appInsights${deploymentSuffix}'
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
  name: 'containerApps${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    domainName: domainName
    sharedResourceGroup: sharedResourceGroup
    containerRegistryName: containerRegistryName
    mainImageName: mainImageName
    mainImageTag: mainImageTag
    logAnalyticsWorkspaceId: appInsightsModule.outputs.logAnalyticsWorkspaceId
    appConfigurationEndpoint: appConfigurationEndpoint
  }
}

// jobs function app
module jobsAppModule './functionApp.bicep' = {
  name: 'functionApp${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    appName: 'jobs'
    location: location
    appConfigEndpoint: appConfigurationEndpoint
    storageAccountName: storageModule.outputs.accountName
    applicationInsightsConnectionString: appInsightsModule.outputs.connectionString
  }
}

// environment-specific app configuration
module appConfigModule 'appConfig.bicep' = {
  name: 'appConfig-${appEnv}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    appConfigurationName: appConfigurationName
    appEnv: appEnv
    applicationInsightsConnectionString: appInsightsModule.outputs.connectionString
    authenticationClientId: authenticationClientId
    authenticationAudience: authenticationAudience
    storageAccountBlobEndpoint: storageModule.outputs.blobEndpoint
    storageAccountQueueEndpoint: storageModule.outputs.queueEndpoint
    databaseConnectionString: databaseModule.outputs.connectionString
    containerAppPrincipalId: containerAppsModule.outputs.mainAppPrincipalId
    attemptRoleAssignments: attemptRoleAssignments
  }
}

// role assignments
module roleAssignmentsModule 'roleAssignments.bicep' =
  if (attemptRoleAssignments) {
    name: 'roleAssignments${deploymentSuffix}'
    params: {
      adminGroupObjectId: adminGroupObjectId
      mainAppPrincipalId: containerAppsModule.outputs.mainAppPrincipalId
      jobsAppIdentityPrincipalId: jobsAppModule.outputs.identityPrincipalId
      storageAccountName: storageModule.outputs.accountName
      applicationInsightsName: appInsightsModule.outputs.applicationInsightsName
    }
  }

// shared resource role assignments
module sharedRoleAssignmentsModule 'shared/roleAssignments.bicep' =
  if (attemptRoleAssignments) {
    name: 'roleAssignments-${appEnv}${deploymentSuffix}'
    scope: resourceGroup(sharedResourceGroup)
    params: {
      appConfigurationName: appConfigurationName
      configurationDataReaders: [
        containerAppsModule.outputs.mainAppPrincipalId
        jobsAppModule.outputs.identityPrincipalId
      ]
    }
  }

@description('The name of the main container app')
output mainAppName string = containerAppsModule.outputs.mainAppName

@description('Name of the jobs function app')
output functionAppName string = jobsAppModule.outputs.functionAppName

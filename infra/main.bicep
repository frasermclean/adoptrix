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
param sharedResourceGroup string = 'adoptrix-shared-rg'

@description('Domain name')
param domainName string

@maxLength(12)
param actionGroupShortName string

@description('Name of the Azure AD B2C tenant')
param b2cTenantName string

@description('Azure AD B2C application client ID')
param b2cAuthClientId string

@description('Azure AD B2C audience')
param b2cAuthAudience string

@description('Azure AD B2C sign-up/sign-in policy ID')
param b2cAuthSignUpSignInPolicyId string

@description('First two octets of the virtual network address space')
param vnetAddressPrefix string = '10.250'

@description('Application administrator group name')
param adminGroupName string

@description('Application administrator group object ID')
param adminGroupObjectId string

@description('Whether to attempt role assignments (requires appropriate permissions)')
param shouldAttemptRoleAssignments bool = false

@description('Array of allowed external IP addresses')
param allowedExternalIpAddresses array = []

var tags = {
  workload: workload
  appEnv: appEnv
}

var deploymentSuffix = startsWith(deployment().name, 'main-')
 ? replace(deployment().name, 'main-', '-')
 : ''

// virtual network
resource virtualNetwork 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  name: '${workload}-${appEnv}-vnet'
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        '${vnetAddressPrefix}.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'apps-subnet'
        properties: {
          addressPrefix: '${vnetAddressPrefix}.1.0/24'
          delegations: [
            {
              name: 'apps-delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          serviceEndpoints: [
            {
              service: 'Microsoft.Sql'
              locations: [ location ]
            }
            {
              service: 'Microsoft.Storage'
              locations: [ location ]
            }
          ]
        }
      }
    ]
  }

  resource appsSubnet 'subnets' existing = {
    name: 'apps-subnet'
  }
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

  // virtual network rule
  resource vnetRule 'virtualNetworkRules' = {
    name: 'apps-subnet-rule'
    properties: {
      virtualNetworkSubnetId: virtualNetwork::appsSubnet.id
      ignoreMissingVnetServiceEndpoint: false
    }
  }

  // firewall rules
  resource firewallRule 'firewallRules' = [for (ipAddress, i) in allowedExternalIpAddresses:{
    name: 'external-ip-${i}-rule'
    properties: {
      startIpAddress: ipAddress
      endIpAddress: ipAddress
    }
  }]
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

// log analytics workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${workload}-${appEnv}-law'
  location: location
  tags: tags
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGb: 1
    }
  }
}

// application insights
resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${workload}-${appEnv}-appi'
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// failure anomalies smart detector
resource failureAnomaliesSmartDetectorRule 'microsoft.alertsManagement/smartDetectorAlertRules@2021-04-01' = {
  name: '${workload}-${appEnv}-fa-sdar'
  location: 'global'
  tags: tags
  properties: {
    state: 'Enabled'
    description: 'Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.'
    severity: 'Sev3'
    frequency: 'PT1M'
    detector: {
      id: 'FailureAnomaliesDetector'
    }
    scope: [
      applicationInsights.id
    ]
    actionGroups: {
      groupIds: [
        actionGroup.id
      ]
    }
  }
}

// action group
resource actionGroup 'Microsoft.Insights/actionGroups@2023-01-01' = {
  name: '${workload}-${appEnv}-ag'
  location: 'global'
  tags: tags
  properties: {
    enabled: true
    groupShortName: actionGroupShortName
    armRoleReceivers: [
      {
        name: 'Email Owner'
        roleId: '8e3af657-a8ff-443c-a75c-2fe8c4bcb635'
        useCommonAlertSchema: true
      }
    ]
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

// back end app service
module appServiceModule './appService/main.bicep' = {
  name: 'appService-backend${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    appName: 'backend'
    location: location
    deploymentSuffix: deploymentSuffix
    domainName: domainName
    b2cAuthAudience: b2cAuthAudience
    b2cAuthClientId: b2cAuthClientId
    b2cAuthSignUpSignInPolicyId: b2cAuthSignUpSignInPolicyId
    b2cTenantName: b2cTenantName
    sqlServerName: sqlServer.name
    sqlDatabaseName: sqlServer::database.name
    storageAccountName: storageAccount.name
    applicationInsightsConnectionString: applicationInsights.properties.ConnectionString
    virtualNetworkSubnetId: virtualNetwork::appsSubnet.id
    corsAllowedOrigins: map(staticWebAppModule.outputs.hostnames, (hostname) => 'https://${hostname}')
  }
}

// jobs function app
module jobsAppModule './functionApp/main.bicep' = {
  name: 'functionApp-jobs${deploymentSuffix}'
  params: {
    workload: workload
    appEnv: appEnv
    appName: 'jobs'
    location: location
    storageAccountName: storageAccount.name
    applicationInsightsConnectionString: applicationInsights.properties.ConnectionString
    azureStorageBlobEndpoint: storageAccount.properties.primaryEndpoints.blob
    azureStorageQueueEndpoint: storageAccount.properties.primaryEndpoints.queue
  }
}

// role assignments
module roleAssignmentsModule 'roleAssignments.bicep' = if (shouldAttemptRoleAssignments) {
  name: 'roleAssignments${deploymentSuffix}'
  params: {
    adminGroupObjectId: adminGroupObjectId
    appServiceIdentityPrincipalId: appServiceModule.outputs.identityPrincipalId
    functionAppIdentityPrincipalId: jobsAppModule.outputs.identityPrincipalId
    storageAccountName: storageAccount.name
  }
}

output appServiceName string = appServiceModule.outputs.appServiceName
output staticWebAppName string = staticWebAppModule.outputs.staticWebAppName

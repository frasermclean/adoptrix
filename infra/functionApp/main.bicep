targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string = 'adoptrix'

@minLength(3)
@description('Environment of the workload')
param appEnv string = 'demo'

@minLength(3)
@description('Name of the application')
param appName string = 'jobs'

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Name of the Azure Storage Account to use for the function app')
param storageAccountName string

@description('Application Insights connection string')
param applicationInsightsConnectionString string

@description('The endpoint for the Azure Storage Blob service')
param azureStorageBlobEndpoint string

@description('The endpoint for the Azure Storage Queue service')
param azureStorageQueueEndpoint string

var tags = {
  workload: workload
  appEnv: appEnv
  appName: appName
}

var storageAccountConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName

  resource fileServices 'fileServices' = {
    name: 'default'

    // create a share for the function app content
    resource functionAppContentShare 'shares' = {
      name: '${appName}-app-content'
    }
  }
}

// consumption app service plan
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: '${workload}-${appEnv}-${appName}-asp'
  location: location
  tags: tags
  kind: 'functionapp'
  sku: {
    name: 'Y1'
  }
}

// function app
resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
  name: '${workload}-${appEnv}-${appName}-func'
  location: location
  tags: tags
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      http20Enabled: true
      ftpsState: 'FtpsOnly'
      netFrameworkVersion: 'v8.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccountConnectionString
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: storageAccountConnectionString
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: storageAccount::fileServices::functionAppContentShare.name
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsightsConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'Recommended'
        }
        {
          name: 'AzureStorage__BlobEndpoint'
          value: azureStorageBlobEndpoint
        }
        {
          name: 'AzureStorage__QueueEndpoint'
          value: azureStorageQueueEndpoint
        }
      ]
    }
  }
}

@description('The name of the function app')
output functionAppName string = functionApp.name

@description('The principal ID of the function app managed identity')
output identityPrincipalId string = functionApp.identity.principalId

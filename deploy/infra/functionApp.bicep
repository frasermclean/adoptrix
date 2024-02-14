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

@description('The endpoint for the Azure App Configuration instance')
param appConfigEndpoint string

@description('Name of the Azure Storage Account to use for the function app')
param storageAccountName string

@description('Application Insights connection string')
param applicationInsightsConnectionString string

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
      use32BitWorkerProcess: false
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccountConnectionString
        }
        {
          name: 'AZURE_FUNCTIONS_ENVIRONMENT'
          value: appEnv
        }
        {
          name: 'APP_CONFIG_ENDPOINT'
          value: appConfigEndpoint
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
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED' // improves cold start time: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#placeholders
          value: '1'
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
      ]
    }
  }
}

@description('The name of the function app')
output functionAppName string = functionApp.name

@description('The principal ID of the function app managed identity')
output identityPrincipalId string = functionApp.identity.principalId

targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Name of the storage account')
param storageAccountName string

@description('Name of the Application Insights instance')
param applicationInsightsName string

var tags = {
  workload: workload
  category: category
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

// consumption app service plan
resource appServicePlan 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: '${workload}-${category}-functions-asp'
  location: location
  tags: tags
  kind: 'functionapp'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

// function app
resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
  name: '${workload}-${category}-functions-app'
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
      appSettings: [
        {
          name: 'AzureWebJobsStorage__accountName'
          value: storageAccount.name
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
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsights.properties.ConnectionString
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
          value: storageAccount.properties.primaryEndpoints.blob
        }
        {
          name: 'AzureStorage__QueueEndpoint'
          value: storageAccount.properties.primaryEndpoints.queue
        }
      ]
    }
  }
}

@description('The principal ID of the function app managed identity')
output identityPrincipalId string = functionApp.identity.principalId

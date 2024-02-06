targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Environment of the application')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Log Analytics workspace to send logs to')
param logAnalyticsWorkspaceName string

@description('Name of the Azure Container Registry')
param containerRegistryName string

@description('Resource group that contains the Azure Container Registry')
param containerRegistryResourceGroup string

@description('Name of the API container image')
param apiImageName string

@description('Name of the Storage Account')
param storageAccountName string

@description('Application Insights connection string')
param applicationInsightsConnectionString string

var tags = {
  workload: workload
  appEnv: appEnv
}

// container registry (existing)
resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: containerRegistryName
  scope: resourceGroup(containerRegistryResourceGroup)
}

// storage account (existing)
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

// log analytics workspace (existing)
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' existing = {
  name: logAnalyticsWorkspaceName
}

// container apps environment
resource appsEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  name: '${workload}-${appEnv}-cae'
  location: location
  tags: tags
  properties: {
    appLogsConfiguration: {
      destination: 'azure-monitor'
    }
  }
}

// container apps environment diagnostic settings
resource appsEnvironmentDiagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'log-analytics'
  scope: appsEnvironment
  properties: {
    workspaceId: logAnalyticsWorkspace.id
    logs: [
      {
        category: 'ContainerAppSystemLogs'
        enabled: true
      }
    ]
  }
}

// adoptrix api container app
resource apiContainerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: '${workload}-${appEnv}-api-ca'
  location: location
  tags: union(tags, { appName: 'api' })
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    environmentId: appsEnvironment.id
    configuration: {
      activeRevisionsMode: 'Single'
      maxInactiveRevisions: 3
      ingress: {
        external: true
        targetPort: 8080
        allowInsecure: false
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
      }
      registries: [
        {
          server: containerRegistry.properties.loginServer
          identity: 'System'
        }
      ]
    }
    template: {
      containers: [
        {
          name: apiImageName
          image: '${containerRegistry.properties.loginServer}/${apiImageName}:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: applicationInsightsConnectionString
            }
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: appEnv
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
      ]
    }
  }
}

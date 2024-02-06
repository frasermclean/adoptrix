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

@description('Name of the shared resource group')
param sharedResourceGroup string

@description('Name of the Azure App Configuration instance')
param appConfigurationName string

@description('Name of the Azure Container Registry')
param containerRegistryName string

@description('Name of the API container image')
param apiImageName string

@description('Azure AD B2C application client ID')
param azureAdClientId string

@description('Azure AD B2C audience')
param azureAdAudience string

@description('Name of the Storage Account')
param storageAccountName string

@description('Application Insights connection string')
param applicationInsightsConnectionString string

@description('Name of the existing SQL server')
param sqlServerName string

@description('Name of the existing SQL database')
param sqlDatabaseName string

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

var tags = {
  workload: workload
  appEnv: appEnv
}

var containerRegistryLoginServer = '${containerRegistryName}${environment().suffixes.acrLoginServer}'

// app configuration (existing)
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-03-01' existing = {
  name: appConfigurationName
  scope: resourceGroup(sharedResourceGroup)
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
      {
        category: 'ContainerAppConsoleLogs'
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
          server: containerRegistryLoginServer
          identity: 'System'
        }
      ]
    }
    template: {
      containers: [
        {
          name: apiImageName
          image: '${containerRegistryLoginServer}/${apiImageName}:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: appEnv
            }
            {
              name: 'APP_CONFIG_ENDPOINT'
              value: appConfiguration.properties.endpoint
            }
          ]
        }
      ]
    }
  }
}

module appConfig 'appConfig.bicep' = {
  name: 'appConfig-${workload}-${appEnv}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    appConfigurationName: appConfigurationName
    appEnv: appEnv
    azureAdClientId: azureAdClientId
    azureAdAudience: azureAdAudience
    applicationInsightsConnectionString: applicationInsightsConnectionString
    storageAccountBlobEndpoint: storageAccount.properties.primaryEndpoints.blob
    storageAccountQueueEndpoint: storageAccount.properties.primaryEndpoints.queue
    databaseConnectionString: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname};Database=${sqlDatabaseName};Authentication="Active Directory Default";'
    containerAppPrincipalId: apiContainerApp.identity.principalId
    attemptRoleAssignments: attemptRoleAssignments
  }
}

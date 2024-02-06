targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Environment of the application')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Resource ID of the Log Analytics workspace')
param logAnalyticsWorkspaceId string

@description('Endpoint of the App Configuration instance')
param appConfigurationEndpoint string

@description('Name of the Azure Container Registry')
param containerRegistryName string

@description('Name of the API container image')
param apiImageName string

var tags = {
  workload: workload
  appEnv: appEnv
}

var containerRegistryLoginServer = '${containerRegistryName}${environment().suffixes.acrLoginServer}'

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
    workspaceId: logAnalyticsWorkspaceId
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
              value: appConfigurationEndpoint
            }
          ]
        }
      ]
    }
  }
}

@description('The principal ID of the API container app managed identity')
output apiAppPrincipalId string = apiContainerApp.identity.principalId

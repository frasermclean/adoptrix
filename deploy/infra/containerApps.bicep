targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Environment of the application')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Domain name')
param domainName string

@description('Name of the shared resource group')
param sharedResourceGroup string

@description('Resource ID of the Log Analytics workspace')
param logAnalyticsWorkspaceId string

@description('Endpoint of the App Configuration instance')
param appConfigurationEndpoint string

@description('Name of the Azure Container Registry')
param containerRegistryName string

@description('Name of the API container image')
param apiImageName string

@description('Tag of the API container image')
param apiImageTag string

@description('Array of front-end origins that are allowed to access the app service')
param corsAllowedOrigins array

var tags = {
  workload: workload
  appEnv: appEnv
}

var containerRegistryLoginServer = '${containerRegistryName}${environment().suffixes.acrLoginServer}'
var apiContainerAppName = '${workload}-${appEnv}-api-ca'

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

  resource commentsCertificate 'managedCertificates' = {
    name: 'api-cert'
    location: location
    tags: tags
    properties: {
      subjectName: dnsRecordsModule.outputs.apiFqdn
      domainControlValidation: 'CNAME'
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

module dnsRecordsModule 'dnsRecords.bicep' = {
  name: 'dnsRecords-${workload}-${appEnv}-api'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    appEnv: appEnv
    domainName: domainName
    apiDefaultHostname: '${apiContainerAppName}.${appsEnvironment.properties.defaultDomain}'
    apiCustomDomainVerificationId: appsEnvironment.properties.customDomainConfiguration.customDomainVerificationId
  }
}

// adoptrix api container app
resource apiContainerApp 'Microsoft.App/containerApps@2023-05-01' = {
  name: apiContainerAppName
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
        customDomains: [
          {
            name: dnsRecordsModule.outputs.apiFqdn
            bindingType: 'SniEnabled'
            certificateId: appsEnvironment::commentsCertificate.id
          }
        ]
        corsPolicy: {
          allowedOrigins: corsAllowedOrigins
        }
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
          image: '${containerRegistryLoginServer}/${apiImageName}:${apiImageTag}'
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
            {
              name: 'OTEL_SERVICE_NAME'
              value: apiContainerAppName
            }
          ]
        }
      ]
    }
  }
}

@description('The name of the API container app')
output apiAppName string = apiContainerApp.name

@description('The principal ID of the API container app managed identity')
output apiAppPrincipalId string = apiContainerApp.identity.principalId

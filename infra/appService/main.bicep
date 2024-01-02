targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Environment of the application')
param appEnv string

@minLength(3)
@description('Name of the application')
param appName string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('String to append to the end of deployment names')
param deploymentSuffix string = ''

@description('Name of the shared resource group')
param sharedResourceGroup string = 'adoptrix-shared-rg'

@description('Domain name')
param domainName string

@description('Name of the existing Azure AD B2C tenant')
param b2cTenantName string

@description('Azure AD B2C application client ID')
param b2cAuthClientId string

@description('Azure AD B2C audience')
param b2cAuthAudience string

@description('Azure AD B2C sign-up/sign-in policy ID')
param b2cAuthSignUpSignInPolicyId string

@description('Application Insights connection string')
param applicationInsightsConnectionString string

@description('Name of the existing storage account')
param storageAccountName string

@description('Name of the existing SQL server')
param sqlServerName string

@description('Name of the existing SQL database')
param sqlDatabaseName string

@description('Resource ID of the subnet to deploy the app service to')
param virtualNetworkSubnetId string

@description('Array of front-end hostnames allowed to access the app service')
param corsAllowedOrigins array

var tags = {
  workload: workload
  appEnv: appEnv
  appName: appName
}

// existing Azure AD B2C tenant
resource b2cTenant 'Microsoft.AzureActiveDirectory/b2cDirectories@2021-04-01' existing = {
  name: '${b2cTenantName}.onmicrosoft.com'
  scope: resourceGroup(sharedResourceGroup)
}

// existing storage account
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

// app service plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${workload}-${appEnv}-${appName}-asp'
  location: location
  tags: tags
  kind: 'linux'
  sku: {
    name: 'B1'
  }
  properties: {
    reserved: true
  }
}

// app service
resource appService 'Microsoft.Web/sites@2022-09-01' = {
  name: '${workload}-${appEnv}-${appName}-app'
  location: location
  tags: tags
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: false
    virtualNetworkSubnetId: virtualNetworkSubnetId
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      http20Enabled: true
      ftpsState: 'FtpsOnly'
      healthCheckPath: '/api/health'
      appSettings: [
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
          value: 'recommended'
        }
        {
          name: 'AzureAd__Instance'
          value: 'https://${b2cTenant.name}.b2clogin.com'
        }
        {
          name: 'AzureAd__Domain'
          value: '${b2cTenant.name}.onmicrosoft.com'
        }
        {
          name: 'AzureAd__TenantId'
          value: b2cTenant.properties.tenantId
        }
        {
          name: 'AzureAd__ClientId'
          value: b2cAuthClientId
        }
        {
          name: 'AzureAd__Audience'
          value: b2cAuthAudience
        }
        {
          name: 'AzureAd__SignUpSignInPolicyId'
          value: b2cAuthSignUpSignInPolicyId
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
      connectionStrings: [
        {
          name: 'AdoptrixDb'
          connectionString: 'Server=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname};Database=${sqlDatabaseName};Authentication="Active Directory Default";'
          type: 'SQLAzure'
        }
      ]
      cors: {
        allowedOrigins: corsAllowedOrigins
      }
    }
  }

  // custom hostname binding - disable ssl initially then enable after certificate is created
  resource hostNameBinding 'hostNameBindings' = {
    name: 'api.${appEnv}.${domainName}'
    properties: {
      sslState: 'Disabled'
    }
    dependsOn: [ dnsRecords ]
  }
}

// custom hostname dns records
module dnsRecords 'dnsRecords.bicep' = {
  name: 'dnsRecords-${appEnv}-${appName}${deploymentSuffix}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    domainName: domainName
    appEnv: appEnv
    appServiceDefaultHostName: appService.properties.defaultHostName
    appServiceDomainVerificationId: appService.properties.customDomainVerificationId
  }
}

// app service certificate
resource appServiceCertificate 'Microsoft.Web/certificates@2022-09-01' = {
  name: '${appService.name}-cert'
  location: location
  tags: tags
  properties: {
    serverFarmId: appServicePlan.id
    canonicalName: appService::hostNameBinding.name
  }
}

// module to enable ssl on the app service
module appServiceSniEnableModule '../modules/siteSniEnable.bicep' = {
  name: 'appServiceSniEnable${deploymentSuffix}'
  params: {
    siteName: appService.name
    certificateThumbprint: appServiceCertificate.properties.thumbprint
    hostname: appServiceCertificate.properties.canonicalName
  }
}

@description('App service name')
output appServiceName string = appService.name

@description('App service principal ID')
output identityPrincipalId string = appService.identity.principalId

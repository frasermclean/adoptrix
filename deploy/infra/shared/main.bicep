targetScope = 'resourceGroup'

@description('Name of the workload')
@minLength(3)
param workload string

@description('Category of the workload')
param category string

@description('Location for non global resources')
param location string = resourceGroup().location

@description('Domain name')
param domainName string

@description('Prefix for the Azure AD B2C tenant name')
param b2cTenantPrefix string

@description('Name of the Azure AD B2C sign-up/sign-in policy')
param b2cSignUpSignInPolicyName string

@description('Array of prinicpal IDs that have read and write access to the configuration data')
param configurationDataOwners array = []

@description('Array of prinicpal IDs that have read access to the configuration data')
param configurationDataReaders array = []

var tags = {
  workload: workload
  category: category
}

// existing Azure AD B2C tenant
resource b2cTenant 'Microsoft.AzureActiveDirectory/b2cDirectories@2021-04-01' existing = {
  name: '${b2cTenantPrefix}.onmicrosoft.com'
}

// dns zone
resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' = {
  name: domainName
  location: 'global'
  tags: tags
  properties: {
    zoneType: 'Public'
  }
}

// app configuration
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-03-01' = {
  name: '${workload}-${category}-ac'
  location: location
  tags: tags
  sku: {
    name: 'Free'
  }
  properties: {
    disableLocalAuth: false
  }

  resource azureAdInstance 'keyValues' = {
    name: 'AzureAd:Instance'
    properties: {
      value: 'https://${b2cTenantPrefix}.b2clogin.com'
      contentType: 'text/plain'
    }
  }

  resource azureAdDomain 'keyValues' = {
    name: 'AzureAd:Domain'
    properties: {
      value: b2cTenant.name
      contentType: 'text/plain'
    }
  }

  resource azureAdTenantId 'keyValues' = {
    name: 'AzureAd:TenantId'
    properties: {
      value: b2cTenant.properties.tenantId
      contentType: 'text/plain'
    }
  }

  resource azureAdSignUpSignInPolicyId 'keyValues' = {
    name: 'AzureAd:SignUpSignInPolicyId'
    properties: {
      value: b2cSignUpSignInPolicyName
      contentType: 'text/plain'
    }
  }
}

module roleAssignments 'roleAssignments.bicep' = {
  name: 'roleAssignments'
  params: {
    appConfigurationName: appConfiguration.name
    configurationDataOwners: configurationDataOwners
    configurationDataReaders: configurationDataReaders
  }
}

output dnsZoneNameServers array = dnsZone.properties.nameServers

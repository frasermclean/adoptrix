targetScope = 'resourceGroup'

@description('Name of the workload')
@minLength(3)
param workload string

@description('Category of the workload')
param category string

@description('Location for non global resources')
param location string = resourceGroup().location

@description('Domain name')
param domainName string = 'adoptrix.com'

@description('Array of prinicpal IDs that have read and write access to the configuration data')
param configurationDataOwners array = []

@description('Array of prinicpal IDs that have read access to the configuration data')
param configurationDataReaders array = []

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

@description('Suffix for child deployments')
param deploymentSuffix string = ''

var tags = {
  workload: workload
  category: category
}

// dns zone
resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' = {
  name: domainName
  location: 'global'
  tags: tags
  properties: {
    zoneType: 'Public'
  }

  // entra tenant verification
  resource dnsRecord 'TXT' = {
    name: '@'
    properties: {
      TTL: 3600
      TXTRecords: [
        {
          value: ['MS=ms14036913']
        }
      ]
    }
  }
}

// entra id directory
resource entraIdDirectory 'Microsoft.AzureActiveDirectory/ciamDirectories@2023-05-17-preview' existing = {
  name: '${workload}.onmicrosoft.com'
}

// user assigned managed identity
resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${workload}-${category}-id'
  location: location
  tags: tags
}

// app configuration
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-09-01-preview' = {
  name: '${workload}-${category}-ac'
  location: location
  tags: tags
  sku: {
    name: 'Free'
  }
  properties: {
    disableLocalAuth: true
    dataPlaneProxy: {
      authenticationMode: 'Pass-through'
    }
  }

  resource authenticationInstanceKeyValue 'keyValues' = {
    name: 'Authentication:Instance'
    properties: {
      value: 'https://${workload}.ciamlogin.com'
      contentType: 'text/plain'
    }
  }

  resource authenticationTenantIdKeyValue 'keyValues' = {
    name: 'Authentication:TenantId'
    properties: {
      value: entraIdDirectory.properties.tenantId
      contentType: 'text/plain'
    }
  }
}

module roleAssignments 'roleAssignments.bicep' = if (attemptRoleAssignments) {
  name: 'roleAssignments${deploymentSuffix}'
  params: {
    appConfigurationName: appConfiguration.name
    configurationDataOwners: configurationDataOwners
    configurationDataReaders: configurationDataReaders
  }
}

output dnsZoneName string = dnsZone.name

output appConfigurationName string = appConfiguration.name

@description('Shared managed identity principal ID')
output sharedIdentityPrincipalId string = managedIdentity.properties.principalId

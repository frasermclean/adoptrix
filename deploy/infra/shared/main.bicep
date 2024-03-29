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

@description('Microsoft Entra instance')
param authenticationInstance string

@description('Microsoft Entra tenant ID')
param authenticationTenantId string

@description('Array of prinicpal IDs that have read and write access to the configuration data')
param configurationDataOwners array = []

@description('Array of prinicpal IDs that have read access to the configuration data')
param configurationDataReaders array = []

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

  resource authenticationInstanceKeyValue 'keyValues' = {
    name: 'Authentication:Instance'
    properties: {
      value: authenticationInstance
      contentType: 'text/plain'
    }
  }

  resource authenticationTenantIdKeyValue 'keyValues' = {
    name: 'Authentication:TenantId'
    properties: {
      value: authenticationTenantId
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

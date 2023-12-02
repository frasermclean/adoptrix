targetScope = 'resourceGroup'
@description('Name of the workload')
param workload string

@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Address prefix for the virtual network in CIDR notation')
param vnetAddressPrefix string = '10.250.0.0/16'

var tags = {
  workload: workload
  category: category
}

// virtual network
resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  name: '${workload}-${category}-vnet'
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        vnetAddressPrefix
      ]
    }
  }
}

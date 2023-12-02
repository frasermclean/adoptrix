targetScope = 'resourceGroup'
@description('Name of the workload')
param workload string

@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('First two octets of the virtual network address space')
param vnetAddressPrefix string = '10.250'

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
        '${vnetAddressPrefix}.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'apps-subnet'
        properties: {
          addressPrefix: '${vnetAddressPrefix}.1.0/24'
          delegations: [
            {
              name: 'apps-delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
        }
      }
      {
        name: 'data-subnet'
        properties: {
          addressPrefix: '${vnetAddressPrefix}.2.0/24'
        }
      }
    ]
  }
}

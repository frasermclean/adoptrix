targetScope = 'resourceGroup'
@description('Name of the workload')
param workload string

@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('First two octets of the virtual network address space')
param vnetAddressPrefix string = '10.250'

@description('Login for the SQL Server administrator')
param sqlAdministratorLogin string

@secure()
@description('Password for the SQL Server administrator')
param sqlAdministratorSid string

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
          serviceEndpoints: [
            {
              service: 'Microsoft.Sql'
              locations: [
                location
              ]
            }
          ]
        }
      }
    ]
  }

  resource appsSubnet 'subnets' existing = {
    name: 'apps-subnet'
  }
}

// azure sql server
resource sqlServer 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: '${workload}-${category}-sql'
  location: location
  tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: sqlAdministratorLogin
      principalType: 'User'
      sid: sqlAdministratorSid
      tenantId: subscription().tenantId
    }
    minimalTlsVersion: '1.2'
    version: '12.0'
    publicNetworkAccess: 'Enabled'
  }

  resource vnetRule 'virtualNetworkRules' = {
    name: 'apps-subnet-rule'
    properties: {
      virtualNetworkSubnetId: vnet::appsSubnet.id
      ignoreMissingVnetServiceEndpoint: false
    }
  }
}

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Application environment')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Tags for the resources')
param tags object = {
  workload: workload
  appEnv: appEnv
}

@description('Application administrator group name')
param adminGroupName string

@description('Application administrator group object ID')
param adminGroupObjectId string

@description('Array of allowed external IP addresses. Needs to be an array of objects with name and ipAddress properties.')
param allowedExternalIpAddresses array = []

var allowedAzureServices = [
  {
    name: 'azure'
    ipAddress: '0.0.0.0'
  }
]

// azure sql server
resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: '${workload}-${appEnv}-sql'
  location: location
  tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: adminGroupName
      sid: adminGroupObjectId
      principalType: 'Group'
      tenantId: subscription().tenantId
    }
    minimalTlsVersion: '1.2'
    version: '12.0'
    publicNetworkAccess: 'Enabled'
  }

  // database
  resource database 'databases' = {
    name: '${workload}-${appEnv}-sqldb'
    location: location
    tags: tags
    sku: {
      name: 'S0'
      tier: 'Standard'
      capacity: 10
    }
    properties: {
      collation: 'SQL_Latin1_General_CP1_CI_AS'
    }
  }

  // firewall rules
  resource firewallRule 'firewallRules' = [
    for item in concat(allowedAzureServices, allowedExternalIpAddresses): if (!empty(item.ipAddress)) {
      name: 'allow-${item.name}-rule'
      properties: {
        startIpAddress: item.ipAddress
        endIpAddress: item.ipAddress
      }
    }
  ]
}

@description('Connection string for the database that uses Azure AD authentication')
output connectionString string = 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname};Database=${sqlServer::database.name};Connection Timeout=30;Authentication="Active Directory Default";'

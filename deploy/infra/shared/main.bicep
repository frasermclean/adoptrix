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

@secure()
@description('Password for the container registry')
param containerRegistryPassword string

@description('Expiry date for the container registry password in ISO 8601 format')
#disable-next-line secure-secrets-in-params
param containerRegistryPasswordExpiry string

@description('Array of prinicpal IDs that have administrative roles')
param adminPrincipalIds array = []

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

@description('Suffix for child deployments')
param deploymentSuffix string = ''

param now string = utcNow()

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

// key vault
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: '${workload}-${category}-kv'
  location: location
  properties: {
    enabledForTemplateDeployment: true
    enableRbacAuthorization: true
    tenantId: tenant().tenantId
    sku: {
      name: 'standard'
      family: 'A'
    }
  }

  resource containerRegistryPasswordSecret 'secrets' = if (!empty(containerRegistryPassword)) {
    name: 'container-registry-password'
    properties: {
      value: containerRegistryPassword
      contentType: 'text/plain'
      attributes: {
        enabled: true
        nbf: dateTimeToEpoch(now)
        exp: dateTimeToEpoch(containerRegistryPasswordExpiry)
      }
    }
  }
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
    keyVaultName: keyVault.name
    keyVaultAdministrators: adminPrincipalIds
    keyVaultSecretsUsers: [managedIdentity.properties.principalId]
    appConfigurationName: appConfiguration.name
    configurationDataOwners: adminPrincipalIds
  }
}

output dnsZoneName string = dnsZone.name
output keyVaultName string = keyVault.name
output appConfigurationName string = appConfiguration.name

@description('Shared managed identity principal ID')
output sharedIdentityPrincipalId string = managedIdentity.properties.principalId

targetScope = 'resourceGroup'

param appConfigurationName string

@description('Array of prinicpal IDs that have read and write access to the configuration data')
param configurationDataOwners array = []

@description('Array of prinicpal IDs that have read access to the configuration data')
param configurationDataReaders array = []

// existing app configuration to assign roles to
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-03-01' existing = {
  name: appConfigurationName
}

// app configuration data owner role assignments
var appConfigurationDataOwnerRoleId = '5ae67dd6-50cb-40e7-96ff-dc2bfa4b606b'
resource configurationDataOwnerRoleAssigment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in configurationDataOwners: {
  name: guid(appConfiguration.id, appConfigurationDataOwnerRoleId, principalId)
  scope: appConfiguration
  properties: {
    principalId: principalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions@2022-04-01', appConfigurationDataOwnerRoleId)
  }
}]

// app configuration data reader role assignments
var appConfigurationDataReaderRoleId = '516239f1-63e1-4d78-a4de-a74fb236a071'
resource configurationDataReaderRoleAssigment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in configurationDataReaders: {
  name: guid(appConfiguration.id, appConfigurationDataReaderRoleId, principalId)
  scope: appConfiguration
  properties: {
    principalId: principalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions@2022-04-01', appConfigurationDataReaderRoleId)
  }
}]

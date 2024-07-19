targetScope = 'resourceGroup'

@description('The object id of the Azure AD group that will be granted admin access')
param adminGroupObjectId string

@description('Prinicpal id of the API application managed identity')
param apiAppPrincipalId string

@description('Prinicpal id of the jobs application managed identity')
param jobsAppIdentityPrincipalId string

@description('The name of the storage account to grant access to')
param storageAccountName string

@description('The name of the application insights instance to grant access to')
param applicationInsightsName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
}

var roleIds = {
  storageBlobDataOwner: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
  storageBlobDataContributor: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
  storageAccountContributor: '17d1049b-9a84-46fb-8f53-869881c3d3ab'
  storageQueueDataContributor: '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
  monitoringMetricsPublisher: '3913510d-42f4-4e42-8a64-420c390055eb'
}

var storageAccountRoleAssignmentData = [
  {
    principalId: adminGroupObjectId
    roleId: roleIds.storageBlobDataOwner
  }
  {
    principalId: apiAppPrincipalId
    roleId: roleIds.storageBlobDataContributor
  }
  {
    principalId: apiAppPrincipalId
    roleId: roleIds.storageQueueDataContributor
  }
  {
    principalId: jobsAppIdentityPrincipalId
    roleId: roleIds.storageBlobDataOwner
  }
  {
    principalId: jobsAppIdentityPrincipalId
    roleId: roleIds.storageAccountContributor
  }
  {
    principalId: jobsAppIdentityPrincipalId
    roleId: roleIds.storageQueueDataContributor
  }
]

// storage account role assignments
resource storageAccountRoleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [
  for item in storageAccountRoleAssignmentData: {
    name: guid(storageAccountName, item.principalId, item.roleId)
    scope: storageAccount
    properties: {
      roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions@2022-04-01', item.roleId)
      principalId: item.principalId
    }
  }
]

var applicationInsightsRoleData = [
  {
    roleId: roleIds.monitoringMetricsPublisher
    principalId: apiAppPrincipalId
  }
  {
    roleId: roleIds.monitoringMetricsPublisher
    principalId: jobsAppIdentityPrincipalId
  }
]

// application insights role assignments
resource applicationInsightsRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [
  for item in applicationInsightsRoleData: {
    name: guid(applicationInsightsName, item.principalId, item.roleId)
    scope: applicationInsights
    properties: {
      roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions@2022-04-01', item.roleId)
      principalId: item.principalId
    }
  }
]

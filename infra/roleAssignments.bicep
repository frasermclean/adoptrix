targetScope = 'resourceGroup'

@description('The object id of the Azure AD group that will be granted admin access')
param adminGroupObjectId string

@description('Prinicpal id of the app service managed identity')
param appServiceIdentityPrincipalId string

@description('The name of the storage account to grant access to')
param storageAccountName string

resource storageaccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource storageBlobDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource storageBlobDataContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

var roleAssignmentData = [
  {
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalId: adminGroupObjectId
    principalType: 'Group'
  }
  {
    roleDefinitionId: storageBlobDataContributorRoleDefinition.id
    principalId: appServiceIdentityPrincipalId
    principalType: 'ServicePrincipal'
  }
]

// storage account role assignments
resource storageAccountContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for item in roleAssignmentData: {
  name: guid(storageAccountName, item.principalId, item.roleDefinitionId, item.principalType)
  scope: storageaccount
  properties: {
    roleDefinitionId: item.roleDefinitionId
    principalId: item.principalId
    principalType: item.principalType
  }
}]

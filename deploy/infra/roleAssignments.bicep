targetScope = 'resourceGroup'

@description('The object id of the Azure AD group that will be granted admin access')
param adminGroupObjectId string

@description('Prinicpal id of the app service managed identity')
param appServiceIdentityPrincipalId string

@description('Prinicpal id of the function app managed identity')
param functionAppIdentityPrincipalId string

@description('The name of the storage account to grant access to')
param storageAccountName string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource storageBlobDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource storageBlobDataContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

resource storageAccountContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '17d1049b-9a84-46fb-8f53-869881c3d3ab'
}

resource storageQueueDataContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '974c5e8b-45b9-4653-ba55-5f855dd0fb88'
}

var roleAssignmentData = [
  {
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalId: adminGroupObjectId
  }
  {
    roleDefinitionId: storageBlobDataContributorRoleDefinition.id
    principalId: appServiceIdentityPrincipalId
  }
  {
    roleDefinitionId: storageQueueDataContributorRoleDefinition.id
    principalId: appServiceIdentityPrincipalId
  }
  {
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalId: functionAppIdentityPrincipalId
  }
  {
    roleDefinitionId: storageAccountContributorRoleDefinition.id
    principalId: functionAppIdentityPrincipalId
  }
  {
    roleDefinitionId: storageQueueDataContributorRoleDefinition.id
    principalId: functionAppIdentityPrincipalId
  }
]

// storage account role assignments
resource storageAccountContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for item in roleAssignmentData: {
  name: guid(storageAccountName, item.roleDefinitionId, item.principalId)
  scope: storageAccount
  properties: {
    roleDefinitionId: item.roleDefinitionId
    principalId: item.principalId
  }
}]

targetScope = 'resourceGroup'

@description('Container registry name')
param containerRegistryName string

@description('Array of principal IDs that have pull access to the ACR')
param pullPrinicpalIds array = []

@description('Array of principal IDs that have push access to the ACR')
param pushPrinicpalIds array = []

var acrPullRoleDefinitionId = '7f951dda-4ed3-4680-a7ca-43fe172d538d'
var acrPushRoleDefinitionId = '8311e382-0749-4cb8-b61a-304f252e45ec'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: containerRegistryName
}

// acr pull role assignments
resource acrPullRoleAssigments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in pullPrinicpalIds: {
  name: guid(containerRegistry.id, acrPullRoleDefinitionId, principalId)
  scope: containerRegistry
  properties: {
    principalId: principalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', acrPullRoleDefinitionId)
  }
}]

// acr push role assignments
resource acrPushRoleAssigments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for principalId in pushPrinicpalIds: {
  name: guid(containerRegistry.id, acrPushRoleDefinitionId, principalId)
  scope: containerRegistry
  properties: {
    principalId: principalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', acrPushRoleDefinitionId)
  }
}]

targetScope = 'resourceGroup'

@description('Container registry name')
param containerRegistryName string

@description('Principal ID to assign role to')
param principalId string

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: containerRegistryName
}

resource acrPullRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '7f951dda-4ed3-4680-a7ca-43fe172d538d'
}

resource acrPullRoleAssigment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(containerRegistry.id, acrPullRoleDefinition.id, principalId)
  scope: containerRegistry
  properties: {
    principalId: principalId
    roleDefinitionId: acrPullRoleDefinition.id
  }
}

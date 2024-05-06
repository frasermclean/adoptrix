targetScope = 'resourceGroup'

@description('The object id of the Azure AD group that will be granted admin access')
param adminGroupObjectId string

@description('Prinicpal id of the API application managed identity')
param apiAppPrincipalId string

@description('Prinicpal id of the function app managed identity')
param functionAppIdentityPrincipalId string

@description('The name of the storage account to grant access to')
param storageAccountName string

@description('The name of the application insights instance to grant access to')
param applicationInsightsName string

param azureOpenAiName string

var roleDefinitionIds = {
  StorageBlobDataOwner: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
  StorageBlobDataContributor: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
  CognitiveServicesOpenAiUser: '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd'
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
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

resource monitoringMetricsPublisherRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '3913510d-42f4-4e42-8a64-420c390055eb'
}

var roleAssignmentData = [
  {
    roleDefinitionId: storageBlobDataOwnerRoleDefinition.id
    principalId: adminGroupObjectId
  }
  {
    roleDefinitionId: storageBlobDataContributorRoleDefinition.id
    principalId: apiAppPrincipalId
  }
  {
    roleDefinitionId: storageQueueDataContributorRoleDefinition.id
    principalId: apiAppPrincipalId
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
resource storageAccountContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [
  for item in roleAssignmentData: {
    name: guid(storageAccountName, item.roleDefinitionId, item.principalId)
    scope: storageAccount
    properties: {
      roleDefinitionId: item.roleDefinitionId
      principalId: item.principalId
    }
  }
]

var applicationInsightsRoleData = [
  {
    roleDefinitionId: monitoringMetricsPublisherRoleDefinition.id
    principalId: apiAppPrincipalId
  }
  {
    roleDefinitionId: monitoringMetricsPublisherRoleDefinition.id
    principalId: functionAppIdentityPrincipalId
  }
]

// application insights role assignments
resource applicationInsightsRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [
  for item in applicationInsightsRoleData: {
    name: guid(applicationInsightsName, item.roleDefinitionId, item.principalId)
    scope: applicationInsights
    properties: {
      roleDefinitionId: item.roleDefinitionId
      principalId: item.principalId
    }
  }
]

resource azureOpenAi 'Microsoft.CognitiveServices/accounts@2023-10-01-preview' existing = {
  name: azureOpenAiName
}

resource azureOpenAiRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(azureOpenAiName, roleDefinitionIds.CognitiveServicesOpenAiUser, apiAppPrincipalId)
  scope: azureOpenAi
  properties: {
    principalId: apiAppPrincipalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionIds.CognitiveServicesOpenAiUser)
  }
}

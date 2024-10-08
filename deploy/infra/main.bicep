targetScope = 'subscription'

@description('Name of the workload')
param workload string

@description('Location of the resources')
param location string = deployment().location

@description('Application environment')
param appEnv string

@description('Azure AD B2C application client ID')
param authenticationClientId string

@description('Azure AD B2C audience')
param authenticationAudience string

@description('Application administrator group name')
param adminGroupName string

@description('Application administrator group object ID')
param adminGroupObjectId string

@description('Container registry name')
param containerRegistryName string

@description('Username to access the container registry')
param containerRegistryUsername string

@secure()
@description('Password for the container registry')
param containerRegistryPassword string

@description('Expiry date for the container registry password in ISO 8601 format')
#disable-next-line secure-secrets-in-params
param containerRegistryPasswordExpiry string

@description('Repository of the API container image')
param apiImageRepository string

@description('Tag of the API container image')
param apiImageTag string

@description('Suffix for child deployments')
param deploymentSuffix string = ''

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

@description('Array of allowed external IP addresses. Needs to be an array of objects with name and ipAddress properties.')
param allowedExternalIpAddresses array = []

// shared resource group
resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: '${workload}-shared-rg'
  location: location
  tags: {
    workload: workload
    category: 'shared'
  }
}

// shared resources
module sharedResources 'shared/main.bicep' = {
  name: 'sharedResources${deploymentSuffix}'
  scope: resourceGroup(sharedResourceGroup.name)
  params: {
    workload: workload
    category: 'shared'
    location: location
    containerRegistryPassword: containerRegistryPassword
    containerRegistryPasswordExpiry: containerRegistryPasswordExpiry
    attemptRoleAssignments: attemptRoleAssignments
    adminPrincipalIds: [
      ghActionsApp.outputs.servicePrincipalId
    ]
    deploymentSuffix: deploymentSuffix
  }
}

// app environment resource group
resource appResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: '${workload}-${appEnv}-rg'
  location: location
  tags: {
    workload: workload
    category: 'app'
    environment: appEnv
  }
}

// app environment resources
module appResources 'apps/main.bicep' = {
  name: 'appResources${deploymentSuffix}'
  scope: resourceGroup(appResourceGroup.name)
  params: {
    workload: workload
    location: location
    appEnv: appEnv
    domainName: sharedResources.outputs.dnsZoneName
    actionGroupShortName: 'AdoptrixDemo'
    authenticationAudience: authenticationAudience
    authenticationClientId: authenticationClientId
    sharedResourceGroup: sharedResourceGroup.name
    keyVaultName: sharedResources.outputs.keyVaultName
    appConfigurationName: sharedResources.outputs.appConfigurationName
    adminGroupName: adminGroupName
    adminGroupObjectId: adminGroupObjectId
    containerRegistryName: containerRegistryName
    containerRegistryUsername: containerRegistryUsername
    apiImageTag: apiImageTag
    apiImageRepository: apiImageRepository
    attemptRoleAssignments: attemptRoleAssignments
    allowedExternalIpAddresses: allowedExternalIpAddresses
    deploymentSuffix: deploymentSuffix
  }
}

// GitHub Actions application
module ghActionsApp 'ghActionsApp.bicep' = if (attemptRoleAssignments) {
  name: '${workload}-ghActionsApp-${appEnv}${deploymentSuffix}'
  params: {
    repositoryName: 'frasermclean/adoptrix'
    appEnv: appEnv
  }
}

var contributorRoleDefinitionId = 'b24988ac-6180-42a0-ab88-20f7382dd24c'

// shared resource group role assignment
module sharedResourceGroupRoleAssignment 'rgRoleAssignment.bicep' = if (attemptRoleAssignments) {
  name: 'sharedResourcesRoleAssigments${deploymentSuffix}'
  scope: sharedResourceGroup
  params: {
    principalId: ghActionsApp.outputs.servicePrincipalId
    roleDefinitionId: contributorRoleDefinitionId
  }
}

// app resource group role assignment
module appResourceGroupRoleAssignment 'rgRoleAssignment.bicep' = if (attemptRoleAssignments) {
  name: 'appResourcesRoleAssigments${deploymentSuffix}'
  scope: appResourceGroup
  params: {
    principalId: ghActionsApp.outputs.servicePrincipalId
    roleDefinitionId: contributorRoleDefinitionId
  }
}

@description('The name of the app resource group')
output appResourceGroup string = appResourceGroup.name

@description('The name of the API container app')
output apiAppName string = appResources.outputs.apiAppName

@description('Name of the jobs function app')
output functionAppName string = appResources.outputs.functionAppName

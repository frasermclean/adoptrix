targetScope = 'resourceGroup'

@description('Name of the Azure App Configuration instance')
param appConfigurationName string

@minLength(3)
@description('Environment of the application')
param appEnv string

@description('Authentication client ID')
param authenticationClientId string

@description('Authentication audience')
param authenticationAudience string

@description('Storage account blob endpoint to be stored in App Configuration.')
param storageAccountBlobEndpoint string

@description('Storage account queue endpoint to be stored in App Configuration.')
param storageAccountQueueEndpoint string

@description('Database connection string to be stored in App Configuration.')
param databaseConnectionString string

@description('Principal ID of the API application managed identity')
param apiAppPrincipalId string

@description('Name of the Azure Key Vault instance')
param keyVaultName string

@description('Whether to attempt role assignments (requires appropriate permissions)')
param attemptRoleAssignments bool

resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2023-03-01' existing = {
  name: appConfigurationName

  resource authenticationClientIdKeyValue 'keyValues' = {
    name: 'Authentication:ClientId$${appEnv}'
    properties: {
      value: authenticationClientId
      contentType: 'text/plain'
    }
  }

  resource authenticationAudienceKeyValue 'keyValues' = {
    name: 'Authentication:Audience$${appEnv}'
    properties: {
      value: authenticationAudience
      contentType: 'text/plain'
    }
  }

  resource databaseConnectionStringKeyValue 'keyValues' = {
    name: 'ConnectionStrings:database$${appEnv}'
    properties: {
      value: databaseConnectionString
      contentType: 'text/plain'
    }
  }

  resource blobStorageConnectionStringKeyValue 'keyValues' = {
    name: 'ConnectionStrings:blob-storage$${appEnv}'
    properties: {
      value: storageAccountBlobEndpoint
      contentType: 'text/plain'
    }
  }

  resource queueStorageConnectionStringKeyValue 'keyValues' = {
    name: 'ConnectionStrings:queue-storage$${appEnv}'
    properties: {
      value: storageAccountQueueEndpoint
      contentType: 'text/plain'
    }
  }

  resource userManagerClientSecretKeyValue 'keyValues' = {
    name: 'UserManager:ClientSecret$${appEnv}'
    properties: {
      value: '{"uri":"https://${keyVaultName}${environment().suffixes.keyvaultDns}/secrets/user-manager-demo-client-secret"}'
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
    }
  }
}

var appConfigurationDataReaderRoleId = '516239f1-63e1-4d78-a4de-a74fb236a071'

// app configuration data reader role assignment
resource configurationDataOwnerRoleAssigment 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (attemptRoleAssignments) {
  name: guid(appConfiguration.id, appConfigurationDataReaderRoleId, apiAppPrincipalId)
  scope: appConfiguration
  properties: {
    principalId: apiAppPrincipalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions@2022-04-01', appConfigurationDataReaderRoleId)
  }
}

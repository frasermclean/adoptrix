targetScope = 'subscription'

extension microsoftGraph

@description('Name of the GitHub repository in the format owner/repo')
param repositoryName string

@description('Application environment name')
param appEnv string = 'demo'

// application registration
resource application 'Microsoft.Graph/applications@v1.0' = {
  displayName: 'Adoptrix GitHub Actions - ${appEnv}'
  uniqueName: 'adoptrix-github-actions-${appEnv}'
  web: {
    homePageUrl: 'https://${appEnv}.adoptrix.com'
  }

  resource demoCredentials 'federatedIdentityCredentials' = {
    name: '${application.uniqueName}/${appEnv}-env'
    audiences: ['api://AzureADTokenExchange']
    issuer: 'https://token.actions.githubusercontent.com'
    subject: 'repo:${repositoryName}:environment:${appEnv}'
    description: 'GitHub Actions for Adoptrix ${appEnv} environment'
  }
}

// service principal
resource servicePrincipal 'Microsoft.Graph/servicePrincipals@v1.0' = {
  appId: application.appId
}

@description('Service principal object ID. Can be used for role assignments')
output servicePrincipalId string = servicePrincipal.id

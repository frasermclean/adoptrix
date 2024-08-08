targetScope = 'subscription'

extension microsoftGraph

@allowed(['demo'])
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
    subject: 'repo:frasermclean/adoptrix:environment:${appEnv}'
    description: 'GitHub Actions for Adoptrix ${appEnv} environment'
  }
}

// service principal
resource servicePrincipal 'Microsoft.Graph/servicePrincipals@v1.0' = {
  appId: application.appId
}

@description('Application ID (Client ID) or the service principal')
output servicePrincipalId string = servicePrincipal.appId

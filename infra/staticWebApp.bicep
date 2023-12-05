@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Category of the workload')
param category string

@allowed([
  'eastasia'
  'centralus'
  'eastus2'
  'westeurope'
  'westus2'
])
@description('Location of the static web app')
param location string

@description('Resource ID of the app service that will be linked to the static web app')
param appServiceResourceId string

@description('Location of the app service that will be linked to the static web app')
param appServiceLocation string

var tags = {
  workload: workload
  category: category
}

// static web app
resource staticWebApp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: '${workload}-${category}-swa'
  location: location
  tags: tags
  sku: {
    name: 'Standard'
  }
  properties: {
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    buildProperties: {
      skipGithubActionWorkflowGeneration: true
    }
  }

  // link to app service
  resource linkedBackend 'linkedBackends' = {
    name: 'appService'
    properties: {
      backendResourceId: appServiceResourceId
      region: appServiceLocation
    }
  }
}

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Category of the workload')
param category string

@description('Domain name')
param domainName string

@description('Name of the shared resource group')
param sharedResourceGroup string

@allowed([
  'eastasia'
  'centralus'
  'eastus2'
  'westeurope'
  'westus2'
])
@description('Location of the static web app')
param location string

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
    name: 'Free'
  }
  properties: {
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    buildProperties: {
      skipGithubActionWorkflowGeneration: true
    }
  }

  // custom domain
  resource customDomain 'customDomains' = {
    name: '${category}.${domainName}'
    dependsOn: [ swaDnsRecordModule ]
  }
}

module swaDnsRecordModule 'swaDnsRecord.bicep' = {
  name: replace(deployment().name, 'staticWebApp', 'swaDnsRecord-${category}')
  scope: resourceGroup(sharedResourceGroup)
  params: {
    domainName: domainName
    hostName: category
    swaDefaultHostName: staticWebApp.properties.defaultHostname
  }
}

output hostnames string[] = [
  staticWebApp.properties.defaultHostname
  staticWebApp::customDomain.name
]

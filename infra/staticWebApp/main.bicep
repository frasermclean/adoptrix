@minLength(3)
@description('Name of the workload')
param workload string

@description('Environment of the application')
param appEnv string

@minLength(3)
@description('Name of the application')
param appName string

@description('Domain name')
param domainName string

@description('String to append to the end of deployment names')
param deploymentSuffix string = ''

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
  appEnv: appEnv
  appName: appName
}

// static web app
resource staticWebApp 'Microsoft.Web/staticSites@2022-03-01' = {
  name: '${workload}-${appEnv}-${appName}-swa'
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
    name: '${appEnv}.${domainName}'
    dependsOn: [ dnsRecordsModule ]
  }
}

module dnsRecordsModule 'dnsRecords.bicep' = {
  name: 'dnsRecords-${appEnv}-${appName}${deploymentSuffix}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    domainName: domainName
    hostName: appEnv
    swaDefaultHostName: staticWebApp.properties.defaultHostname
  }
}

output staticWebAppName string = staticWebApp.name

output hostnames string[] = [
  staticWebApp.properties.defaultHostname
  staticWebApp::customDomain.name
]

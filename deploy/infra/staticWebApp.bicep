@minLength(3)
@description('Name of the workload')
param workload string

@description('Environment of the application')
param appEnv string

@allowed([
  'eastasia'
  'centralus'
  'eastus2'
  'westeurope'
  'westus2'
])
@description('Location of the static web app')
param location string

@minLength(3)
@description('Name of the application')
param appName string

@description('Tags for the resources')
param tags object = {
  workload: workload
  appEnv: appEnv
}

@description('Domain name')
param domainName string

@description('Name of the shared resource group')
param sharedResourceGroup string

@description('String to append to the end of deployment names')
param deploymentSuffix string = ''

// static web app
resource staticWebApp 'Microsoft.Web/staticSites@2023-12-01' = {
  name: '${workload}-${appEnv}-${appName}-swa'
  location: location
  tags: union(tags, {
    appName: appName
  })
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
    dependsOn: [dnsRecordsModule]
  }
}

module dnsRecordsModule 'dnsRecords.bicep' = {
  name: 'dnsRecords-${appEnv}-${appName}${deploymentSuffix}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    domainName: domainName
    appEnv: appEnv
    clientAppDefaultHostname: staticWebApp.properties.defaultHostname
  }
}

output staticWebAppName string = staticWebApp.name

output hostnames string[] = [
  staticWebApp.properties.defaultHostname
  staticWebApp::customDomain.name
]

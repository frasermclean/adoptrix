targetScope = 'resourceGroup'

/*
This template defines Azure resources for use in a development environment.
*/

param workload string
param appEnv string
param location string = resourceGroup().location
param principalIds array = []

@maxLength(12)
@description('Short name for the action group')
param actionGroupShortName string

module appInsightsModule 'appInsights.bicep' = {
  name: 'appInsights'
  params: {
    workload: workload
    appEnv: appEnv
    location: location
    monitoringMetricsPublishers: principalIds
    actionGroupShortName: actionGroupShortName
  }
}

output applicationInsightsConnectionString string = appInsightsModule.outputs.connectionString

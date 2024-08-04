@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Application environment')
param appEnv string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Tags to be applied to the resources')
param tags object = {
  workload: workload
  appEnv: appEnv
}

@description('Log Analytics workspace ID - if empty, a new workspace will be created')
param logAnalyticsWorkspaceId string = ''

@description('Set to true to force authentication via Entra ID')
param disableLocalAuth bool = false

@description('An array of principal IDs to be assigned the Monitoring Metrics Publisher role')
param monitoringMetricsPublishers array = []

@maxLength(12)
param actionGroupShortName string

// log analytics workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = if (empty(logAnalyticsWorkspaceId)) {
  name: '${workload}-${appEnv}-law'
  location: location
  tags: tags
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
    workspaceCapping: {
      dailyQuotaGb: 1
    }
  }
}

// application insights
resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${workload}-${appEnv}-appi'
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    DisableLocalAuth: disableLocalAuth
    WorkspaceResourceId: empty(logAnalyticsWorkspaceId) ? logAnalyticsWorkspace.id : logAnalyticsWorkspaceId
  }
}

var monitoringMetricsPublisherRoleId = '3913510d-42f4-4e42-8a64-420c390055eb'

resource roleAssignments 'Microsoft.Authorization/roleAssignments@2022-04-01' = [
  for principalId in monitoringMetricsPublishers: {
    name: guid(applicationInsights.id, monitoringMetricsPublisherRoleId, principalId)
    scope: applicationInsights
    properties: {
      principalId: principalId
      roleDefinitionId: resourceId(
        'Microsoft.Authorization/roleDefinitions@2022-04-01',
        monitoringMetricsPublisherRoleId
      )
    }
  }
]

// failure anomalies smart detector
resource failureAnomaliesSmartDetectorRule 'microsoft.alertsManagement/smartDetectorAlertRules@2021-04-01' = {
  name: '${workload}-${appEnv}-fa-sdar'
  location: 'global'
  tags: tags
  properties: {
    state: 'Enabled'
    description: 'Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.'
    severity: 'Sev3'
    frequency: 'PT1M'
    detector: {
      id: 'FailureAnomaliesDetector'
    }
    scope: [
      applicationInsights.id
    ]
    actionGroups: {
      groupIds: [
        actionGroup.id
      ]
    }
  }
}

// action group
resource actionGroup 'Microsoft.Insights/actionGroups@2023-01-01' = {
  name: '${workload}-${appEnv}-ag'
  location: 'global'
  tags: tags
  properties: {
    enabled: true
    groupShortName: actionGroupShortName
    armRoleReceivers: [
      {
        name: 'Email Owner'
        roleId: '8e3af657-a8ff-443c-a75c-2fe8c4bcb635'
        useCommonAlertSchema: true
      }
    ]
  }
}

@description('Application Insights connection string')
output connectionString string = applicationInsights.properties.ConnectionString

@description('Application Insights resource name')
output applicationInsightsName string = applicationInsights.name

@description('Log Analytics workspace resource ID')
output logAnalyticsWorkspaceId string = applicationInsights.properties.WorkspaceResourceId

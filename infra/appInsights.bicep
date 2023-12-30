@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@maxLength(12)
param actionGroupShortName string

var tags = {
  workload: workload
  category: category
}

// log analytics workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${workload}-${category}-law'
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
  name: '${workload}-${category}-appi'
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// action group
resource actionGroup 'Microsoft.Insights/actionGroups@2023-01-01' = {
  name: '${workload}-${category}-ag'
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

// failure anomalies smart detector
resource failureAnomaliesSmartDetectorRule 'microsoft.alertsManagement/smartDetectorAlertRules@2021-04-01' = {
  name: '${workload}-${category}-fa-sdar'
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

output name string = applicationInsights.name
output connectionString string = applicationInsights.properties.ConnectionString

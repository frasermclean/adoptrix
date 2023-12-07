targetScope = 'resourceGroup'

@minLength(3)
@description('Name of the workload')
param workload string

@minLength(3)
@description('Category of the workload')
param category string

@description('Azure region for the non-global resources')
param location string = resourceGroup().location

@description('Name of the shared resource group')
param sharedResourceGroup string = 'adoptrix-shared-rg'

@description('Domain name')
param domainName string

@description('Name of the Azure AD B2C tenant')
param b2cTenantName string = 'adoptrixauth'

@description('Azure AD B2C application client ID')
param b2cAuthClientId string

@description('Azure AD B2C audience')
param b2cAuthAudience string

@description('Azure AD B2C sign-up/sign-in policy ID')
param b2cAuthSignUpSignInPolicyId string = 'B2C_1_Signup_SignIn'

@description('Sqids alphabet')
param appSettingsSqidsAlphabet string

@description('Sqids minimum length')
param appSettingsSqidsMinLength int = 8

@description('First two octets of the virtual network address space')
param vnetAddressPrefix string = '10.250'

@description('Application administrator group name')
param adminGroupName string

@secure()
@description('Application administrator group object ID')
param adminGroupObjectId string

@description('Whether to attempt role assignments (requires appropriate permissions)')
param shouldAttemptRoleAssignments bool = false

var tags = {
  workload: workload
  category: category
}

var deploymentSuffix = startsWith(deployment().name, 'main-')
 ? replace(deployment().name, 'main-', '-')
 : ''

// existing Azure AD B2C tenant
resource b2cTenant 'Microsoft.AzureActiveDirectory/b2cDirectories@2021-04-01' existing = {
  name: '${b2cTenantName}.onmicrosoft.com'
  scope: resourceGroup(sharedResourceGroup)
}

// virtual network
resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  name: '${workload}-${category}-vnet'
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        '${vnetAddressPrefix}.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'apps-subnet'
        properties: {
          addressPrefix: '${vnetAddressPrefix}.1.0/24'
          delegations: [
            {
              name: 'apps-delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          serviceEndpoints: [
            {
              service: 'Microsoft.Sql'
              locations: [ location ]
            }
            {
              service: 'Microsoft.Storage'
              locations: [ location ]
            }
          ]
        }
      }
    ]
  }

  resource appsSubnet 'subnets' existing = {
    name: 'apps-subnet'
  }
}

// azure sql server
resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: '${workload}-${category}-sql'
  location: location
  tags: tags
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: adminGroupName
      sid: adminGroupObjectId
      principalType: 'Group'
      tenantId: subscription().tenantId
    }
    minimalTlsVersion: '1.2'
    version: '12.0'
    publicNetworkAccess: 'Enabled'
  }

  // database
  resource database 'databases' = {
    name: '${workload}-${category}-sqldb'
    location: location
    tags: tags
    sku: {
      name: 'GP_S_Gen5_2'
      tier: 'GeneralPurpose'
      family: 'Gen5'
      capacity: 2
    }
    properties: {
      autoPauseDelay: 60
      collation: 'SQL_Latin1_General_CP1_CI_AS'
      freeLimitExhaustionBehavior: 'AutoPause'
      maxSizeBytes: 34359738368 // 32 GB
      minCapacity: json('0.5')
      useFreeLimit: true
    }
  }

  // virtual network rule
  resource vnetRule 'virtualNetworkRules' = {
    name: 'apps-subnet-rule'
    properties: {
      virtualNetworkSubnetId: vnet::appsSubnet.id
      ignoreMissingVnetServiceEndpoint: false
    }
  }
}

module appInsightsModule 'appInsights.bicep' = {
  name: 'appInsights${deploymentSuffix}'
  params: {
    workload: workload
    category: category
    location: location
    actionGroupShortName: 'AdoptrixDemo'
  }
}

// app service plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: '${workload}-${category}-asp'
  location: location
  tags: tags
  kind: 'linux'
  sku: {
    name: 'B1'
  }
  properties: {
    reserved: true
  }
}

// app service
resource appService 'Microsoft.Web/sites@2022-09-01' = {
  name: '${workload}-${category}-app'
  location: location
  tags: tags
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      http20Enabled: true
      ftpsState: 'FtpsOnly'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsModule.outputs.connectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'recommended'
        }
        {
          name: 'AzureAd__Instance'
          value: 'https://${b2cTenantName}.b2clogin.com'
        }
        {
          name: 'AzureAd__Domain'
          value: '${b2cTenantName}.onmicrosoft.com'
        }
        {
          name: 'AzureAd__TenantId'
          value: b2cTenant.properties.tenantId
        }
        {
          name: 'AzureAd__ClientId'
          value: b2cAuthClientId
        }
        {
          name: 'AzureAd__Audience'
          value: b2cAuthAudience
        }
        {
          name: 'AzureAd__SignUpSignInPolicyId'
          value: b2cAuthSignUpSignInPolicyId
        }
        {
          name: 'AzureStorage__BlobEndpoint'
          value: storageaccount.properties.primaryEndpoints.blob
        }
        {
          name: 'Sqids__Alphabet'
          value: appSettingsSqidsAlphabet
        }
        {
          name: 'Sqids__MinLength'
          value: string(appSettingsSqidsMinLength)
        }
      ]
      connectionStrings: [
        {
          name: 'AdoptrixDb'
          connectionString: 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname};Database=${sqlServer::database.name};Authentication="Active Directory Default";'
          type: 'SQLAzure'
        }
      ]
      cors: {
        allowedOrigins: [
          'https://${staticWebAppModule.outputs.staticWebDefaultHostname}'
        ]
      }
    }
  }

  // virtual network integration
  resource vnetIntegration 'networkConfig' = {
    name: 'virtualNetwork'
    properties: {
      subnetResourceId: vnet::appsSubnet.id
    }
  }

  // custom hostname binding
  resource hostNameBinding 'hostNameBindings' = {
    name: 'api.${category}.${domainName}'
    dependsOn: [ dnsRecords ]
    properties: {
      siteName: appService.name
      customHostNameDnsRecordType: 'CName'
      hostNameType: 'Verified'
      sslState: 'Disabled' // will enable SNI using module
    }
  }
}

// app service certificate
resource appServiceCertificate 'Microsoft.Web/certificates@2022-09-01' = {
  name: '${appService.name}-cert'
  location: location
  tags: tags
  dependsOn: [ appService::hostNameBinding ]
  properties: {
    serverFarmId: appServicePlan.id
    canonicalName: 'api.${category}.${domainName}'
  }
}

// storage account
resource storageaccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${workload}${category}'
  location: location
  tags: tags
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    allowBlobPublicAccess: true
    allowSharedKeyAccess: false
    defaultToOAuthAuthentication: true
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      defaultAction: 'Allow'
    }
  }

  resource blobServices 'blobServices' = {
    name: 'default'

    resource container 'containers' = {
      name: 'animal-images'
      properties: {
        publicAccess: 'Blob'
      }
    }
  }
}

module dnsRecords 'dnsRecords.bicep' = {
  name: 'dnsRecords-${category}${deploymentSuffix}'
  scope: resourceGroup(sharedResourceGroup)
  params: {
    domainName: domainName
    category: category
    appServiceDefaultHostName: appService.properties.defaultHostName
    appServiceDomainVerificationId: appService.properties.customDomainVerificationId
  }
}

module roleAssignmentsModule 'roleAssignments.bicep' = if (shouldAttemptRoleAssignments) {
  name: 'roleAssignments${deploymentSuffix}'
  params: {
    adminGroupObjectId: adminGroupObjectId
    appServiceIdentityPrincipalId: appService.identity.principalId
    storageAccountName: storageaccount.name
  }
}

module staticWebAppModule 'staticWebApp.bicep' = {
  name: 'staticWebApp${deploymentSuffix}'
  params: {
    category: category
    workload: workload
    #disable-next-line no-hardcoded-location // static web apps have limited locations
    location: 'eastasia'
  }
}

module appServiceSniEnableModule 'siteSniEnable.bicep' = {
  name: 'appServiceSniEnable${deploymentSuffix}'
  params: {
    siteName: appService.name
    certificateThumbprint: appServiceCertificate.properties.thumbprint
    hostname: appServiceCertificate.properties.canonicalName
  }
}

output appServiceName string = appService.name

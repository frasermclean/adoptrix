/*
  Bicep template parameters for Adoptrix demo environment
*/

using 'main.bicep'

param workload = 'adoptrix'
param appEnv = 'demo'

param domainName = 'adoptrix.com'
param actionGroupShortName = 'AdoptrixDemo'

// authentication
param authenticationClientId = '05ff30c5-ebb0-49cd-a127-13ace1478c9f' // front end app registration
param authenticationAudience = '1daf5539-1932-4c47-a9a1-a1f52a2db804' // back end app registration

param adminGroupName = 'Adoptrix Demo Administrators'
param adminGroupObjectId = '0356480d-b5dc-440f-a5d3-e7b3f2169c40'

param attemptRoleAssignments = bool(readEnvironmentVariable('ATTEMPT_ROLE_ASSIGNMENTS', 'true'))
param allowedExternalIpAddresses = [
  {
    name: 'hive'
    ipAddress: readEnvironmentVariable('HIVE_IP_ADDRESS', '')
  }
]

// shared resources
param sharedResourceGroup = 'adoptrix-shared-rg'
param appConfigurationName = 'adoptrix-shared-ac'

// container apps
param containerRegistryName = readEnvironmentVariable('ACR_NAME', 'sbocorecr')
param apiImageRepository = readEnvironmentVariable('ADOPTRIX_API_IMAGE_REPOSITORY', 'adoptrix-api')
param apiImageTag = readEnvironmentVariable('ADOPTRIX_API_IMAGE_TAG')

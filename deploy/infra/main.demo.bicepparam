/*
  Bicep template parameters for Adoptrix demo environment
*/

using 'main.bicep'

param workload = 'adoptrix'
param appEnv = 'demo'

param domainName = 'adoptrix.com'
param actionGroupShortName = 'AdoptrixDemo'

// authentication
param azureAdClientId = 'd100dfc4-d993-4e4a-8ebb-a6a55ef72809' // front end app registration
param azureAdAudience = 'a4edca74-3be8-4579-85c6-9a92819b703c' // back end app registration

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
param containerRegistryName = readEnvironmentVariable('CONTAINER_REGISTRY', 'snakebytecorecr')
param apiImageName = 'adoptrix-api'
param apiImageTag = readEnvironmentVariable('API_IMAGE_TAG', 'latest')

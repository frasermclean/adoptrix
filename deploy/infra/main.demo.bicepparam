/*
  Bicep template parameters for Adoptrix demo environment
*/

using 'main.bicep'

param workload = 'adoptrix'
param appEnv = 'demo'

param domainName = 'adoptrix.com'
param actionGroupShortName = 'AdoptrixDemo'

// Azure AD B2C
param b2cTenantName = 'adoptrixauth'
param b2cAuthClientId = 'd100dfc4-d993-4e4a-8ebb-a6a55ef72809' // front end app registration
param b2cAuthAudience = 'a4edca74-3be8-4579-85c6-9a92819b703c' // back end app registration
param b2cAuthSignUpSignInPolicyId = 'B2C_1_Signup_SignIn'

param adminGroupName = 'Adoptrix Demo Administrators'
param adminGroupObjectId = '0356480d-b5dc-440f-a5d3-e7b3f2169c40'

param attemptRoleAssignments = bool(readEnvironmentVariable('ATTEMPT_ROLE_ASSIGNMENTS', 'true'))
param allowedExternalIpAddresses = [
  {
    name: 'hive'
    ipAddress: readEnvironmentVariable('HIVE_IP_ADDRESS')
  }
]

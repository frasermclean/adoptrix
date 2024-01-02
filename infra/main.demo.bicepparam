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
param b2cAuthClientId = '91556c30-ad29-4d36-a3df-29701eaba8a9'
param b2cAuthAudience = '2cb24eb4-3363-42fb-81ca-e69ff34ac201'
param b2cAuthSignUpSignInPolicyId = 'B2C_1_Signup_SignIn'

param adminGroupName = 'Adoptrix Demo Administrators'
param adminGroupObjectId = '0356480d-b5dc-440f-a5d3-e7b3f2169c40'

param allowedExternalIpAddresses = [
  '218.212.202.209'
]

param shouldAttemptRoleAssignments = true

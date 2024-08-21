using 'main.bicep'

param workload = 'adoptrix'
param category = 'shared'
param location = 'australiaeast'
param domainName = 'adoptrix.com'

param containerRegistryPassword = readEnvironmentVariable('ADOPTRIX_CONTAINER_REGISTRY_PASSWORD', '')
param containerRegistryPasswordExpiry = '2025-01-01T00:00:00Z'

param attemptRoleAssignments = bool(readEnvironmentVariable('ATTEMPT_ROLE_ASSIGNMENTS', 'true'))
param deploymentSuffix = readEnvironmentVariable('DEPLOYMENT_SUFFIX', '')

param adminPrincipalIds = [
  'd120ebdd-dad5-4b31-9bb0-2b9cea918b09'
]


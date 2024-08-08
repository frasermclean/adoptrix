using 'main.bicep'

param workload = 'adoptrix'
param category = 'shared'
param location = 'australiaeast'
param domainName = 'adoptrix.com'

param attemptRoleAssignments = bool(readEnvironmentVariable('ATTEMPT_ROLE_ASSIGNMENTS', 'true'))

param configurationDataOwners = [
  'd120ebdd-dad5-4b31-9bb0-2b9cea918b09'
]

param configurationDataReaders = []

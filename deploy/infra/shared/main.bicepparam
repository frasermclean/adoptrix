using 'main.bicep'

param workload = 'adoptrix'
param category = 'shared'
param location = 'southeastasia'
param domainName = 'adoptrix.com'
param b2cTenantPrefix = 'adoptrixauth'

param configurationDataOwners = [
  'd120ebdd-dad5-4b31-9bb0-2b9cea918b09'
]

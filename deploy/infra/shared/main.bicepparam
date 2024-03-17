using 'main.bicep'

param workload = 'adoptrix'
param category = 'shared'
param location = 'southeastasia'
param domainName = 'adoptrix.com'

// authentication
param authenticationInstance = 'https://adoptrix.ciamlogin.com'
param authenticationTenantId = 'adoptrix.com'

param configurationDataOwners = [
  'd120ebdd-dad5-4b31-9bb0-2b9cea918b09'
]

param configurationDataReaders = []

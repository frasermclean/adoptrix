targetScope = 'resourceGroup'

@description('Environment of the application')
param appEnv string

@description('Domain name')
param domainName string

@description('Default hostname of the API app')
param apiAppDefaultHostname string = ''

@description('Default hostname of the client app')
param clientAppDefaultHostname string = ''

@description('Default hostname of the jobs app')
param jobsAppDefaultHostname string = ''

@description('Custom domain verification ID')
param customDomainVerificationId string = ''

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: domainName

  // API app CNAME record
  resource apiAppCnameRecord 'CNAME' = if (!empty(apiAppDefaultHostname)) {
    name: 'api.${appEnv}'
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: apiAppDefaultHostname
      }
    }
  }

  // API app TXT verification record
  resource apiAppTxtRecord 'TXT' = if (!empty(customDomainVerificationId)) {
    name: 'asuid.api.${appEnv}'
    properties: {
      TTL: 3600
      TXTRecords: [
        { value: [customDomainVerificationId] }
      ]
    }
  }

  // client web app CNAME record
  resource cnameRecord 'CNAME' = if (!empty(clientAppDefaultHostname)) {
    name: appEnv
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: clientAppDefaultHostname
      }
    }
  }

  // jobs app CNAME record
  resource jobsAppCnameRecord 'CNAME' = if (!empty(jobsAppDefaultHostname)) {
    name: 'jobs.${appEnv}'
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: jobsAppDefaultHostname
      }
    }
  }

  // jobs app TXT verification record
  resource jobsAppTxtRecord 'TXT' = if (!empty(customDomainVerificationId)) {
    name: 'asuid.jobs.${appEnv}'
    properties: {
      TTL: 3600
      TXTRecords: [
        { value: [customDomainVerificationId] }
      ]
    }
  }
}

@description('Fully qualified domain name of the API application')
output apiAppFqdn string = '${dnsZone::apiAppCnameRecord.name}.${domainName}'

@description('Fully qualified domain name of the jobs application')
output jobsAppFqdn string = '${dnsZone::jobsAppCnameRecord.name}.${domainName}'


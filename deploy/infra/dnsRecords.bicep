targetScope = 'resourceGroup'

@description('Environment of the application')
param appEnv string

@description('Domain name')
param domainName string

param apiDefaultHostname string = ''
param apiCustomDomainVerificationId string = ''

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: domainName

  // API CNAME record
  resource apiCnameRecord 'CNAME' = if (!empty(apiDefaultHostname)) {
    name: 'api.${appEnv}'
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: apiDefaultHostname
      }
    }
  }

  // API TXT verification record
  resource apiTxtRecord 'TXT' = if(!empty(apiCustomDomainVerificationId)) {
    name: 'asuid.api.${appEnv}'
    properties: {
      TTL: 3600
      TXTRecords: [
        { value: [ apiCustomDomainVerificationId ] }
      ]
    }
  }
}

@description('Fully qualified domain name of the API')
output apiFqdn string = '${dnsZone::apiCnameRecord.name}.${domainName}'

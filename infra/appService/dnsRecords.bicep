@description('Environment of the application')
param appEnv string

@description('Domain name')
param domainName string

param appServiceDefaultHostName string
param appServiceDomainVerificationId string

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: domainName

  resource apiCnameRecord 'CNAME' = {
    name: 'api.${appEnv}'
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: appServiceDefaultHostName
      }
    }
  }

  // API TXT verification record
  resource apiTxtRecord 'TXT' = {
    name: 'asuid.api.${appEnv}'
    properties: {
      TTL: 3600
      TXTRecords: [
        { value: [ appServiceDomainVerificationId ] }
      ]
    }
  }
}

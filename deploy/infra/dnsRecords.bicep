targetScope = 'resourceGroup'

@description('Environment of the application')
param appEnv string

@description('Domain name')
param domainName string

param mainAppDefaultHostname string = ''
param mainAppCustomDomainVerificationId string = ''

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: domainName

  // main app CNAME record
  resource mainAppCnameRecord 'CNAME' = if (!empty(mainAppDefaultHostname)) {
    name: appEnv
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: mainAppDefaultHostname
      }
    }
  }

  // main app TXT verification record
  resource mainAppTxtRecord 'TXT' = if(!empty(mainAppCustomDomainVerificationId)) {
    name: 'asuid.${appEnv}'
    properties: {
      TTL: 3600
      TXTRecords: [
        { value: [ mainAppCustomDomainVerificationId ] }
      ]
    }
  }
}

@description('Fully qualified domain name of the main application')
output mainAppFqdn string = '${dnsZone::mainAppCnameRecord.name}.${domainName}'

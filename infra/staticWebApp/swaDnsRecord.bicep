param domainName string
param hostName string

@description('The default hostname for the static web app')
param swaDefaultHostName string

resource dnsZone 'Microsoft.Network/dnsZones@2018-05-01' existing = {
  name: domainName

  resource cnameRecord 'CNAME' = {
    name: hostName
    properties: {
      TTL: 3600
      CNAMERecord: {
        cname: swaDefaultHostName
      }
    }
  }
}

@description('Name of the web app')
param siteName string

@description('Hostname to bind to')
param hostname string

@description('Thumbprint of the certificate to use for SSL')
param certificateThumbprint string

resource site 'Microsoft.Web/sites@2022-09-01' existing = {
  name: siteName

  resource hostNameBinding 'hostNameBindings' = {
    name: hostname
    properties: {
      sslState: 'SniEnabled'
      thumbprint: certificateThumbprint
    }
  }
}

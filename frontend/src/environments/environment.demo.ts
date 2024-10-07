import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: false,
  apiBaseUrl: 'https://api.demo.adoptrix.com/api',
  blobStorageBaseUrl: 'https://adoptrixdemo.blob.core.windows.net/animal-images',
  auth: {
    clientId: '05ff30c5-ebb0-49cd-a127-13ace1478c9f',
    redirectUri: 'https://demo.adoptrix.com',
    appIdUri: 'https://api.demo.adoptrix.com',
  },
  appInsights: {
    connectionString:
      'InstrumentationKey=c1f91173-56d4-4237-a79f-04f5d08e2af5;IngestionEndpoint=https://australiaeast-1.in.applicationinsights.azure.com/;LiveEndpoint=https://australiaeast.livediagnostics.monitor.azure.com/;ApplicationId=b180f887-d8c3-491d-8660-c9f59163789b',
  },
};

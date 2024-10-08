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
      'InstrumentationKey=f31d8a50-134b-44f2-b121-7261052e2f90;IngestionEndpoint=https://australiaeast-1.in.applicationinsights.azure.com/;LiveEndpoint=https://australiaeast.livediagnostics.monitor.azure.com/;ApplicationId=5bb5ae25-c5ae-47f6-99eb-67c131c63a4c',
  },
};

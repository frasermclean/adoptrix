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
      'InstrumentationKey=197c296e-252b-4c50-bf68-e0094b35bd8d;IngestionEndpoint=https://southeastasia-1.in.applicationinsights.azure.com/;LiveEndpoint=https://southeastasia.livediagnostics.monitor.azure.com/;ApplicationId=ed4a05e7-9b22-4748-a9f0-6e919b052aa1',
  },
};

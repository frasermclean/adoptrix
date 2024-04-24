import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: true,
  apiBaseUrl: 'https://localhost:5443/api',
  blobStorageBaseUrl: 'http://localhost:10000/devstoreaccount1/animal-images',
  auth: {
    clientId: '00aa68b2-64bf-40ad-a761-1e979d5987f5',
    redirectUri: 'http://localhost:4200',
    appIdUri: 'api://7e86487e-ac55-4988-8c1e-941d543cb376',
  },
  appInsights: {
    connectionString:
      'InstrumentationKey=19115a76-4a34-4c66-b339-31b90e69de57;IngestionEndpoint=https://southeastasia-1.in.applicationinsights.azure.com/;LiveEndpoint=https://southeastasia.livediagnostics.monitor.azure.com/;ApplicationId=564b7b87-740f-4ee8-8fe2-6e87c38dfa3a',
  },
};

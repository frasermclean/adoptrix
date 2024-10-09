import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: true,
  apiBaseUrl: 'https://localhost:5001/api',
  auth: {
    clientId: '00aa68b2-64bf-40ad-a761-1e979d5987f5',
    redirectUri: 'http://localhost:4200',
    appIdUri: 'api://7e86487e-ac55-4988-8c1e-941d543cb376',
  },
  appInsights: {
    connectionString:
      'InstrumentationKey=41d73ec0-49ed-47ff-a7da-16d6446c1b90;IngestionEndpoint=https://southeastasia-1.in.applicationinsights.azure.com/;LiveEndpoint=https://southeastasia.livediagnostics.monitor.azure.com/;ApplicationId=3062368b-d58a-4e7b-9716-b68eeb73faba',
  },
};

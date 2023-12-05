import { Environment } from "./environment.interface";

export const environment: Environment = {
  isDevelopment: true,
  apiBaseUrl: '/api',
  auth: {
    clientId: 'd100dfc4-d993-4e4a-8ebb-a6a55ef72809',
    redirectUri: 'https://demo.adoptrix.com',
    scopes: ['https://adoptrixauth.onmicrosoft.com/demo-api/access'],
  },
};

import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: false,
  apiBaseUrl: 'https://api.demo.adoptrix.com/api',
  blobStorageBaseUrl: 'https://adoptrixdemo.blob.core.windows.net/animal-images',
  auth: {
    clientId: 'd100dfc4-d993-4e4a-8ebb-a6a55ef72809',
    redirectUri: 'https://demo.adoptrix.com',
    scopes: ['https://adoptrixauth.onmicrosoft.com/demo-api/access'],
  },
};

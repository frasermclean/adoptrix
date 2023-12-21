import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: true,
  apiBaseUrl: 'https://localhost:5443/api',
  blobStorageBaseUrl: 'http://localhost:10000/devstoreaccount1/animal-images',
  auth: {
    clientId: '91556c30-ad29-4d36-a3df-29701eaba8a9',
    redirectUri: 'http://localhost:4200',
    scopes: ['https://adoptrixauth.onmicrosoft.com/dev-api/access'],
  },
};

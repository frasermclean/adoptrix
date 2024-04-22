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
};

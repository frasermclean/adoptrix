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
};

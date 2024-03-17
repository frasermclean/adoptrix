import { Environment } from './environment.interface';

export const environment: Environment = {
  isDevelopment: false,
  apiBaseUrl: 'https://api.demo.adoptrix.com/api',
  blobStorageBaseUrl: 'https://adoptrixdemo.blob.core.windows.net/animal-images',
  auth: {
    clientId: '05ff30c5-ebb0-49cd-a127-13ace1478c9f',
    redirectUri: 'https://demo.adoptrix.com',
    scopes: ['api://1daf5539-1932-4c47-a9a1-a1f52a2db804/access'],
  },
};

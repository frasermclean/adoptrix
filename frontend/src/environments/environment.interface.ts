/**
 * Environment interface -  provides type safety for environment variables.
 */
export interface Environment {
  isDevelopment: boolean;
  apiBaseUrl: string;
  auth: {
    clientId: string;
    redirectUri: string;
    appIdUri: string;
  };
  appInsights: {
    connectionString: string;
  };
}

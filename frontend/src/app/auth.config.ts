import {
  BrowserCacheLocation,
  IPublicClientApplication,
  InteractionType,
  LogLevel,
  PublicClientApplication,
} from '@azure/msal-browser';
import { MsalGuardConfiguration, MsalInterceptorConfiguration } from '@azure/msal-angular';

import { environment } from '../environments/environment';

const tenantName = 'adoptrixauth';
const b2cPolicyName = 'B2C_1_Signup_SignIn';

/**
 * MSAL Instance Factory
 */
export function msalInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.auth.clientId,
      authority: `https://${tenantName}.b2clogin.com/${tenantName}.onmicrosoft.com/${b2cPolicyName}`,
      knownAuthorities: [`https://${tenantName}.b2clogin.com`],
      redirectUri: environment.auth.redirectUri,
      postLogoutRedirectUri: '/',
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
    },
    system: {
      allowNativeBroker: false, // Disables WAM Broker
      loggerOptions: {
        loggerCallback,
        logLevel: LogLevel.Info,
        piiLoggingEnabled: false,
      },
    },
  });
}

/**
 * MSAL guard configuration factory
 */
export function msalGuardConfigurationFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [...environment.auth.scopes]
    },
    loginFailedRoute: 'login-failed' // TODO: Implement login failed route
  }
}

/**
 * MSAL interceptor configuration factory
 */
export function msalInterceptorConfigurationFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(`${environment.apiBaseUrl}/admin`, environment.auth.scopes);

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

function loggerCallback(logLevel: LogLevel, message: string): void {
  switch (logLevel) {
    case LogLevel.Error:
      return console.error(message);
    case LogLevel.Warning:
      return console.warn(message);
    case LogLevel.Info:
      return console.info(message);
    case LogLevel.Verbose:
      return console.debug(message);
    default:
      return console.log(message);
  }
}

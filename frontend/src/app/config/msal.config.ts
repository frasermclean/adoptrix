import { Provider } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import {
  BrowserCacheLocation,
  IPublicClientApplication,
  InteractionType,
  LogLevel,
  PublicClientApplication,
} from '@azure/msal-browser';
import {
  MSAL_GUARD_CONFIG,
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService,
  MsalGuard,
  MsalGuardConfiguration,
  MsalInterceptor,
  MsalInterceptorConfiguration,
  MsalService,
  ProtectedResourceScopes,
} from '@azure/msal-angular';

import { environment } from '../../environments/environment';

export function provideMsal(): Provider[] {
  return [
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: instanceFactory,
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: guardConfigurationFactory,
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: interceptorConfigurationFactory,
    },
  ];
}

const instance = 'adoptrix.ciamlogin.com';
const tenantId = 'adoptrix.com';

// scopes
export const apiAccessScope = `${environment.auth.appIdUri}/access`;

/**
 * MSAL Instance Factory
 */
function instanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.auth.clientId,
      authority: `https://${instance}/${tenantId}`,
      knownAuthorities: [`https://${instance}`],
      redirectUri: environment.auth.redirectUri,
      postLogoutRedirectUri: '/',
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,
    },
    system: {
      allowNativeBroker: false, // Disables WAM Broker
      loggerOptions: {
        logLevel: LogLevel.Info,
        piiLoggingEnabled: false,
        loggerCallback: (logLevel, message) => {
          if (logLevel <= LogLevel.Warning) {
            console.error(message);
          } else {
            console.log(message);
          }
        },
      },
    },
  });
}

/**
 * MSAL guard configuration factory
 */
function guardConfigurationFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [apiAccessScope],
    },
    loginFailedRoute: 'login-failed', // TODO: Implement login failed route
  };
}

/**
 * MSAL interceptor configuration factory
 */
function interceptorConfigurationFactory(): MsalInterceptorConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap: new Map<string, Array<string | ProtectedResourceScopes> | null>([
      ['https://graph.microsoft.com/v1.0/me', ['user.read', 'profile']],
      [
        `${environment.apiBaseUrl}/*`,
        [
          {
            httpMethod: 'POST',
            scopes: [apiAccessScope],
          },
          {
            httpMethod: 'PUT',
            scopes: [apiAccessScope],
          },
          {
            httpMethod: 'DELETE',
            scopes: [apiAccessScope],
          },
        ],
      ],
    ]),
  };
}

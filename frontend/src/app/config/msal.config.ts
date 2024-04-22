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
const accessScope = `${environment.auth.appIdUri}/access`;

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
function guardConfigurationFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [accessScope],
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
            scopes: [accessScope],
          },
          {
            httpMethod: 'PUT',
            scopes: [accessScope],
          },
          {
            httpMethod: 'DELETE',
            scopes: [accessScope],
          },
        ],
      ],
    ]),
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

import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter, withComponentInputBinding, withEnabledBlockingInitialNavigation } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

// MSAL dependencies
import {
  MSAL_GUARD_CONFIG,
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService,
  MsalGuard,
  MsalInterceptor,
  MsalService,
} from '@azure/msal-angular';

// NGXS modules
import { NgxsModule } from '@ngxs/store';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';

import { routes } from './app.routes';
import { environment } from '../environments/environment';
import {
  msalGuardConfigurationFactory,
  msalInstanceFactory,
  msalInterceptorConfigurationFactory,
} from './auth/auth.config';
import { AuthState } from './auth/auth.state';
import { GALLERY_CONFIG } from 'ng-gallery';
import { galleryConfigFactory } from '@shared/gallery.config';
import { MAT_TOOLTIP_DEFAULT_OPTIONS } from '@angular/material/tooltip';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withEnabledBlockingInitialNavigation()),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    importProvidersFrom([
      NgxsModule.forRoot([AuthState], { developmentMode: environment.isDevelopment }),
      NgxsLoggerPluginModule.forRoot({ disabled: !environment.isDevelopment }),
    ]),
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
      useFactory: msalInstanceFactory,
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: msalGuardConfigurationFactory,
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: msalInterceptorConfigurationFactory,
    },
    {
      provide: GALLERY_CONFIG,
      useFactory: galleryConfigFactory,
    },
    {
      provide: MAT_TOOLTIP_DEFAULT_OPTIONS,
      useValue: {
        showDelay: 500,
        hideDelay: 100,
        touchendHideDelay: 1000,
      },
    },
  ],
};

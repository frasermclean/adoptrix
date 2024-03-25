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
import {
  msalGuardConfigurationFactory,
  msalInstanceFactory,
  msalInterceptorConfigurationFactory,
} from './auth/auth.config';

// NGXS modules
import { NgxsModule } from '@ngxs/store';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { AuthState } from './auth/auth.state';
import { ngxsConfig } from './ngxs.config';

// gallery
import { GALLERY_CONFIG } from 'ng-gallery';
import { galleryConfigFactory } from '@shared/gallery.config';

import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { provideMaterialConfig } from '@config/material.config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withEnabledBlockingInitialNavigation()),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    provideMaterialConfig(),
    importProvidersFrom([
      NgxsModule.forRoot([AuthState], ngxsConfig),
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
  ],
};

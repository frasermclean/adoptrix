import { ApplicationConfig, ErrorHandler, importProvidersFrom } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter, withComponentInputBinding, withEnabledBlockingInitialNavigation } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import {
  ApplicationinsightsAngularpluginErrorService,
  ApplicationinsightsAngularpluginJsModule,
} from '@microsoft/applicationinsights-angularplugin-js';

// app configuration providers
import { provideMaterialConfig } from '@config/material.config';
import { provideMsal } from '@config/msal.config';
import { provideNgxsModules } from '@config/ngxs.config';
import { provideGallery } from '@config/gallery.config';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withEnabledBlockingInitialNavigation()),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    provideMaterialConfig(),
    provideMsal(),
    provideNgxsModules(),
    provideGallery(),
    importProvidersFrom(ApplicationinsightsAngularpluginJsModule),
    {
      provide: ErrorHandler,
      useClass: ApplicationinsightsAngularpluginErrorService,
    },
  ],
};

import { ApplicationConfig } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter, withComponentInputBinding, withEnabledBlockingInitialNavigation } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

// app configuration providers
import { provideMaterialConfig } from '@config/material.config';
import { provideMsal } from '@config/msal.config';
import { provideNgxsModules } from '@config/ngxs.config';

// gallery
import { GALLERY_CONFIG } from 'ng-gallery';
import { galleryConfigFactory } from '@shared/gallery.config';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding(), withEnabledBlockingInitialNavigation()),
    provideAnimations(),
    provideHttpClient(withInterceptorsFromDi()),
    provideMaterialConfig(),
    provideMsal(),
    provideNgxsModules(),
    {
      provide: GALLERY_CONFIG,
      useFactory: galleryConfigFactory,
    },
  ],
};

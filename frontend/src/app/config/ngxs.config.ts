import { NgxsModule, NgxsModuleOptions } from '@ngxs/store';
import { environment } from '../../environments/environment';
import { EnvironmentProviders, importProvidersFrom } from '@angular/core';
import { NgxsLoggerPluginModule, NgxsLoggerPluginOptions } from '@ngxs/logger-plugin';
import { NgxsRouterPluginModule } from '@ngxs/router-plugin';
import { AuthState } from '../auth/auth.state';

export function provideNgxsModules(): EnvironmentProviders {
  return importProvidersFrom([
    NgxsModule.forRoot([AuthState], rootConfig),
    NgxsLoggerPluginModule.forRoot(loggerPluginConfig),
    NgxsRouterPluginModule.forRoot(),
  ]);
}

const rootConfig: NgxsModuleOptions = {
  developmentMode: !environment.isDevelopment,
};

const loggerPluginConfig: NgxsLoggerPluginOptions = {
  disabled: !environment.isDevelopment,
};

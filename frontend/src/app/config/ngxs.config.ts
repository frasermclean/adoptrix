import { provideStore, withNgxsDevelopmentOptions } from '@ngxs/store';
import { withNgxsLoggerPlugin } from '@ngxs/logger-plugin';
import { environment } from '../../environments/environment';
import { AuthState } from '@state/auth.state';
import { withNgxsRouterPlugin } from '@ngxs/router-plugin';

export function provideNgxs() {
  return [
    provideStore(
      [AuthState],
      {
        developmentMode: !environment.isDevelopment,
      },
      withNgxsDevelopmentOptions({
        warnOnUnhandledActions: true,
      }),
      withNgxsRouterPlugin(),
      withNgxsLoggerPlugin({
        disabled: !environment.isDevelopment,
      }),
    ),
  ];
}

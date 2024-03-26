import { Provider } from '@angular/core';
import { MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS, MatSnackBarConfig } from '@angular/material/snack-bar';
import { MAT_TOOLTIP_DEFAULT_OPTIONS, MatTooltipDefaultOptions } from '@angular/material/tooltip';

export function provideMaterialConfig(): Provider[] {
  return [
    provideNativeDateAdapter(),
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'en-GB', // TODO: Look into using the user's locale
    },
    {
      provide: MAT_TOOLTIP_DEFAULT_OPTIONS,
      useFactory: tooltipDefaultOptionsFactory,
    },
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useFactory: snackBarDefaultOptionsFactory,
    },
  ];
}

function tooltipDefaultOptionsFactory(): MatTooltipDefaultOptions {
  return {
    showDelay: 500,
    hideDelay: 100,
    touchendHideDelay: 100,
  };
}

function snackBarDefaultOptionsFactory(): MatSnackBarConfig {
  return {
    duration: 2500,
    horizontalPosition: 'center',
    verticalPosition: 'bottom',
  };
}

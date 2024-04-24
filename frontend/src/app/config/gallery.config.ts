import { Provider } from '@angular/core';
import { GALLERY_CONFIG } from 'ng-gallery';

export function provideGallery(): Provider[] {
  return [
    {
      provide: GALLERY_CONFIG,
      useValue: {
        itemAutosize: true,
      },
    },
  ];
}

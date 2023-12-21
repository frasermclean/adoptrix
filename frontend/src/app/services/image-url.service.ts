import { Injectable } from '@angular/core';
import { ImageInformation } from '@models/image-information.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ImageUrlService {
  constructor() {}

  public getFullSizeUrl(animalId: string, image: ImageInformation): string {
    return this.getImageUrl(animalId, image, 'full');
  }

  public getPreviewUrl(animalId: string, image: ImageInformation): string {
    return this.getImageUrl(animalId, image, 'preview');
  }

  public getThumbnailUrl(animalId: string, image: ImageInformation): string {
    return this.getImageUrl(animalId, image, 'thumb');
  }

  private getImageUrl(animalId: string, image: ImageInformation, category: 'full' | 'thumb' | 'preview'): string {
    let suffix = 'original';

    if (category === 'full' && image.hasFullSize) {
      suffix = 'full';
    } else if (category === 'preview' && image.hasPreview) {
      suffix = 'preview';
    } else if (category === 'thumb' && image.hasThumbnail) {
      suffix = 'thumb';
    }

    return `${environment.blobStorageBaseUrl}/${animalId}/${image.id}-${suffix}.jpg`;
  }
}

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
    const suffix = (image.isProcessed && category) || 'original';
    return `${environment.blobStorageBaseUrl}/${animalId}/${image.id}/${suffix}`;
  }
}

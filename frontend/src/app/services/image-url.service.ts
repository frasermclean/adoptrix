import { Injectable } from '@angular/core';
import { ImageResponse } from '@models/image.response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ImageUrlService {
  constructor() {}

  public getFullSizeUrl(animalId: string, image: ImageResponse): string {
    return this.getImageUrl(animalId, image, 'full');
  }

  public getPreviewUrl(animalId: string, image: ImageResponse): string {
    return this.getImageUrl(animalId, image, 'preview');
  }

  public getThumbnailUrl(animalId: string, image: ImageResponse): string {
    return this.getImageUrl(animalId, image, 'thumb');
  }

  /**
   * Get the default image URL for a species
   * @param speciesName The species name to lookup
   * @returns The URL of the default image for the species
   */
  public getSpeciesDefaultImageUrl(speciesName: string): string {
    return `images/${speciesName.toLowerCase()}.png`
  }

  private getImageUrl(animalId: string, image: ImageResponse, category: 'full' | 'thumb' | 'preview'): string {
    const suffix = (image.isProcessed && category) || 'original';
    return `${environment.blobStorageBaseUrl}/${animalId}/${image.id}/${suffix}`;
  }
}

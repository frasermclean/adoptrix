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

  /**
   * Get the default image URL for a species
   * @param speciesId The species ID to lookup
   * @returns The URL of the default image for the species
   */
  public getSpeciesDefaultImageUrl(speciesId: string): string {
    switch (speciesId) {
      case 'c9c1836b-1051-45c3-a2c4-0d841e69e6d3':
        return 'images/cat.png';
      case 'e6d11a53-bacb-4a8b-a171-beea7e935467':
        return 'images/horse.png';
      default:
        return 'images/dog.png';
    }
  }

  private getImageUrl(animalId: string, image: ImageInformation, category: 'full' | 'thumb' | 'preview'): string {
    const suffix = (image.isProcessed && category) || 'original';
    return `${environment.blobStorageBaseUrl}/${animalId}/${image.id}/${suffix}`;
  }
}

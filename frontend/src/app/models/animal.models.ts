import { Sex } from './sex.enum';

export interface Animal {
  id: string;
  name: string;
  description: string | null;
  speciesName: string;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
  lastModifiedUtc: string;
  images: AnimalImage[];
}

export interface AnimalImage {
  id: number;
  description?: string;
  isProcessed: boolean;
  previewUrl: string | null;
  thumbnailUrl: string | null;
  fullSizeUrl: string | null;
}

export interface SearchAnimalsRequest {
  speciesName: string;
  breedName: string;
  sex: Sex;
}

export interface SearchAnimalsItem {
  id: string;
  slug: string;
  name: string;
  speciesName: string;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
  previewImageUrl: string | null;
}

export interface SetAnimalRequest {
  name: string;
  description: string | null;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
}

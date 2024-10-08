import { ImageResponse } from './image.response';
import { Sex } from './sex.enum';

export interface Animal {
  id: string;
  name: string;
  description: string | null;
  speciesId: string;
  speciesName: string;
  breedId: string;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
  images: ImageResponse[];
}

export interface SearchAnimalsQuery {
  name: string;
  breedId: string;
  speciesId: string;
  sex: Sex;
  limit: number;
}

export interface SearchAnimalsResult {
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
  speciesId: string;
  breedId: string;
  sex: Sex;
  dateOfBirth: string;
}

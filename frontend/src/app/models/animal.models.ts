import { ImageResponse } from './image.response';
import { Sex } from './sex.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  speciesName: string;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
  images: ImageResponse[];
}

export interface SearchAnimalsParams {
  speciesId?: string;
  name?: string;
}

export interface SearchAnimalsResult {
  id: string;
  name: string;
  speciesName: string;
  breedName: string;
  sex: Sex;
  dateOfBirth: string;
  image?: ImageResponse;
}

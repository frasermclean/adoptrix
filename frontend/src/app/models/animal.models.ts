import { Breed } from './breed.models';
import { ImageResponse } from './image.response';
import { Sex } from './sex.enum';
import { Species } from './species.model';

export interface Animal {
  id: string;
  name: string;
  description: string | null;
  species: Species;
  breed: Breed;
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

export interface SetAnimalRequest {
  name: string;
  description: string | null;
  speciesId: string;
  breedId: string;
  sex: Sex;
  dateOfBirth: string;
}

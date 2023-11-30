import { Species } from './species.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  species: Species;
  breed?: string;
  dateOfBirth: string;
  images: AnimalImage[];
}

export interface AnimalImage {
  id: number;
  uri: string;
  description?: string;
}

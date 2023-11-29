import { Species } from './species.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  species: Species;
  dateOfBirth: Date;
  images: AnimalImage[];
}

interface AnimalImage {
  id: number;
  uri: string;
  description?: string;
}

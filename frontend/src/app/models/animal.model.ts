import { ImageInformation } from './image-information.model';
import { Species } from './species.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  speciesId: Species;
  breedId: string;
  sex: Sex;
  dateOfBirth: string;
  images: ImageInformation[];
}

export enum Sex {
  Male = 'male',
  Female = 'female',
}

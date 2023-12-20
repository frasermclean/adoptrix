import { ImageInformation } from './image-information.model';
import { Species } from './species.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  species: Species;
  breed?: string;
  dateOfBirth: string;
  images: ImageInformation[];
}

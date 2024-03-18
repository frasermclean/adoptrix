import { ImageInformation } from './image-information.model';
import { Sex } from './sex.enum';

export interface Animal {
  id: string;
  name: string;
  description?: string;
  speciesId: string;
  breedId: string;
  sex: Sex;
  dateOfBirth: string;
  images: ImageInformation[];
}

export interface Breed {
  id: string;
  name: string;
  speciesName: string;
  animalCount: number;
}

export interface SearchBreedsRequest {
  speciesId?: string;
  withAnimals?: boolean;
}

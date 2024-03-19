export interface Breed {
  id: string;
  name: string;
  speciesId: string;
  animalIds: string[];
}

export interface SearchBreedsRequest {
  speciesId?: string;
  withAnimals?: boolean;
}

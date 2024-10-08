export interface Breed {
  id: string;
  name: string;
  speciesName: string;
  animalCount: number;
}

export interface SearchBreedsRequest {
  speciesName: string;
  withAnimals: boolean;
}

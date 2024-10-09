export interface SearchSpeciesRequest {
  withAnimals: boolean;
}

export interface Species {
  id: string;
  name: string;
  breedCount: number;
  animalCount: number;
}

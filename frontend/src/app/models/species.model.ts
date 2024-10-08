export interface SearchSpeciesQuery {
  withAnimals: boolean;
}

export interface SpeciesResponse {
  id: string;
  name: string;
  breedCount: number;
  animalCount: number;
}

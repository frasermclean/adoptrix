export interface Species {
  id: string;
  name: string;
}

export interface SearchSpeciesQuery {
  withAnimals: boolean;
}

export interface SearchSpeciesMatch {
  speciesId: string;
  speciesName: string;
  breedCount: number;
  animalCount: number;
}

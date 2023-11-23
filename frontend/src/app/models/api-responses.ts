export interface AnimalSearchResult {
  id: string;
  name: string;
  description?: string;
  species: Species;
}

export enum Species {
  Dog = 'dog',
  Cat = 'cat',
  Horse = 'horse',
}

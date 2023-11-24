export interface AnimalSearchResult {
  id: string;
  name: string;
  description?: string;
  imageUrl?: string;
  species: Species;
}

export enum Species {
  Dog = 'dog',
  Cat = 'cat',
  Horse = 'horse',
}

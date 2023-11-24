export interface Animal {
  id: string;
  name: string;
  description?: string;
  species: Species;
}

export interface AnimalSearchResult {
  id: string;
  name: string;
  description?: string;
  species: Species;
  mainImage?: AnimalImageResult;
  imageCount: number;
}

export interface AnimalImageResult {
  uri: string;
  description?: string;
}

export enum Species {
  Dog = 'dog',
  Cat = 'cat',
  Horse = 'horse',
}

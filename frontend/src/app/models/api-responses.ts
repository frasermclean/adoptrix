export interface AnimalResult {
  id: string;
  name: string;
  description?: string;
  species: Species;
  dateOfBirth: string;
}

export interface AnimalSearchResult {
  id: string;
  name: string;
  description?: string;
  species: Species;
  mainImage?: AnimalImageResult;
  imageCount: number;
}

interface AnimalImageResult {
  uri: string;
  description?: string;
}

export enum Species {
  Dog = 'dog',
  Cat = 'cat',
  Horse = 'horse',
}

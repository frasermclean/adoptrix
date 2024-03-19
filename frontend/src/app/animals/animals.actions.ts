import { SearchAnimalsParams } from '@models/animal.models';

export class SearchAnimals {
  static readonly type = '[Animals List Component] Search Animals';
  constructor(public params?: SearchAnimalsParams) {}
}

export class GetAnimal {
  static readonly type = '[Animal View Component] Get Animal';
  constructor(public animalId: string) {}
}

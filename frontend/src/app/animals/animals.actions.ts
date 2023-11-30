import { SearchAnimalsParams } from '@services/animals.service';

export class SearchAnimals {
  static readonly type = '[Animals List Component] Search Animals';
  constructor(public params?: SearchAnimalsParams) {}
}

export class GetAnimal {
  static readonly type = '[Animal View Component] Get Animal';
  constructor(public id: string) {}
}

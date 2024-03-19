import { SearchAnimalsParams } from '@models/animal.models';

export namespace AnimalsActions {
  export class Search {
    static readonly type = '[Animals List Component] Search Animals';
    constructor(public params?: SearchAnimalsParams) {}
  }

  export class Get {
    static readonly type = '[Animal View Component] Get Animal';
    constructor(public animalId: string) {}
  }
}

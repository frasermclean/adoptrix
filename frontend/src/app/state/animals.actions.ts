import { SearchAnimalsParams, SetAnimalRequest } from '@models/animal.models';

export namespace AnimalsActions {
  export class Search {
    static readonly type = '[Animals List Component] Search Animals';
    constructor(public params?: SearchAnimalsParams) {}
  }

  export class Get {
    static readonly type = '[Animal View Component] Get Animal';
    constructor(public animalId: string) {}
  }

  export class Add {
    static readonly type = '[Animal Edit Component] Add Animal';
    constructor(public request: SetAnimalRequest) {}
  }

  export class Delete {
    static readonly type = '[Animal Admin Controls Component] Delete Animal';
    constructor(public animalId: string) {}
  }
}

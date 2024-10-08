import { SearchAnimalsRequest, SetAnimalRequest } from '@models/animal.models';

export namespace AnimalsActions {
  export class Search {
    static readonly type = '[Animals List Component] Search Animals';
    constructor(public request: Partial<SearchAnimalsRequest>) {}
  }

  export class Get {
    static readonly type = '[Animal View Component] Get Animal';
    constructor(public animalId: string) {}
  }

  export class Add {
    static readonly type = '[Animal List Component] Add Animal';
    constructor(public request: SetAnimalRequest) {}
  }

  export class Update {
    static readonly type = '[Animal Admin Controls Component] Update Animal';
    constructor(public animalId: string, public request: SetAnimalRequest) {}
  }

  export class Delete {
    static readonly type = '[Animal Admin Controls Component] Delete Animal';
    constructor(public animalId: string) {}
  }
}

import { SearchSpeciesRequest } from '@models/species.model';

export namespace SpeciesActions {
  export class Search {
    static readonly type = 'Search Species';
    constructor(public query: Partial<SearchSpeciesRequest> = {}) {}
  }

  export class GetAll {
    static readonly type = 'Get All Species';
  }

  export class Get {
    static readonly type = 'Get Species by Id';
    constructor(public speciesId: string) {}
  }
}

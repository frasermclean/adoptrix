import { SearchBreedsRequest } from '@models/breed.models';

export namespace BreedsActions {
  export class Search {
    static readonly type = 'SearchBreeds';
    constructor(public request: SearchBreedsRequest) {}
  }
}

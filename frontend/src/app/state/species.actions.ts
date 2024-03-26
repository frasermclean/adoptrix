export namespace SpeciesActions {
  export class GetAll {
    static readonly type = 'Get All Species';
  }

  export class GetSpecies {
    static readonly type = 'Get Species by Id';
    constructor(public speciesId: string) {}
  }
}

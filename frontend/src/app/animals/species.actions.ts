export class SearchSpecies {
  static readonly type = '[xxx] Search Species';
}

export class GetSpecies {
  static readonly type = '[xxx] Get Species';
  constructor(public speciesId: string) {}
}

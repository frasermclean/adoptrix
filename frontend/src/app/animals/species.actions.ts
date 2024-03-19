export class GetAllSpecies {
  static readonly type = '[xxx] Get All Species';
}

export class GetSpecies {
  static readonly type = '[xxx] Get Species by Id';
  constructor(public speciesId: string) {}
}

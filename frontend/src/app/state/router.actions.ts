import { Router } from '@angular/router';

export namespace RouterActions {
  export class Navigate {
    static readonly type = 'Router Navigate';
    constructor(public payload: string[]) {}
  }
}

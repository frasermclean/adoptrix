export namespace AuthActions {
  export class Processing {
    static readonly type = '[Auth Service] Processing';
  }

  export class Completed {
    static readonly type = '[Auth Service] Completed';
    constructor(public data: { isLoggedIn: boolean; userId?: string; name?: string; email?: string }) {}
  }

  export class Login {
    static readonly type = '[Auth Component] Login';
  }

  export class Logout {
    static readonly type = '[Auth Component] Logout';
  }

  export class GetTokenClaims {
    static readonly type = '[Auth Service] Get Token Claims';
  }

  export class TokenClaimsRetrieved {
    static readonly type = '[Auth Service] Token Claims Fetched';
    constructor(public groups: string[]) {}
  }
}

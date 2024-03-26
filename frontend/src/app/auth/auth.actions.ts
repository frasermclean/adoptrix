import { AccountInfo } from '@azure/msal-browser';

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

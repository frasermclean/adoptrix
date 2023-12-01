import { AccountInfo } from '@azure/msal-browser';

export class Processing {
  static readonly type = '[Auth Service] Processing';
}

export class Completed {
  static readonly type = '[Auth Service] Completed';
  constructor(public accountInfo: AccountInfo | null) {}
}

export class Login {
  static readonly type = '[Auth Component] Login';
}

export class Logout {
  static readonly type = '[Auth Component] Logout';
}

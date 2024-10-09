import { Inject, Injectable } from '@angular/core';
import { MsalService, MsalBroadcastService, MSAL_GUARD_CONFIG, MsalGuardConfiguration } from '@azure/msal-angular';
import { AccountInfo, InteractionStatus, RedirectRequest } from '@azure/msal-browser';
import { Store } from '@ngxs/store';
import { AuthActions } from '@state/auth.actions';
import { apiAccessScope } from '@config/msal.config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private store: Store
  ) {
    this.msalService.handleRedirectObservable().subscribe();

    this.msalBroadcastService.inProgress$.subscribe((status) => {
      switch (status) {
        case InteractionStatus.HandleRedirect:
          this.store.dispatch(new AuthActions.Processing());
          break;
        case InteractionStatus.None:
          const activeAccount = this.getActiveAccount();
          this.store.dispatch(
            new AuthActions.Completed({
              isLoggedIn: activeAccount !== null,
              userId: activeAccount?.localAccountId,
              name: activeAccount?.name,
              email: activeAccount?.username,
            })
          );
          break;
      }
    });
  }

  public login(): void {
    if (this.msalGuardConfig.authRequest) {
      this.msalService.loginRedirect({ ...this.msalGuardConfig.authRequest } as RedirectRequest);
    } else {
      this.msalService.loginRedirect();
    }
  }

  public logout(): void {
    this.msalService.logoutRedirect();
  }

  public async getTokenClaims() {
    const activeAccount = this.getActiveAccount();
    if (!activeAccount) {
      throw new Error('No active account found');
    }

    const tokenResult = await this.msalService.instance.acquireTokenSilent({
      scopes: [apiAccessScope],
    });
    const groups = tokenResult.account.idTokenClaims?.['groups'] as string[];

    this.store.dispatch(new AuthActions.TokenClaimsRetrieved(groups));
  }

  private getActiveAccount(): AccountInfo | null {
    let activeAccount = this.msalService.instance.getActiveAccount();

    if (!activeAccount && this.msalService.instance.getAllAccounts().length > 0) {
      const accounts = this.msalService.instance.getAllAccounts();
      activeAccount = accounts[0];
      this.msalService.instance.setActiveAccount(activeAccount);
    }

    return activeAccount;
  }
}

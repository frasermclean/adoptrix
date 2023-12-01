import { Injectable } from '@angular/core';
import { MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { AccountInfo, InteractionStatus } from '@azure/msal-browser';
import { Store } from '@ngxs/store';
import { Processing, Completed } from '../auth/auth.actions';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private msalService: MsalService,
    private msalBroadcastService: MsalBroadcastService,
    private store: Store
  ) {
    this.msalService.handleRedirectObservable().subscribe();

    this.msalBroadcastService.inProgress$.subscribe((status) => {
      switch (status) {
        case InteractionStatus.HandleRedirect:
          this.store.dispatch(new Processing());
          break;
        case InteractionStatus.None:
          const activeAccount = this.getActiveAccount();
          this.store.dispatch(new Completed(activeAccount));
          break;
      }
    });
  }

  public login(): void {
    this.msalService.loginRedirect();
  }

  public logout(): void {
    this.msalService.logoutRedirect();
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

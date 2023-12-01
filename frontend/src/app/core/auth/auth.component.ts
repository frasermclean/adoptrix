import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { InteractionStatus } from '@azure/msal-browser';
import { Subject, filter, takeUntil } from 'rxjs';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss',
})
export class AuthComponent implements OnInit, OnDestroy {
  private readonly destroying$ = new Subject<void>();
  public status: 'initial' | 'logged-out' | 'logged-in' = 'initial';

  constructor(private msalService: MsalService, private msalBroadcastService: MsalBroadcastService) {}

  ngOnInit(): void {
    this.msalService.handleRedirectObservable().subscribe();

    this.msalBroadcastService.inProgress$
      .pipe(
        filter((status: InteractionStatus) => status === InteractionStatus.None),
        takeUntil(this.destroying$)
      )
      .subscribe(() => {
        this.updateStatus();
      });
  }

  updateStatus() {
    let activeAccount = this.msalService.instance.getActiveAccount();

    if (!activeAccount && this.msalService.instance.getAllAccounts().length > 0) {
      const accounts = this.msalService.instance.getAllAccounts();
      activeAccount = accounts[0];
      this.msalService.instance.setActiveAccount(activeAccount);
    }

    if (activeAccount) {
      this.status = 'logged-in';
    } else {
      this.status = 'logged-out';
    }
  }

  onLogin(): void {
    this.msalService.loginRedirect();
  }

  onLogout(): void {
    this.msalService.logoutRedirect();
  }

  ngOnDestroy(): void {
    this.destroying$.next();
    this.destroying$.complete();
  }
}

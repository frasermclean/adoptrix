import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Store } from '@ngxs/store';
import { AuthState } from '../../auth/auth.state';
import { Login, Logout } from '../../auth/auth.actions';

@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatProgressSpinnerModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss',
})
export class UserComponent {
  status$ = this.store.select(AuthState.status);

  constructor(private store: Store) {}

  onLogin(): void {
    this.store.dispatch(new Login());
  }

  onLogout(): void {
    this.store.dispatch(new Logout());
  }
}

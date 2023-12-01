import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { Store } from '@ngxs/store';
import { AuthState } from '../../auth/auth.state';
import { Login, Logout } from '../../auth/auth.actions';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, MatButtonModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss',
})
export class AuthComponent {
  status$ = this.store.select(AuthState.status);

  constructor(private store: Store) {}

  onLogin(): void {
    this.store.dispatch(new Login());
  }

  onLogout(): void {
    this.store.dispatch(new Logout());
  }
}

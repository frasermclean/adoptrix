import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { AuthService } from '@services/auth.service';
import { Processing, Completed, Login, Logout } from './auth.actions';

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

export interface AuthStateModel {
  status: 'initial' | 'busy' | 'logged-out' | 'logged-in';
  name: string;
  email: string;
  role: 'guest' | 'user' | 'admin';
}

@State<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {
    status: 'initial',
    name: '',
    email: '',
    role: 'guest',
  },
})
@Injectable()
export class AuthState {
  constructor(private authService: AuthService) {}

  @Action(Login)
  onLogin(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
    this.authService.login();
  }

  @Action(Logout)
  onLogout(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
    this.authService.logout();
  }

  @Action(Processing)
  onProcessing(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
  }

  @Action(Completed)
  onCompleted(context: StateContext<AuthStateModel>, action: Completed) {
    context.patchState({
      status: action.data.isLoggedIn ? 'logged-in' : 'logged-out',
      name: action.data.name || '',
      email: action.data.email || '',
      role: action.data.isLoggedIn ? (adminUserIds.includes(action.data.userId || '') ? 'admin' : 'user') : 'guest',
    });
  }

  @Selector()
  static status(state: AuthStateModel) {
    return state.status;
  }

  @Selector()
  static email(state: AuthStateModel) {
    return state.email;
  }

  @Selector()
  static role(state: AuthStateModel) {
    return state.role;
  }
}

const adminUserIds = [
  // TODO: Replace with proper authentication logic
  '6a63381f-4477-4899-8a37-bfb2c109c62d',
];

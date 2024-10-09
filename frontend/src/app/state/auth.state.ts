import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { AuthService } from '@services/auth.service';
import { AuthActions } from './auth.actions';

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

  @Action(AuthActions.Login)
  onLogin(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
    this.authService.login();
  }

  @Action(AuthActions.Logout)
  onLogout(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
    this.authService.logout();
  }

  @Action(AuthActions.Processing)
  onProcessing(context: StateContext<AuthStateModel>) {
    context.patchState({ status: 'busy' });
  }

  @Action(AuthActions.Completed)
  onCompleted(context: StateContext<AuthStateModel>, action: AuthActions.Completed) {
    context.patchState({
      status: action.data.isLoggedIn ? 'logged-in' : 'logged-out',
      name: action.data.name || '',
      email: action.data.email || '',
    });
    context.dispatch(new AuthActions.GetTokenClaims());
  }

  @Action(AuthActions.GetTokenClaims)
  async onGetTokenClaims(context: StateContext<AuthStateModel>) {
    await this.authService.getTokenClaims();
  }

  @Action(AuthActions.TokenClaimsRetrieved)
  onTokenClaimsRetrieved(context: StateContext<AuthStateModel>, action: AuthActions.TokenClaimsRetrieved) {
    const role = action.groups.includes(adminGroupId) ? 'admin' : 'user';
    context.patchState({ role });
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

const adminGroupId = '0def1fe4-2aec-47d8-9653-5cdc09501c31';

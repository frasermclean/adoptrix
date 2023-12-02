import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { AuthService } from '@services/auth.service';
import { Processing, Completed, Login, Logout } from './auth.actions';

const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

export interface AuthStateModel {
  status: 'initial' | 'busy' | 'logged-out' | 'logged-in';
}

@State<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {
    status: 'initial',
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
      status: action.accountInfo ? 'logged-in' : 'logged-out',
    });
  }

  @Selector()
  static status(state: AuthStateModel) {
    return state.status;
  }
}

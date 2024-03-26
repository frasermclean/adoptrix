import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, ofActionDispatched } from '@ngxs/store';
import { RouterActions } from '@state/router.actions';

@Injectable({ providedIn: 'root' })
export class RouteHandler {
  constructor(private router: Router, private actions$: Actions) {
    this.actions$
      .pipe(ofActionDispatched(RouterActions.Navigate))
      .subscribe(({ payload }) => this.router.navigate(payload));
  }
}

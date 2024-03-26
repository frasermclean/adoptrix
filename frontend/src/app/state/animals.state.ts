import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap } from 'rxjs';

import { AnimalsService } from '@services/animals.service';
import { NotificationService } from '@services/notification.service';
import { AnimalsActions } from './animals.actions';
import { Animal, SearchAnimalsResult } from '@models/animal.models';
import { RouterActions } from './router.actions';

interface AnimalsStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  searchResults: SearchAnimalsResult[];
  currentAnimal: Animal | null;
}

const ANIMALS_STATE_TOKEN = new StateToken<AnimalsStateModel>('animals');

@State<AnimalsStateModel>({
  name: ANIMALS_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    searchResults: [],
    currentAnimal: null,
  },
})
@Injectable()
export class AnimalsState {
  constructor(private animalsService: AnimalsService, private notificationService: NotificationService) {}

  @Action(AnimalsActions.Search)
  searchAnimals(context: StateContext<AnimalsStateModel>, action: AnimalsActions.Search) {
    context.patchState({ state: 'busy' });
    return this.animalsService.searchAnimals(action.params).pipe(
      tap((searchResults) => {
        context.patchState({ state: 'ready', searchResults });
        this.notificationService.showNotification(`Found ${searchResults.length} animals`);
      }),
      catchError((error) => {
        context.patchState({ state: 'error', searchResults: [], error });
        throw error;
      })
    );
  }

  @Action(AnimalsActions.Get)
  getAnimal(context: StateContext<AnimalsStateModel>, action: AnimalsActions.Get) {
    context.patchState({ state: 'busy', currentAnimal: null });
    return this.animalsService.getAnimal(action.animalId).pipe(
      tap((animal) => {
        context.patchState({ state: 'ready', currentAnimal: animal });
      }),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Action(AnimalsActions.Add)
  addAnimal(context: StateContext<AnimalsStateModel>, action: AnimalsActions.Add) {
    context.patchState({ state: 'busy' });
    return this.animalsService.addAnimal(action.request).pipe(
      tap((animal) => {
        context.patchState({ state: 'ready', currentAnimal: animal });
        context.dispatch(new RouterActions.Navigate(['/animals', animal.id]));
        this.notificationService.showNotification('Animal added successfully');
      }),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Action(AnimalsActions.Update)
  updateAnimal(context: StateContext<AnimalsStateModel>, action: AnimalsActions.Update) {
    context.patchState({ state: 'busy' });
    return this.animalsService.updateAnimal(action.animalId, action.request).pipe(
      tap((animal) => {
        context.patchState({ state: 'ready', currentAnimal: animal });
        this.notificationService.showNotification('Animal updated successfully');
      }),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Action(AnimalsActions.Delete)
  deleteAnimal(context: StateContext<AnimalsStateModel>, action: AnimalsActions.Delete) {
    context.patchState({ state: 'busy' });
    return this.animalsService.deleteAnimal(action.animalId).pipe(
      tap(() => {
        context.patchState({ state: 'ready', currentAnimal: null });
        context.dispatch(new RouterActions.Navigate(['/animals']));
        this.notificationService.showNotification('Animal deleted successfully');
      }),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Selector()
  static state(state: AnimalsStateModel) {
    return state.state;
  }

  @Selector()
  static searchResults(state: AnimalsStateModel) {
    return state.searchResults;
  }

  @Selector()
  static currentAnimal(state: AnimalsStateModel) {
    return state.currentAnimal;
  }
}
import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken, Store, createSelector } from '@ngxs/store';
import { catchError, tap } from 'rxjs';

import { AnimalsService } from '@services/animals.service';
import { GetAnimal, SearchAnimals } from './animals.actions';
import { Animal, SearchAnimalsResult } from '@models/animal.models';

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
  constructor(private animalsService: AnimalsService, private store: Store) {}

  @Action(SearchAnimals)
  searchAnimals(context: StateContext<AnimalsStateModel>, action: SearchAnimals) {
    context.patchState({ state: 'busy' });
    return this.animalsService.searchAnimals(action.params).pipe(
      tap((searchResults) => context.patchState({ state: 'ready', searchResults })),
      catchError((error) => {
        context.patchState({ state: 'error', searchResults: [], error });
        throw error;
      })
    );
  }

  @Action(GetAnimal)
  getAnimal(context: StateContext<AnimalsStateModel>, action: GetAnimal) {
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

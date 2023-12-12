import { Injectable } from '@angular/core';
import { Animal } from '@models/animal.model';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, of, tap } from 'rxjs';

import { GetAnimal, SearchAnimals } from './animals.actions';
import { AnimalsService } from '@services/animals.service';

const ANIMALS_STATE_TOKEN = new StateToken<AnimalsStateModel>('animals');
interface AnimalsStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  animals: Animal[];
  currentAnimal: Animal | null;
}

@State<AnimalsStateModel>({
  name: ANIMALS_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    animals: [],
    currentAnimal: null,
  },
})
@Injectable()
export class AnimalsState {
  constructor(private animalsService: AnimalsService) {}

  @Action(SearchAnimals)
  searchAnimals(context: StateContext<AnimalsStateModel>, action: SearchAnimals) {
    context.patchState({ state: 'busy' });
    return this.animalsService.searchAnimals(action.params).pipe(
      tap((animals) => context.patchState({ state: 'ready', animals })),
      catchError((error) => {
        context.patchState({ state: 'error', animals: [], error });
        return of(error);
      })
    );
  }

  @Action(GetAnimal)
  getAnimal(context: StateContext<AnimalsStateModel>, action: GetAnimal) {
    context.patchState({ state: 'busy' });
    return this.animalsService.getAnimal(action.id).pipe(
      tap((currentAnimal) => context.patchState({ state: 'ready', currentAnimal })),
      catchError((error) => {
        context.patchState({ state: 'error', currentAnimal: null, error });
        return of(error);
      })
    );
  }

  @Selector()
  static state(state: AnimalsStateModel) {
    return state.state;
  }

  @Selector()
  static animals(state: AnimalsStateModel) {
    return state.animals;
  }

  @Selector()
  static currentAnimal(state: AnimalsStateModel) {
    return state.currentAnimal;
  }
}

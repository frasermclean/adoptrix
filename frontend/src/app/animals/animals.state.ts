import { Injectable } from '@angular/core';
import { Animal } from '@models/animal.model';
import { Action, Selector, State, StateContext, StateToken, createSelector } from '@ngxs/store';
import { catchError, of, tap } from 'rxjs';

import { GetAnimal, SearchAnimals } from './animals.actions';
import { AnimalsService } from '@services/animals.service';

const ANIMALS_STATE_TOKEN = new StateToken<AnimalsStateModel>('animals');
interface AnimalsStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  animals: Animal[];
  lastUpdate: Date | null;
}

@State<AnimalsStateModel>({
  name: ANIMALS_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    animals: [],
    lastUpdate: null,
  },
})
@Injectable()
export class AnimalsState {
  constructor(private animalsService: AnimalsService) {}

  @Action(SearchAnimals)
  searchAnimals(context: StateContext<AnimalsStateModel>, action: SearchAnimals) {
    context.patchState({ state: 'busy' });
    return this.animalsService.searchAnimals(action.params).pipe(
      tap((animals) => context.patchState({ state: 'ready', lastUpdate: new Date(), animals })),
      catchError((error) => {
        context.patchState({ state: 'error', animals: [], error });
        throw error;
      })
    );
  }

  @Action(GetAnimal)
  getAnimal(context: StateContext<AnimalsStateModel>, action: GetAnimal) {
    // if the animal is already in the store, don't make a request
    const animal = context.getState().animals.find((animal) => animal.id === action.id);
    if (animal) {
      return of(animal);
    }

    // look up the animal
    context.patchState({ state: 'busy' });
    return this.animalsService.getAnimal(action.id).pipe(
      tap((animal) => context.patchState({ state: 'ready', animals: [animal] })),
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
  static animals(state: AnimalsStateModel) {
    return state.animals;
  }

  @Selector()
  static animal(state: AnimalsStateModel) {
    return (id: string) => state.animals.find((animal) => animal.id === id);
  }
}

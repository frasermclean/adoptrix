import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap } from 'rxjs';
import { Breed } from '@models/breed.models';
import { BreedsService } from '@services/breeds.service';
import { BreedsActions } from './breeds.actions';

interface BreedStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  searchResults: Breed[];
}

const BREEDS_STATE_TOKEN = new StateToken<BreedStateModel>('breeds');

@State<BreedStateModel>({
  name: BREEDS_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    searchResults: [],
  },
})
@Injectable()
export class BreedsState {
  constructor(private breedsService: BreedsService) {}

  @Action(BreedsActions.Search)
  searchBreeds(context: StateContext<BreedStateModel>, action: BreedsActions.Search) {
    context.patchState({ state: 'busy', searchResults: [] });
    return this.breedsService.searchBreeds(action.request).pipe(
      tap((paging) => context.patchState({ state: 'ready', searchResults: paging.data })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Selector()
  static searchResults(state: BreedStateModel) {
    return state.searchResults;
  }
}

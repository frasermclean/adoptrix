import { Injectable } from '@angular/core';
import { SearchSpeciesMatch } from '@models/species.model';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap } from 'rxjs';
import { SpeciesService } from '@services/species.service';
import { SpeciesActions } from './species.actions';

interface SpeciesStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  all: SearchSpeciesMatch[];
  matches: SearchSpeciesMatch[];
}

const SPECIES_STATE_TOKEN = new StateToken<SpeciesStateModel>('species');

@State<SpeciesStateModel>({
  name: SPECIES_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    all: [],
    matches: [],
  },
})
@Injectable()
export class SpeciesState {
  constructor(private speciesService: SpeciesService) {}

  @Action(SpeciesActions.Search)
  searchSpecies(context: StateContext<SpeciesStateModel>, action: SpeciesActions.Search) {
    context.patchState({ state: 'busy' });
    return this.speciesService.searchSpecies(action.query).pipe(
      tap((matches) => context.patchState({ state: 'ready', matches: matches })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Action(SpeciesActions.GetAll)
  getAllSpecies(context: StateContext<SpeciesStateModel>) {
    context.patchState({ state: 'busy' });
    return this.speciesService.searchSpecies().pipe(
      tap((all) => context.patchState({ state: 'ready', all })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Selector()
  static allSpecies(state: SpeciesStateModel) {
    return state.all;
  }

  @Selector()
  static matches(state: SpeciesStateModel) {
    return state.matches;
  }
}

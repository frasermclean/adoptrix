import { Injectable } from '@angular/core';
import { Species } from '@models/species.model';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { catchError, tap } from 'rxjs';
import { SpeciesService } from '@services/species.service';
import { SpeciesActions } from './species.actions';

interface SpeciesStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  species: Species[];
}

const SPECIES_STATE_TOKEN = new StateToken<SpeciesStateModel>('species');

@State<SpeciesStateModel>({
  name: SPECIES_STATE_TOKEN,
  defaults: {
    state: 'initial',
    error: null,
    species: [],
  },
})
@Injectable()
export class SpeciesState {
  constructor(private speciesService: SpeciesService) {}

  @Action(SpeciesActions.Search)
  searchSpecies(context: StateContext<SpeciesStateModel>, action: SpeciesActions.Search) {
    context.patchState({ state: 'busy' });
    return this.speciesService.searchSpecies(action.query).pipe(
      tap((paging) => context.patchState({ state: 'ready', species: paging.data })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Action(SpeciesActions.GetAll)
  getAllSpecies(context: StateContext<SpeciesStateModel>) {
    context.patchState({ state: 'busy' });
    return this.speciesService.getAllSpecies().pipe(
      tap((paging) => context.patchState({ state: 'ready', species: paging.data })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Selector()
  static species(state: SpeciesStateModel) {
    return state.species;
  }
}

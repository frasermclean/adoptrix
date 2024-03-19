import { Injectable } from '@angular/core';
import { Species } from '@models/species.model';
import { Action, Selector, State, StateContext, StateToken } from '@ngxs/store';
import { SpeciesService } from '@services/species.service';
import { GetSpecies } from './species.actions';
import { catchError, of, tap } from 'rxjs';

const SPECIES_STATE_TOKEN = new StateToken<SpeciesStateModel>('species');

interface SpeciesStateModel {
  state: 'initial' | 'busy' | 'ready' | 'error';
  error: any;
  species: Species[];
}

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

  @Action(GetSpecies)
  getSpecies(context: StateContext<SpeciesStateModel>, action: GetSpecies) {
    // if the species is already in the store, don't make a request
    const species = context.getState().species.find((s) => s.id === action.speciesId);
    if (species) {
      return of(species);
    }

    // look up the species
    context.patchState({ state: 'busy' });
    return this.speciesService.getSpecies(action.speciesId).pipe(
      tap((species) => context.patchState({ state: 'ready', species: [species] })),
      catchError((error) => {
        context.patchState({ state: 'error', error });
        throw error;
      })
    );
  }

  @Selector()
  static species(state: SpeciesStateModel) {
    return (speciesId: string) => state.species.find((s) => s.id === speciesId);
  }
}

import { Route } from '@angular/router';
import { importProvidersFrom } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { AnimalsListComponent } from './animals-list/animals-list.component';
import { AnimalViewComponent } from './animal-view/animal-view.component';
import { AnimalsState } from '@state/animals.state';
import { BreedsState } from '@state/breeds.state';
import { SpeciesState } from '@state/species.state';

export default [
  {
    path: '',
    pathMatch: 'prefix',
    children: [
      { path: '', component: AnimalsListComponent },
      { path: ':animalId', component: AnimalViewComponent },
    ],
    providers: [importProvidersFrom([NgxsModule.forFeature([AnimalsState, BreedsState, SpeciesState])])],
  },
] satisfies Route[];

import { Route } from '@angular/router';
import { importProvidersFrom } from '@angular/core';
import { NgxsModule } from '@ngxs/store';

import { AnimalsListComponent } from './animals-list/animals-list.component';
import { AnimalViewComponent } from './animal-view/animal-view.component';
import { AnimalsState } from './animals.state';
import { SpeciesState } from './species.state';

export default [
  {
    path: '',
    pathMatch: 'prefix',
    children: [
      { path: '', component: AnimalsListComponent },
      { path: ':animalId', component: AnimalViewComponent },
    ],
    providers: [importProvidersFrom([NgxsModule.forFeature([AnimalsState, SpeciesState])])],
  },
] satisfies Route[];

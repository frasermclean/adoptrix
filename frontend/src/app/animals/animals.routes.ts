import { Route } from '@angular/router';
import { provideStore } from '@ngxs/store';
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
    providers: [provideStore([AnimalsState, BreedsState, SpeciesState])],
  },
] satisfies Route[];

import { Route } from '@angular/router';
import { AnimalsListComponent } from './animals-list/animals-list.component';
import { AnimalViewComponent } from './animal-view/animal-view.component';

export default [
  { path: '', component: AnimalsListComponent },
  { path: ':animalId', component: AnimalViewComponent },
] satisfies Route[];

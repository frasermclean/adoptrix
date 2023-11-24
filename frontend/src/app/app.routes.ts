import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AnimalsListComponent } from './animals/animals-list/animals-list.component';
import { AnimalViewComponent } from './animals/animal-view/animal-view.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'animals', component: AnimalsListComponent },
  { path: 'animals/:animalId', component: AnimalViewComponent },
];

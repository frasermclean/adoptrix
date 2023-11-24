import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AnimalsListComponent } from './animals/animals-list.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'animals', component: AnimalsListComponent },
];

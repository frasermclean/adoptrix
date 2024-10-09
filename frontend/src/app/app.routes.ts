import { Routes } from '@angular/router';
import { HomeComponent } from './core/home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: 'animals',
    loadChildren: () => import('./animals/animals.routes'),
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.routes'),
  },
];

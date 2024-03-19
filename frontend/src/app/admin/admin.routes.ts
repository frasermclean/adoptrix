import { Route } from '@angular/router';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { importProvidersFrom } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { AnimalsState } from '../animals/animals.state';
import { SpeciesState } from '../animals/species.state';

export default [
  {
    path: '',
    pathMatch: 'prefix',
    children: [{ path: '', component: AdminPortalComponent }],
    providers: [importProvidersFrom([NgxsModule.forFeature([AnimalsState, SpeciesState])])],
  },
] satisfies Route[];

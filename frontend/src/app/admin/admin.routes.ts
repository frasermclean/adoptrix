import { Route } from '@angular/router';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';
import { importProvidersFrom } from '@angular/core';
import { NgxsModule } from '@ngxs/store';
import { AnimalsState } from '@state/animals.state';
import { SpeciesState } from '@state/species.state';
import { BreedsState } from '@state/breeds.state';

export default [
  {
    path: '',
    pathMatch: 'prefix',
    children: [{ path: '', component: AdminPortalComponent }],
    providers: [importProvidersFrom([NgxsModule.forFeature([AnimalsState, BreedsState, SpeciesState])])],
  },
] satisfies Route[];

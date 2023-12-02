import { Route } from '@angular/router';
import { AdminPortalComponent } from './admin-portal/admin-portal.component';

export default [
  {
    path: '',
    pathMatch: 'prefix',
    children: [{ path: '', component: AdminPortalComponent }],
  },
] satisfies Route[];

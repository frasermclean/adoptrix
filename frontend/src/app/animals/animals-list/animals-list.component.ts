import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Store } from '@ngxs/store';
import { Subject, filter, takeUntil } from 'rxjs';

import { AnimalListItemComponent } from './animal-list-item/animal-list-item.component';
import { SearchControlsComponent } from './search-controls/search-controls.component';
import { AnimalsState } from '@state/animals.state';
import { AnimalsActions } from '@state/animals.actions';
import { AuthState } from '@state/auth.state';
import { LoadingSpinnerComponent } from '@shared/loading-spinner/loading-spinner.component';
import { AnimalEditComponent, AnimalEditData } from '../animal-edit/animal-edit.component';
import { SetAnimalRequest } from '@models/animal.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-animals',
  standalone: true,
  imports: [
    CommonModule,
    AnimalListItemComponent,
    MatButtonModule,
    MatDialogModule,
    MatDividerModule,
    MatIconModule,
    MatTooltipModule,
    SearchControlsComponent,
    LoadingSpinnerComponent,
  ],
  templateUrl: './animals-list.component.html',
  styleUrl: './animals-list.component.scss',
})
export class AnimalsListComponent implements OnInit, OnDestroy {
  readonly state$ = this.store.select(AnimalsState.state);
  readonly animals$ = this.store.select(AnimalsState.searchResults);
  readonly userRole$ = this.store.select(AuthState.role);

  readonly destroy$ = new Subject<void>();

  constructor(private store: Store, private dialog: MatDialog, private router: Router) {}

  ngOnInit(): void {
    this.store.dispatch(new AnimalsActions.Search());
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onAddAnimal() {
    this.dialog
      .open<AnimalEditComponent, AnimalEditData, SetAnimalRequest>(AnimalEditComponent)
      .afterClosed()
      .pipe(
        filter((result) => !!result),
        takeUntil(this.destroy$)
      )
      .subscribe((request) => this.store.dispatch(new AnimalsActions.Add(request!)));
  }
}

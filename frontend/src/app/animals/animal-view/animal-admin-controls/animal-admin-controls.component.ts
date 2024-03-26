import { CommonModule } from '@angular/common';
import { Component, Input, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { Store } from '@ngxs/store';
import { Subject, filter, takeUntil } from 'rxjs';

import { AnimalEditComponent, AnimalEditData } from '../../animal-edit/animal-edit.component';
import { Animal, SetAnimalRequest } from '@models/animal.models';
import {
  ConfirmationPromptComponent,
  ConfirmationPromptData,
} from '@shared/confirmation-prompt/confirmation-prompt.component';
import { AnimalsActions } from '@state/animals.actions';

@Component({
  selector: 'app-animal-admin-controls',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './animal-admin-controls.component.html',
  styleUrl: './animal-admin-controls.component.scss',
})
export class AnimalAdminControlsComponent implements OnDestroy {
  @Input({ required: true }) animal!: Animal;

  private readonly destroy$ = new Subject<void>();

  constructor(private store: Store, private dialog: MatDialog) {}

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onEdit() {
    this.dialog
      .open<AnimalEditComponent, AnimalEditData, SetAnimalRequest>(AnimalEditComponent, {
        data: { animal: this.animal },
      })
      .afterClosed()
      .pipe(
        filter((request) => !!request),
        takeUntil(this.destroy$)
      )
      .subscribe((request) => this.store.dispatch(new AnimalsActions.Update(this.animal.id, request!)));
  }

  onDelete() {
    this.dialog
      .open<ConfirmationPromptComponent, ConfirmationPromptData, boolean>(ConfirmationPromptComponent, {
        data: {
          title: `Delete ${this.animal.name}`,
          message: `Are you sure you want to delete ${this.animal.name}?`,
          confirmText: 'Yes, delete',
          cancelText: 'No, cancel',
        },
      })
      .afterClosed()
      .pipe(
        filter((isConfirmed) => !!isConfirmed),
        takeUntil(this.destroy$)
      )
      .subscribe(() => this.store.dispatch(new AnimalsActions.Delete(this.animal.id)));
  }
}

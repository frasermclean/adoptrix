import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { AnimalEditComponent, AnimalEditData } from '../../animal-edit/animal-edit.component';
import { Animal } from '@models/animal.models';

@Component({
  selector: 'app-animal-admin-controls',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './animal-admin-controls.component.html',
  styleUrl: './animal-admin-controls.component.scss',
})
export class AnimalAdminControlsComponent {
  @Input({ required: true }) animal!: Animal;

  constructor(private dialog: MatDialog) {}

  onEdit() {
    this.dialog.open<AnimalEditComponent, AnimalEditData>(AnimalEditComponent, {
      data: { animal: this.animal },
    });
  }

  onDelete() {}
}

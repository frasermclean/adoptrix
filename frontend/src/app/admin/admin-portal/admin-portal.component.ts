import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AnimalEditComponent } from '../../animals/animal-edit/animal-edit.component';

@Component({
  selector: 'app-admin-portal',
  standalone: true,
  imports: [MatButtonModule, MatDialogModule],
  templateUrl: './admin-portal.component.html',
  styleUrl: './admin-portal.component.scss',
})
export class AdminPortalComponent {
  constructor(private dialogService: MatDialog) {}

  addAnimal() {
    this.dialogService.open(AnimalEditComponent);
  }
}

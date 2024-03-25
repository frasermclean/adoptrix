import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-animal-admin-controls',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './animal-admin-controls.component.html',
  styleUrl: './animal-admin-controls.component.scss',
})
export class AnimalAdminControlsComponent {
  @Input({ required: true }) animalId!: string;

  onEdit() {}

  onDelete() {}
}

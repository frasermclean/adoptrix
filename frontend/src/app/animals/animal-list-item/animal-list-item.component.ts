import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AnimalSearchResult } from '../../models/api-responses';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-animal-list-item',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './animal-list-item.component.html',
  styleUrl: './animal-list-item.component.scss',
})
export class AnimalListItemComponent {
  @Input({ required: true }) animal!: AnimalSearchResult;
}

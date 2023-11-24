import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

import { AnimalSearchResult } from '../../../models/api-responses';

@Component({
  selector: 'app-animal-list-item',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './animal-list-item.component.html',
  styleUrl: './animal-list-item.component.scss',
})
export class AnimalListItemComponent {
  @Input({ required: true }) animal!: AnimalSearchResult;

  getImageUrl(animal: AnimalSearchResult) {
    return animal.imageUrl ? animal.imageUrl : `images/${animal.species}.png`;
  }
}

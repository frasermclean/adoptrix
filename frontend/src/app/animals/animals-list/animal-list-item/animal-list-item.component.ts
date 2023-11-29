import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { Animal } from '@models/animal.model';

@Component({
  selector: 'app-animal-list-item',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatDividerModule,
  ],
  templateUrl: './animal-list-item.component.html',
  styleUrl: './animal-list-item.component.scss',
})
export class AnimalListItemComponent {
  @Input({ required: true }) animal!: Animal;

  get imageUrl() {
    return this.animal.images.length > 0
      ? this.animal.images[0].uri
      : `images/${this.animal.species.toLowerCase()}.png`;
  }

  get imageAltText() {
    return this.animal.images.length > 0
      ? this.animal.images[0].description
      : `Placeholder image of a ${this.animal.species}`;
  }
}

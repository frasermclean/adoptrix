import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { SearchAnimalsItem } from '@models/animal.models';
import { SpeciesIconComponent } from '@shared/species-icon/species-icon.component';

@Component({
  selector: 'app-animal-list-item',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatDividerModule, SpeciesIconComponent],
  templateUrl: './animal-list-item.component.html',
  styleUrl: './animal-list-item.component.scss',
})
export class AnimalListItemComponent {
  @Input({ required: true }) result!: SearchAnimalsItem;

  get defaultImageUrl() {
    return `images/${this.result.speciesName.toLowerCase()}.png`;
  }
}

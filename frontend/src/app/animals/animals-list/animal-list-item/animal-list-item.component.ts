import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { SearchAnimalsResult } from '@models/animal.models';
import { ImageUrlService } from '@services/image-url.service';

@Component({
  selector: 'app-animal-list-item',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatDividerModule],
  templateUrl: './animal-list-item.component.html',
  styleUrl: './animal-list-item.component.scss',
})
export class AnimalListItemComponent {
  @Input({ required: true }) animal!: SearchAnimalsResult;

  constructor(private imageUrlService: ImageUrlService) {}

  get imageUrl() {
    return this.animal.image
      ? this.imageUrlService.getPreviewUrl(this.animal.id, this.animal.image)
      : this.imageUrlService.getSpeciesDefaultImageUrl(this.animal.speciesName);
  }

  get imageAltText() {
    return this.animal.image ? this.animal.image.description : 'Placeholder image';
  }
}

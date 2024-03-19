import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { Store } from '@ngxs/store';
import { map } from 'rxjs';

import { LoadingSpinnerComponent } from '@shared/loading-spinner/loading-spinner.component';
import { ImageGalleryComponent } from '@shared/image-gallery/image-gallery.component';
import { AnimalDetailsComponent } from './animal-details/animal-details.component';
import { AnimalsState } from '../animals.state';
import { GetAnimal } from '../animals.actions';

@Component({
  selector: 'app-animal-view',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatButtonModule,
    LoadingSpinnerComponent,
    ImageGalleryComponent,
    AnimalDetailsComponent,
  ],
  templateUrl: './animal-view.component.html',
  styleUrl: './animal-view.component.scss',
})
export class AnimalViewComponent implements OnInit {
  @Input({ required: true }) animalId!: string;
  readonly state$ = this.store.select(AnimalsState.state);
  readonly animal$ = this.store.select(AnimalsState.currentAnimal);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new GetAnimal(this.animalId));
  }
}

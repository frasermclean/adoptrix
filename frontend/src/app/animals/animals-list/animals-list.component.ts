import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Store } from '@ngxs/store';
import { AnimalListItemComponent } from './animal-list-item/animal-list-item.component';
import { SearchControlsComponent } from './search-controls/search-controls.component';
import { AnimalsState } from '../animals.state';
import { SearchAnimals } from '../animals.actions';
import { LoadingSpinnerComponent } from '@shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-animals',
  standalone: true,
  imports: [CommonModule, AnimalListItemComponent, SearchControlsComponent, LoadingSpinnerComponent],
  templateUrl: './animals-list.component.html',
  styleUrl: './animals-list.component.scss',
})
export class AnimalsListComponent implements OnInit {
  readonly state$ = this.store.select(AnimalsState.state);
  readonly animals$ = this.store.select(AnimalsState.animals);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SearchAnimals());
  }
}

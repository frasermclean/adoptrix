import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';
import { Store } from '@ngxs/store';
import { AnimalListItemComponent } from './animal-list-item/animal-list-item.component';
import { SearchControlsComponent } from './search-controls/search-controls.component';
import { AnimalsState } from '@state/animals.state';
import { AnimalsActions } from '@state/animals.actions';
import { LoadingSpinnerComponent } from '@shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-animals',
  standalone: true,
  imports: [CommonModule, AnimalListItemComponent, MatDividerModule, SearchControlsComponent, LoadingSpinnerComponent],
  templateUrl: './animals-list.component.html',
  styleUrl: './animals-list.component.scss',
})
export class AnimalsListComponent implements OnInit {
  readonly state$ = this.store.select(AnimalsState.state);
  readonly animals$ = this.store.select(AnimalsState.searchResults);

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new AnimalsActions.Search());
  }
}

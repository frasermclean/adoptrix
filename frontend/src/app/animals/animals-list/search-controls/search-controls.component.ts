import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Store } from '@ngxs/store';
import { SpeciesActions } from '@state/species.actions';
import { SpeciesState } from '@state/species.state';
import { AnimalsActions } from '@state/animals.actions';
import { Sex } from '@models/sex.enum';

interface SearchData {
  speciesId?: string;
  sex?: Sex;
}

@Component({
  selector: 'app-search-controls',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
  ],
  templateUrl: './search-controls.component.html',
  styleUrl: './search-controls.component.scss',
})
export class SearchControlsComponent implements OnInit {
  speciesMatches$ = this.store.select(SpeciesState.matches);
  data = signal<SearchData>({});

  constructor(private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.Search({ withAnimals: true }));
    this.store.dispatch(new AnimalsActions.Search());
  }

  onSpeciesChanged(speciesId: string): void {
    this.data.update((value) => ({ ...value, speciesId: speciesId }));
    this.updateSearch();
  }

  onSexChanged(sex: Sex): void {
    this.data.update((value) => ({ ...value, sex: sex }));
    this.updateSearch();
  }

  private updateSearch() {
    this.store.dispatch(new AnimalsActions.Search(this.data()));
  }
}

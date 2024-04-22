import { Component, OnInit } from '@angular/core';
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

  constructor(private store: Store) {}
  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.Search({ withAnimals: true }));
  }
  value = '';

  onSpeciesChanged(speciesId: string): void {
    console.log(speciesId);
  }
}

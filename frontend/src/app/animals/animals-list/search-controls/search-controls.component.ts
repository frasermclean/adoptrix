import { Component, effect, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { Store } from '@ngxs/store';
import { SpeciesActions } from '@state/species.actions';
import { SpeciesState } from '@state/species.state';
import { AnimalsActions } from '@state/animals.actions';
import { Sex } from '@models/sex.enum';
import { BreedsState } from '@state/breeds.state';
import { BreedsActions } from '@state/breeds.actions';
import { SpeciesIconComponent } from '@shared/species-icon/species-icon.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faMars, faVenus } from '@fortawesome/free-solid-svg-icons'

@Component({
  selector: 'app-search-controls',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatBadgeModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    FontAwesomeModule,
    SpeciesIconComponent
],
  templateUrl: './search-controls.component.html',
  styleUrl: './search-controls.component.scss',
})
export class SearchControlsComponent implements OnInit {
  readonly species$ = this.store.select(SpeciesState.species);
  readonly breeds$ = this.store.select(BreedsState.breeds);

  readonly speciesName = signal<string | undefined>(undefined);
  readonly breedName = signal<string | undefined>(undefined);
  readonly sex = signal<Sex | undefined>(undefined);

  readonly maleIcon = faMars;
  readonly femaleIcon = faVenus;

  constructor(private store: Store) {
    // dispatch search breeds action when speciesName changes
    effect(() => {
      const speciesName = this.speciesName();
      this.store.dispatch(new BreedsActions.Search({ speciesName, withAnimals: true }));
    });

    // dispatch search animals action when any of the search criteria changes
    effect(() => {
      const request = {
        speciesName: this.speciesName(),
        breedName: this.breedName(),
        sex: this.sex(),
      };

      this.store.dispatch(new AnimalsActions.Search(request));
    });
  }

  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.Search({ withAnimals: true }));
  }
}

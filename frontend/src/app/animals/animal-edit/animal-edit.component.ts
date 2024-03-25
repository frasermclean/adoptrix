import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { Store } from '@ngxs/store';
import { SpeciesState } from '@state/species.state';
import { BreedsActions } from '@state/breeds.actions';
import { BreedsState } from '@state/breeds.state';
import { SpeciesActions } from '@state/species.actions';
import { AnimalsActions } from '@state/animals.actions';
import { Sex } from '@models/sex.enum';

@Component({
  selector: 'app-animal-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatDatepickerModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
  ],
  templateUrl: './animal-edit.component.html',
  styleUrl: './animal-edit.component.scss',
})
export class AnimalEditComponent implements OnInit {
  formGroup = this.formBuilder.group({
    name: ['', Validators.required],
    speciesId: ['', Validators.required],
    breedId: ['', Validators.required],
    sex: [Sex.Male, Validators.required],
    dateOfBirth: [new Date(), Validators.required],
    description: this.formBuilder.control<string | null>(null),
  });

  allSpecies = this.store.select(SpeciesState.allSpecies);
  breedsSearchResults = this.store.select(BreedsState.searchResults);

  constructor(private formBuilder: NonNullableFormBuilder, private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.GetAll());
    this.store.dispatch(new BreedsActions.ClearSearchResults());
  }

  onSpeciesChanged(speciesId: string) {
    this.store.dispatch(new BreedsActions.Search({ speciesId }));
  }

  onSubmit() {
    const value = this.formGroup.getRawValue();
    this.store.dispatch(
      new AnimalsActions.Add({
        ...value,
        dateOfBirth: value.dateOfBirth.toISOString().split('T')[0],
      })
    );
  }
}

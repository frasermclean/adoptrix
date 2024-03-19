import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
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

@Component({
  selector: 'app-animal-edit',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
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
    sex: ['', Validators.required],
  });

  allSpecies = this.store.select(SpeciesState.allSpecies);
  breedsSearchResults = this.store.select(BreedsState.searchResults);

  constructor(private formBuilder: FormBuilder, private store: Store) {}

  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.GetAll());
    this.store.dispatch(new BreedsActions.ClearSearchResults());
  }

  onSpeciesChanged(speciesId: string) {
    this.store.dispatch(new BreedsActions.Search({ speciesId }));
  }

  onSubmit() {
    console.log(this.formGroup.value);
  }
}

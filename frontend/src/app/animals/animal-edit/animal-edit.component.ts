import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatSelectModule } from '@angular/material/select';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Store } from '@ngxs/store';
import { SpeciesState } from '@state/species.state';
import { BreedsActions } from '@state/breeds.actions';
import { BreedsState } from '@state/breeds.state';
import { SpeciesActions } from '@state/species.actions';
import { Sex } from '@models/sex.enum';
import { Animal, SetAnimalRequest } from '@models/animal.models';
import { AssistantsService } from '@services/assistants.service';

export interface AnimalEditData {
  animal: Animal;
}

interface AnimalEditForm {
  name: FormControl<string>;
  speciesId: FormControl<string>;
  breedId: FormControl<string>;
  sex: FormControl<Sex>;
  dateOfBirth: FormControl<Date>;
  description: FormControl<string | null>;
}

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
    MatIconModule,
    MatInputModule,
    MatRadioModule,
    MatSelectModule,
    MatTooltipModule,
  ],
  templateUrl: './animal-edit.component.html',
  styleUrl: './animal-edit.component.scss',
})
export class AnimalEditComponent implements OnInit {
  readonly formGroup: FormGroup<AnimalEditForm>;
  private readonly animal: Animal | undefined;
  readonly allSpecies$ = this.store.select(SpeciesState.allSpecies);
  readonly breedsSearchResults$ = this.store.select(BreedsState.searchResults);

  get isEditing() {
    return this.animal !== undefined;
  }

  get isDescriptionEmpty() {
    const value = this.formGroup.controls.description.getRawValue();
    return value === null || value === '';
  }

  constructor(
    private store: Store,
    private assistantsService: AssistantsService,
    private dialogRef: MatDialogRef<AnimalEditComponent, SetAnimalRequest>,
    formBuilder: NonNullableFormBuilder,
    @Inject(MAT_DIALOG_DATA) data?: AnimalEditData
  ) {
    this.animal = data?.animal;
    this.formGroup = formBuilder.group({
      name: [data?.animal.name || '', Validators.required],
      speciesId: [data?.animal ? data.animal.speciesId : '', Validators.required],
      breedId: [data?.animal ? data.animal.breedId : '', Validators.required],
      sex: [data?.animal ? data.animal.sex : Sex.Male, Validators.required],
      dateOfBirth: [data?.animal ? new Date(data.animal.dateOfBirth) : new Date(), Validators.required],
      description: [data?.animal ? data.animal.description : null],
    });
  }

  ngOnInit(): void {
    this.store.dispatch(new SpeciesActions.GetAll());
    if (this.animal) {
      this.searchBreeds(this.animal.speciesId);
    }
  }

  onSpeciesChanged(speciesId: string) {
    this.searchBreeds(speciesId);
    this.formGroup.controls.breedId.setValue('');
  }

  onSubmit() {
    const value = this.formGroup.getRawValue();
    this.dialogRef.close(value);
  }

  async getDescriptionSuggestion() {
    const speciesName = this.store.selectSnapshot(SpeciesState.speciesName(this.formGroup.controls.speciesId.value));

    const response = await this.assistantsService.generateAnimalDescription(
      this.formGroup.controls.name.value,
      this.formGroup.controls.breedId.value,
      speciesName || '',
      this.formGroup.controls.sex.value,
      this.formGroup.controls.dateOfBirth.value
    );

    this.formGroup.controls.description.setValue(response.description);
  }

  private searchBreeds(speciesId: string) {
    this.store.dispatch(new BreedsActions.Search({ speciesId }));
  }
}

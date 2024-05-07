import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Sex } from '@models/sex.enum';
import { DateUtilities } from '../utilities/date.utilities';
import { AnimalDescriptionResponse } from '@models/assistants.models';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AssistantsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/assistants`;
  constructor(private httpClient: HttpClient) {}

  async generateAnimalDescription(
    animalName: string,
    breedName: string,
    speciesName: string,
    sex: Sex,
    dateOfBirth: Date
  ) {
    const url = `${this.baseUrl}/animal-description`;
    const observable$ = this.httpClient.get<AnimalDescriptionResponse>(url, {
      params: {
        animalName,
        breedName,
        speciesName,
        sex,
        dateOfBirth: DateUtilities.toDateOnlyString(dateOfBirth),
      },
    });

    return lastValueFrom(observable$);
  }
}

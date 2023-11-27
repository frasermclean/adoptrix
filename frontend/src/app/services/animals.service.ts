import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { AnimalResult, AnimalSearchResult } from '@models/api-responses';
import { Animal } from '@models/animal.model';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals() {
    return this.httpClient.get<AnimalSearchResult[]>(`${this.baseUrl}/animals`);
  }

  public getAnimal(animalId: string): Observable<Animal> {
    return this.httpClient
      .get<AnimalResult>(`${this.baseUrl}/animals/${animalId}`)
      .pipe(
        map((animal) => {
          return {
            ...animal,
            dateOfBirth: new Date(animal.dateOfBirth),
          };
        })
      );
  }
}

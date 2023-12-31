import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Animal } from '@models/animal.model';
import { Species } from '@models/species.enum';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals(params: SearchAnimalsParams = {}): Observable<Animal[]> {
    let httpParams = new HttpParams();
    if (params.species) {
      httpParams = httpParams.set('species', params.species);
    }
    if (params.name) {
      httpParams = httpParams.set('name', params.name);
    }

    return this.httpClient.get<Animal[]>(`${this.baseUrl}/animals`, { params: httpParams });
  }

  public getAnimal(animalId: string): Observable<Animal> {
    return this.httpClient.get<Animal>(`${this.baseUrl}/animals/${animalId}`);
  }
}

export interface SearchAnimalsParams {
  species?: Species;
  name?: string;
}

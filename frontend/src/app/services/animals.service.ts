import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Animal } from '@models/animal.model';
import { Species } from '@models/species.enum';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals({ species, name }: { species?: Species; name?: string; } = {}) {
    let params = new HttpParams();
    if (species) {
      params = params.set('species', species);
    }
    if (name) {
      params = params.set('name', name);
    }

    return this.httpClient.get<Animal[]>(`${this.baseUrl}/animals`, { params });
  }

  public getAnimal(animalId: string) {
    return this.httpClient.get<Animal>(`${this.baseUrl}/animals/${animalId}`);
  }
}

import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Animal, SearchAnimalsParams, SearchAnimalsResult } from '@models/animal.models';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/animals`;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals(params: SearchAnimalsParams = {}): Observable<SearchAnimalsResult[]> {
    let httpParams = new HttpParams();
    if (params.speciesId) {
      httpParams = httpParams.set('speciesId', params.speciesId);
    }
    if (params.name) {
      httpParams = httpParams.set('name', params.name);
    }

    return this.httpClient.get<SearchAnimalsResult[]>(this.baseUrl, { params: httpParams });
  }

  public getAnimal(animalId: string): Observable<Animal> {
    return this.httpClient.get<Animal>(`${this.baseUrl}/${animalId}`);
  }
}

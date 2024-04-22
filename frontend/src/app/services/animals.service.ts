import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Animal, SearchAnimalsParams, SearchAnimalsResult, SetAnimalRequest } from '@models/animal.models';

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
    const url = `${this.baseUrl}/${animalId}`;
    return this.httpClient.get<Animal>(url);
  }

  public addAnimal(request: SetAnimalRequest): Observable<Animal> {
    return this.httpClient.post<Animal>(this.baseUrl, request);
  }

  public updateAnimal(animalId: string, request: SetAnimalRequest) {
    const url = `${this.baseUrl}/${animalId}`;
    return this.httpClient.put<Animal>(url, request);
  }

  public deleteAnimal(animalId: string) {
    const url = `${this.baseUrl}/${animalId}`;
    return this.httpClient.delete(url);
  }
}

import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GridifyQueryBuilder, ConditionalOperator as op } from 'gridify-client';

import { Animal, SearchAnimalsRequest, SearchAnimalsResult, SetAnimalRequest } from '@models/animal.models';
import { Paging } from '@models/paging.model';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/animals`;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals(request: Partial<SearchAnimalsRequest> = {}): Observable<Paging<SearchAnimalsResult>> {
    const queryBuilder = new GridifyQueryBuilder().setPage(1).setPageSize(20);
    let hasCondition = false;

    if (request.speciesName) {
      queryBuilder.addCondition('speciesName', op.Equal, request.speciesName);
      hasCondition = true;
    }

    if (request.breedName) {
      if (hasCondition) queryBuilder.and();
      queryBuilder.addCondition('breedName', op.Equal, request.breedName);
    }

    if (request.sex) {
      if (hasCondition) queryBuilder.and();
      queryBuilder.addCondition('sex', op.Equal, request.sex);
    }

    const query = queryBuilder.addOrderBy('name').build();
    const params = new HttpParams({ fromObject: { ...query } });

    return this.httpClient.get<Paging<SearchAnimalsResult>>(this.baseUrl, { params });
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

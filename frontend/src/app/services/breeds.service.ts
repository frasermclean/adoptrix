import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GridifyQueryBuilder, ConditionalOperator as op } from 'gridify-client';

import { environment } from '../../environments/environment';
import { Breed, SearchBreedsRequest } from '@models/breed.models';
import { Paging } from '@models/paging.model';

@Injectable({
  providedIn: 'root',
})
export class BreedsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/breeds`;

  constructor(private httpClient: HttpClient) {}

  searchBreeds(request: Partial<SearchBreedsRequest>): Observable<Paging<Breed>> {
    const queryBuilder = new GridifyQueryBuilder();
    let hasCondition = false;

    if (request.speciesName) {
      queryBuilder.addCondition('speciesName', op.Equal, request.speciesName);
      hasCondition = true;
    }

    if (request.withAnimals) {
      if (hasCondition) queryBuilder.and();
      queryBuilder.addCondition('animalCount', op.GreaterThan, 0);
    }

    const httpParams = new HttpParams({ fromObject: { ...queryBuilder.build() } });

    return this.httpClient.get<Paging<Breed>>(this.baseUrl, { params: httpParams });
  }
}

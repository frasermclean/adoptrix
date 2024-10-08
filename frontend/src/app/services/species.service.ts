import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GridifyQueryBuilder, ConditionalOperator as op } from 'gridify-client';

import { environment } from '../../environments/environment';
import { SearchSpeciesRequest, Species } from '@models/species.model';
import { Paging } from '@models/paging.model';

@Injectable({
  providedIn: 'root',
})
export class SpeciesService {
  private readonly baseUrl = `${environment.apiBaseUrl}/species`;

  constructor(private httpClient: HttpClient) {}

  searchSpecies(request?: Partial<SearchSpeciesRequest>): Observable<Paging<Species>> {
    const queryBuilder = new GridifyQueryBuilder().setPage(1).setPageSize(50);

    if (request?.withAnimals) {
      queryBuilder.addCondition('animalCount', op.GreaterThan, 0);
    }

    const query = queryBuilder.addOrderBy('animalCount', true).addOrderBy('name').build();
    const params = new HttpParams({ fromObject: { ...query } });

    return this.httpClient.get<Paging<Species>>(this.baseUrl, { params });
  }

  getSpecies(speciesId: string): Observable<Species> {
    return this.httpClient.get<Species>(`${this.baseUrl}/${speciesId}`);
  }
}

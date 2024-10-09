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
  private readonly maxPageSize = 50;

  constructor(private httpClient: HttpClient) {}

  searchSpecies(request?: Partial<SearchSpeciesRequest>): Observable<Paging<Species>> {
    const queryBuilder = new GridifyQueryBuilder().setPage(1).setPageSize(this.maxPageSize);

    if (request?.withAnimals) {
      queryBuilder.addCondition('animalCount', op.GreaterThan, 0);
    }

    const query = queryBuilder.addOrderBy('animalCount', true).addOrderBy('name').build();
    const params = new HttpParams({ fromObject: { ...query } });

    return this.httpClient.get<Paging<Species>>(this.baseUrl, { params });
  }

  /**
   * Get all species in alphabetical order.
   */
  getAllSpecies(): Observable<Paging<Species>> {
    const query = new GridifyQueryBuilder().setPage(1).setPageSize(this.maxPageSize).addOrderBy('name').build();
    const params = new HttpParams({ fromObject: { ...query } });

    return this.httpClient.get<Paging<Species>>(this.baseUrl, { params });
  }

  getSpecies(speciesName: string): Observable<Species> {
    return this.httpClient.get<Species>(`${this.baseUrl}/${speciesName}`);
  }
}

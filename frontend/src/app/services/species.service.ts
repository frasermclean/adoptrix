import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { SearchSpeciesMatch, SearchSpeciesQuery, Species } from '@models/species.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SpeciesService {
  private readonly baseUrl = `${environment.apiBaseUrl}/species`;

  constructor(private httpClient: HttpClient) {}

  searchSpecies(query?: Partial<SearchSpeciesQuery>): Observable<SearchSpeciesMatch[]> {
    let httpParams = new HttpParams();
    if (query?.withAnimals) {
      httpParams = httpParams.set('withAnimals', query.withAnimals);
    }

    return this.httpClient.get<SearchSpeciesMatch[]>(this.baseUrl, { params: httpParams });
  }

  getSpecies(speciesId: string): Observable<Species> {
    return this.httpClient.get<Species>(`${this.baseUrl}/${speciesId}`);
  }
}

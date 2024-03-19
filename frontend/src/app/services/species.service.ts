import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Species } from '@models/species.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SpeciesService {
  private readonly baseUrl = `${environment.apiBaseUrl}/species`;

  constructor(private httpClient: HttpClient) {}

  getAllSpecies(): Observable<Species[]> {
    return this.httpClient.get<Species[]>(this.baseUrl);
  }

  getSpecies(speciesId: string): Observable<Species> {
    return this.httpClient.get<Species>(`${this.baseUrl}/${speciesId}`);
  }
}

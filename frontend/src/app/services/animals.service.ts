import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AnimalSearchResult } from '../models/api-responses';

@Injectable({
  providedIn: 'root',
})
export class AnimalsService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private httpClient: HttpClient) {}

  public searchAnimals() {
    return this.httpClient.get<AnimalSearchResult[]>(`${this.baseUrl}/animals`);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class BreedsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/breeds`;

  constructor(private httpClient: HttpClient) {}
}

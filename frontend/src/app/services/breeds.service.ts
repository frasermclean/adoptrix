import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BreedResponse, SearchBreedsRequest } from '@models/breed.models';
import { Observable } from 'rxjs';
import { Paging } from '@models/paging.model';

@Injectable({
  providedIn: 'root',
})
export class BreedsService {
  private readonly baseUrl = `${environment.apiBaseUrl}/breeds`;

  constructor(private httpClient: HttpClient) {}

  searchBreeds(request: SearchBreedsRequest): Observable<Paging<BreedResponse>> {
    let httpParams = new HttpParams({
      fromObject: {}

    });
    if (request.speciesId) {
      httpParams = httpParams.set('speciesId', request.speciesId);
    }
    if (request.withAnimals) {
      httpParams = httpParams.set('withAnimals', request.withAnimals);
    }

    return this.httpClient.get<Paging<BreedResponse>>(this.baseUrl, { params: httpParams });
  }
}

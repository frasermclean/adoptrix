import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-admin-portal',
  standalone: true,
  imports: [],
  templateUrl: './admin-portal.component.html',
  styleUrl: './admin-portal.component.scss',
})
export class AdminPortalComponent implements OnInit {
  constructor(private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.httpClient.get(`${environment.apiBaseUrl}/admin/users/me`).subscribe((result) => {
      console.log(result);
    });
  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ContactServeiceService {
  private url = 'https://localhost:7059/api/Contact/GetContactInfo';

  constructor(private http: HttpClient) {}

  getUserByToken() {
    const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json'
  });
  console.log("im token=> ", token)
    return this.http.get<any>(`${this.url}`, { headers });
  }
}

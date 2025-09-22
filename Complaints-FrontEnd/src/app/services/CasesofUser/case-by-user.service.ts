import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CaseByUserService {
  private url = 'https://localhost:7059/api/Contact/contactidfromtoken';

  constructor(private http: HttpClient) {}

  getContactIdFromToken() {
    const token = localStorage.getItem('token');
  const headers = new HttpHeaders({
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json'
  });
    return this.http.get<any>(`${this.url}`, { headers });
  }

  private apicase = 'https://localhost:7059/api/Complaint/cases/customer';

  getComplaintsByCustomerId(customerId: string) {
    const url2 = `${this.apicase}/${customerId}`;
    return this.http.get<any>(url2);
  }

  getcasesperuser() {
    return this.http.get<any>(this.apicase);
  }
}

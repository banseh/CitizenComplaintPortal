import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private baseUrl = 'https://localhost:7059/api/Complaint';

  constructor(private http: HttpClient) {}

  getCaseByTicketNumberAndEmail(ticketNumber: string, email: string)
  {
    const url = `${this.baseUrl}/case/ticketNumber/${ticketNumber}/email/${email}`;
    return this.http.get<any>(url);
  }

}

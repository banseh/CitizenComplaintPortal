import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DeleteComplainService {
  private urlx = 'https://localhost:7059/api/complaint';

  constructor(private http: HttpClient) {}

  Delete(customerId: string) {
    const urlDel = `${this.urlx}/${customerId}`;
    return this.http.delete<any>(urlDel);
  }
}

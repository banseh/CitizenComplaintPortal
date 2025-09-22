import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  private readonly url = 'https://localhost:7059/api/Register/RegisterAcc';

  constructor(private http: HttpClient) {}

  register(user: { firstname: string; lastname: string; email: string; password: string; gender: number })
  {
    return this.http.post(this.url, user);
  }
}

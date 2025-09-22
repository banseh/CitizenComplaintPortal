import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'

})
export class LoginService {
  private readonly url = 'https://localhost:7059/api/Login';

  constructor(private http: HttpClient) {}

  login(data: { email: string, password: string })
  {
    return this.http.post(this.url, data);
  }

}

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { LoginService } from '../../services/Login/login.service';
import { HeaderComponent } from "../../Components/header/header.component";

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  providers: [LoginService],

})
export class LoginComponent {
  email: string = '';
  password: string = '';
  userName:string ='';
  togglePassword: boolean = false;
  showEmptyFieldsAlert: boolean = false;
  loginSuccess: boolean = false;
  loginFailed: boolean = false;
  isLoading = false;
  constructor(private loginService: LoginService,private router:Router) {}
  login() {
    this.isLoading = true;
    console.log("this.email: ",this.email);
    console.log("this.password: ",this.password);
    if (!this.email.trim() || !this.password.trim()) {
      this.loginSuccess = false;
      this.loginFailed = false;
      this.showEmptyFieldsAlert = true;
      this.email = '';
      this.password = '';
      return;
    }
    this.loginService.login({ email: this.email, password: this.password }).subscribe({
      next: (res:any) => {
        this.isLoading = false;
        this.userName = res.data.fullName;
        setTimeout(() => this.loginSuccess = false, 1000);
        console.log("res: ",res);
        this.loginSuccess = true;
        this.loginFailed = false;
        this.showEmptyFieldsAlert = false;
        localStorage.setItem('token', res.data.token);
        localStorage.setItem('name', res.data.fullName);
        console.log('Token: ', res.data.token);
        console.log('Name: ',  res.data.fullName);
      setTimeout(() => this.router.navigate(['/profile'])   ,500);
      },
      error: (err) => {
        this.isLoading = false;
        setTimeout(() => this.loginFailed = false, 500);
        this.loginFailed = true;
        this.loginSuccess = false;
        this.showEmptyFieldsAlert = false;
        this.email = '';
        this.password = '';
      }
    });
  }

  toggleVisibility() {
    this.togglePassword = !this.togglePassword;
  }
}


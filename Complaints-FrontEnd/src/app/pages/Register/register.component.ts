import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterService } from '../../services/Register/register.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [RegisterService],
})
export class RegisterComponent {
  firstname: string = '';
  lastname: string = '';
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  gender: string = '';
  isLoading: boolean = false;
  loginSuccess: boolean = false;
  loginError: boolean = false;
  userName: string = '';
  togglePassword: boolean = false;
  toggleConfirmPassword: boolean = false;
  confirmPasswordMismatch: boolean = false;

  constructor(private registerService: RegisterService,private router:Router) {}

  Register(form: any) {
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.confirmPasswordMismatch = true;
      return;
    } else {
      this.confirmPasswordMismatch = false;
    }

    const userData = {
      firstname: this.firstname,
      lastname: this.lastname,
      email: this.email,
      password: this.password,
      gender: this.gender === 'Male' ? 1 : 2
    };
  this.isLoading = true;
  console.log('User registered: ', userData);
  this.registerService.register(userData).subscribe({
    next: (res:any) => {
      console.log('Registration successful: ', res);
      this.isLoading = false;
      if(res.status){
        this.loginSuccess = true;
        this.userName = this.firstname;
        setTimeout(() => {this.loginSuccess = false; }, 1000);
        setTimeout(() => {this.router.navigate(['/login']);}, 1500);
      }
      else{
        this.loginError = true;
        setTimeout(() => {  this.loginError = false;}, 1500);
      }
    },
    error: (err) => {
        this.loginError = true;
      setTimeout(() => {
        this.loginError = false;
      }, 2500);
    }
  });
  }
  toggleVisibility() {
    this.togglePassword = !this.togglePassword;
  }

  toggleVisibilityConfirmPassword() {
    this.toggleConfirmPassword = !this.toggleConfirmPassword;
  }
}


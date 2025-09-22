
import { Component } from '@angular/core';
import { routes } from './../../app.routes';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-headerlog',
  imports: [RouterLink,CommonModule,RouterModule    ],
    templateUrl: './headerlog.component.html',
  styleUrl: './headerlog.component.css'

})
export class HeaderlogComponent {

  showConfirmPopup = false;
  showSuccessPopup = false;
  isDeleting: boolean = false;

  isMenuOpen = false;
  showNavbar: boolean = true;
  hasToken: boolean = false;
  name:string = '';

  constructor(private router:Router){}
  ngOnInit() {
    this.hasToken = !!localStorage.getItem('token');
    const storedName = localStorage.getItem('name');
      if (storedName) {
        this.name = storedName;
      }
    }

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu() {
    this.isMenuOpen = false;
  }
  continueASAGuest(){
    localStorage.removeItem("token");
    localStorage.removeItem("name");
    localStorage.removeItem("email");
  }

  reset() {
    this.router.navigate(['/']);
  }

  confirmDelete() {
    this.isDeleting = true;
    this.showSuccessPopup = true;
    this.showConfirmPopup = false;
    setTimeout(() => {
      this.showSuccessPopup = false;
      this.isDeleting = false;
    }, 1000);
    setTimeout(() => {
    localStorage.clear() ;
    this.router.navigate(['/home'])
    }, 1000);
  }

  cxMenuu(){
    this.showConfirmPopup = true;
  }
}

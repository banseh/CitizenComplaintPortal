import { routes } from './../../app.routes';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NavigationEnd, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs/operators';
@Component({
  selector: 'app-header',
  imports: [RouterLink,CommonModule,RouterModule    ],
  templateUrl: './header.component.html',

})
export class HeaderComponent {
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
}








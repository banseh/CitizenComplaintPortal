import { FooterComponent } from './Components/footer/footer.component';
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs/operators';
import { HomeComponent } from './pages/home/home.component';
import { HeaderComponent } from './Components/header/header.component';
import { CommonModule } from '@angular/common';
import { HeaderlogComponent } from './Components/headerlog/headerlog.component';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [RouterOutlet ,HeaderComponent ,CommonModule , FooterComponent  , HeaderlogComponent ],
})
export class AppComponent implements OnInit {
  showNavbar: boolean = true;
  constructor(private router: Router) {}
  ngOnInit(): void {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        const hiddenRoutes = ['/login', '/register'  , '/serverdown'];
        this.showNavbar = !hiddenRoutes.includes(event.urlAfterRedirects);
      });
  }

  get hasToken(): boolean {
    return !!localStorage.getItem('token');
  }
}

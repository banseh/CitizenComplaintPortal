import { authGuard } from './pages/AuthGuard/auth.guard';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/Register/register.component';
import { SubmitComplintComponent } from './pages/submit-complint/submit-complint.component';
import { HomeComponent } from './pages/home/home.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ComplaintCategoryComponent } from './pages/complaint-category/complaint-category.component';
import { AboutComponent } from './pages/About/about.component';
import { authLockedGuard } from './pages/LockedGuard/auth-locked.guard';
import { ServerDownComponent } from './pages/server-down/server-down.component';


export const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'submitComplaint', component: SubmitComplintComponent },
  { path: 'login', component: LoginComponent  ,canActivate: [authLockedGuard] },
  { path: 'register', component: RegisterComponent  ,canActivate: [authLockedGuard]},
  { path: 'profile', component: ProfileComponent,canActivate: [authGuard] },
  { path: 'home', component: HomeComponent },
  { path: 'complaint-category', component: ComplaintCategoryComponent },
  { path: 'about', component: AboutComponent },
  {path: 'serverdown' , component:ServerDownComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

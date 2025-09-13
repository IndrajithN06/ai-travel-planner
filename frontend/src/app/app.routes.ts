import { Routes } from '@angular/router';
import { HomeComponent } from '../app/features/home/home.component';
import { LoginComponent } from '../app/features/auth/login/login.component';
import { SignupComponent } from '../app/features/auth/signup/signup.component';
import { AuthGuard } from '../app/core/guards/auth.guard';
import { AuthLayoutComponent } from '../app/layout/auth-layout/auth-layout.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { 
    path: 'auth', 
    component: AuthLayoutComponent,
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignupComponent },
      { path: '', redirectTo: 'login', pathMatch: 'full' }
    ]
  },
  { 
    path: 'dashboard', 
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.dashboardRoutes),
    canActivate: [AuthGuard]
  },
  { 
    path: 'profile', 
    loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.dashboardRoutes),
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: '' }
];

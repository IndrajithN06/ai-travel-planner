import { Routes } from '@angular/router';
import { HomeComponent } from '../app/features/home/home.component';
import { LoginComponent } from '../app/features/auth/login/login.component';
import { SignupComponent } from '../app/features/auth/signup/signup.component';
import { AuthGuard } from '../app/core/guards/auth.guard';
import { AuthLayoutComponent } from '../app/layout/auth-layout/auth-layout.component';
import { TravelCreateComponent } from './features/travel/components/travel-create/travel-create.component';

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
      path: 'create-plan', component: TravelCreateComponent,
      canActivate: [AuthGuard]
  },
  {
    path: 'travel',
    loadChildren: () => import('./features/travel/travel.routes').then(m => m.travelRoutes),
    canActivate: [AuthGuard]
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

import { Routes } from '@angular/router';
import { TravelDemoComponent } from './components/travel-demo/travel-demo.component';
import { TravelPlansListComponent } from './components/travel-plans-list/travel-plans-list.component';
import { TravelPlanDetailsComponent } from './components/travel-plan-details/travel-plan-details.component';

export const travelRoutes: Routes = [
  { path: '', redirectTo: 'demo', pathMatch: 'full' },
  { path: 'demo', component: TravelDemoComponent },
  { path: 'list', component: TravelPlansListComponent },
  { path: 'details/:id', component: TravelPlanDetailsComponent },
  { path: '**', redirectTo: 'demo' }];

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  constructor(
    private router: Router,
    private authService: AuthService
  ) {}

  startPlanning(): void {
    // Check if user is authenticated
    this.authService.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        // User is logged in, go to travel plans
        this.router.navigate(['/travel/list']);
      } else {
        // User is not logged in, go to login
        this.router.navigate(['/auth/login']);
      }
    });
  }
}

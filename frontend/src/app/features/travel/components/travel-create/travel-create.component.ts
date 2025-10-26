import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { TravelService } from '../../services/travel.service';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';

@Component({
  selector: 'app-travel-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule
  ],
  templateUrl: './travel-create.component.html',
  styleUrls: ['./travel-create.component.scss']
})
export class TravelCreateComponent {
  travelForm: FormGroup;
  travelStyles = ['Luxury', 'Budget', 'Adventure', 'Cultural', 'Romantic', 'Family'];
  groupSizes = ['Solo', 'Couple', 'Family', 'Group'];
  loading = false;

  constructor(
    private fb: FormBuilder,
    private travelService: TravelService,
    private snackBar: MatSnackBar,
    private router: Router
  ) {
    this.travelForm = this.fb.group({
      title: ['', Validators.required],
      destination: ['', Validators.required],
      description: [''],
      travelStyle: ['', Validators.required],
      groupSize: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      budget: [0, [Validators.required, Validators.min(0)]],
      isPublic: [false]
    });
  }

  onSubmit(): void {
    if (this.travelForm.invalid) {
      this.snackBar.open('Please fill all required fields correctly.', 'Close', { duration: 3000 });
      return;
    }

    this.loading = true;
    this.travelService.createTravelPlan(this.travelForm.value).subscribe({
      next: (res) => {
        this.loading = false;
        this.snackBar.open('Travel plan created successfully!', 'Close', { duration: 3000 });
        // Redirect to travel plan list or details page
        this.router.navigate(['/travel/list']);
      },
      error: (err) => {
        this.loading = false;
        this.snackBar.open('Failed to create travel plan. Try again.', 'Close', { duration: 4000 });
        console.error(err);
      }
    });
  }
}

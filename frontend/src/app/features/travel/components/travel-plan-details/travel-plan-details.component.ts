import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Subject, takeUntil } from 'rxjs';
import { TravelService } from '../../services/travel.service';
import { TravelPlan, Activity, Accommodation, Transportation } from '../../models/travel-plan.model';
import { DeleteConfirmationDialogComponent } from '../delete-confirmation-dialog/delete-confirmation-dialog.component';

@Component({
  selector: 'app-travel-plan-details',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule
  ],
  templateUrl: './travel-plan-details.component.html',
  styleUrl: './travel-plan-details.component.scss'
})
export class TravelPlanDetailsComponent implements OnInit, OnDestroy {
  travelPlan: TravelPlan | null = null;
  loading = false;
  error: string | null = null;
  planId!: number;

  private destroy$ = new Subject<void>();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private travelService: TravelService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.route.params.pipe(takeUntil(this.destroy$)).subscribe(params => {
      this.planId = +params['id'];
      if (this.planId) {
        this.loadTravelPlan();
      } else {
        this.error = 'Invalid travel plan ID';
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadTravelPlan(): void {
    this.loading = true;
    this.error = null;

    this.travelService.getTravelPlanById(this.planId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (plan) => {
          this.travelPlan = plan;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading travel plan:', error);
          this.error = error.status === 404 
            ? 'Travel plan not found' 
            : 'Unable to load travel plan details';
          this.loading = false;
          this.snackBar.open(this.error, 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
      });
  }

  get duration(): number {
    if (!this.travelPlan) return 0;
    const start = new Date(this.travelPlan.startDate);
    const end = new Date(this.travelPlan.endDate);
    return Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
  }

  get formattedDateRange(): string {
    if (!this.travelPlan) return '';
    const start = new Date(this.travelPlan.startDate);
    const end = new Date(this.travelPlan.endDate);
    return `${start.toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' })} - ${end.toLocaleDateString('en-US', { month: 'long', day: 'numeric', year: 'numeric' })}`;
  }

  get budgetDisplay(): string {
    if (!this.travelPlan?.budget) return 'Not specified';
    return `$${this.travelPlan.budget.toLocaleString()}`;
  }

  get travelStyleIcon(): string {
    if (!this.travelPlan?.travelStyle) return 'explore';
    const style = this.travelPlan.travelStyle.toLowerCase();
    switch (style) {
      case 'luxury': return 'diamond';
      case 'budget': return 'savings';
      case 'adventure': return 'hiking';
      case 'cultural': return 'museum';
      case 'romantic': return 'favorite';
      case 'family': return 'family_restroom';
      default: return 'explore';
    }
  }

  get groupSizeIcon(): string {
    if (!this.travelPlan?.groupSize) return 'people';
    const size = this.travelPlan.groupSize.toLowerCase();
    switch (size) {
      case 'solo': return 'person';
      case 'couple': return 'favorite';
      case 'family': return 'family_restroom';
      case 'group': return 'group';
      default: return 'people';
    }
  }

  get totalActivitiesCost(): number {
    if (!this.travelPlan) return 0;
    return this.travelPlan.activities.reduce((sum, activity) => sum + (activity.cost || 0), 0);
  }

  get totalAccommodationCost(): number {
    if (!this.travelPlan) return 0;
    return this.travelPlan.accommodations.reduce((sum, acc) => {
      const nights = Math.ceil((new Date(acc.checkOutDate).getTime() - new Date(acc.checkInDate).getTime()) / (1000 * 60 * 60 * 24));
      return sum + ((acc.costPerNight || 0) * nights);
    }, 0);
  }

  get totalTransportationCost(): number {
    if (!this.travelPlan) return 0;
    return this.travelPlan.transportations.reduce((sum, trans) => sum + (trans.cost || 0), 0);
  }

  get estimatedTotalCost(): number {
    return this.totalActivitiesCost + this.totalAccommodationCost + this.totalTransportationCost;
  }

  onBack(): void {
    this.router.navigate(['/travel/list']);
  }

  onEdit(): void {
    if (this.travelPlan) {
      // TODO: Navigate to edit form when implemented
      this.snackBar.open('Edit functionality coming soon!', 'Close', {
        duration: 3000
      });
      // Future: this.router.navigate(['/travel/edit', this.travelPlan.id]);
    }
  }

  onDelete(): void {
    if (!this.travelPlan) return;

    const dialogRef = this.dialog.open(DeleteConfirmationDialogComponent, {
      width: '400px',
      data: {
        title: this.travelPlan.title,
        message: 'Are you sure you want to delete this travel plan? This action cannot be undone.'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.deleteTravelPlan();
      }
    });
  }

  private deleteTravelPlan(): void {
    if (!this.travelPlan) return;

    this.loading = true;
    this.travelService.deleteTravelPlan(this.travelPlan.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackBar.open('Travel plan deleted successfully', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          this.router.navigate(['/travel/list']);
        },
        error: (error) => {
          console.error('Error deleting travel plan:', error);
          this.loading = false;
          this.snackBar.open('Failed to delete travel plan', 'Close', {
            duration: 5000,
            panelClass: ['error-snackbar']
          });
        }
      });
  }

  onShare(): void {
    if (!this.travelPlan) return;

    if (navigator.share) {
      navigator.share({
        title: this.travelPlan.title,
        text: `Check out my travel plan: ${this.travelPlan.title} - ${this.travelPlan.destination}`,
        url: window.location.href
      }).catch(() => {
        this.copyToClipboard();
      });
    } else {
      this.copyToClipboard();
    }
  }

  private copyToClipboard(): void {
    if (!this.travelPlan) return;
    const url = window.location.href;
    navigator.clipboard.writeText(url).then(() => {
      this.snackBar.open('Link copied to clipboard!', 'Close', {
        duration: 3000,
        panelClass: ['success-snackbar']
      });
    }).catch(() => {
      this.snackBar.open('Failed to copy link', 'Close', {
        duration: 3000
      });
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  formatDateTime(dateTimeString: string): string {
    return new Date(dateTimeString).toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      hour: 'numeric',
      minute: '2-digit'
    });
  }

  getAccommodationTotalCost(accommodation: Accommodation): number {
    if (!accommodation.costPerNight || !accommodation.checkInDate || !accommodation.checkOutDate) {
      return 0;
    }
    const checkIn = new Date(accommodation.checkInDate);
    const checkOut = new Date(accommodation.checkOutDate);
    const nights = Math.ceil((checkOut.getTime() - checkIn.getTime()) / (1000 * 60 * 60 * 24));
    return accommodation.costPerNight * nights;
  }
}


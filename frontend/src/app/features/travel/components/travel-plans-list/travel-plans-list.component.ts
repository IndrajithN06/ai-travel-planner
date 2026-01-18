import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';
import { TravelService } from '../../services/travel.service';
import { TravelPlan } from '../../models/travel-plan.model';
import { TripCardComponent } from '../trip-card/trip-card.component';

@Component({
  selector: 'app-travel-plans-list',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule,
    FormsModule,
    ReactiveFormsModule,
    TripCardComponent
  ],
  templateUrl: './travel-plans-list.component.html',
  styleUrl: './travel-plans-list.component.scss'
})
export class TravelPlansListComponent implements OnInit, OnDestroy {
  travelPlans: TravelPlan[] = [];
  filteredPlans: TravelPlan[] = [];
  loading = false;
  error: string | null = null;

  // Filter form
  filterForm: FormGroup;

  // Filter options
  travelStyles = ['Luxury', 'Budget', 'Adventure', 'Cultural', 'Romantic', 'Family'];
  groupSizes = ['Solo', 'Couple', 'Family', 'Group'];

  // View options
  viewMode: 'grid' | 'list' = 'grid';
  showPublicOnly = false;

  private destroy$ = new Subject<void>();

  constructor(
    private travelService: TravelService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      search: [''],
      travelStyle: [''],
      groupSize: [''],
      startDate: [''],
      endDate: ['']
    });
  }

  ngOnInit(): void {
    this.loadTravelPlans();
    this.setupFilterSubscription();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadTravelPlans(): void {
this.loading = true;
  this.error = null;

  const serviceCall = this.showPublicOnly 
    ? this.travelService.getPublicTravelPlans()
    : this.travelService.getAllTravelPlans();

  serviceCall
    .pipe(takeUntil(this.destroy$))
    .subscribe({
      next: (plans) => {
        this.travelPlans = plans;
        this.applyFilters();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading travel plans:', error);
        this.error = 'Unable to connect to the server.';
        this.loading = false;
        this.snackBar.open('Failed to load travel plans', 'Close', { 
          duration: 5000,
          panelClass: ['warning-snackbar']
        });
      }
    });
  }


  // private loadSampleData(): void {
  //   this.travelPlans = [
  //     {
  //       id: 1,
  //       destination: 'Paris, France',
  //       title: 'Romantic Paris Getaway',
  //       startDate: '2024-06-01',
  //       endDate: '2024-06-08',
  //       description: 'A magical week in the City of Light, exploring iconic landmarks, enjoying world-class cuisine, and creating unforgettable memories.',
  //       aiRecommendations: 'AI suggests visiting the Eiffel Tower at sunset, taking a Seine river cruise, and exploring Montmartre.',
  //       budget: 5000,
  //       travelStyle: 'Luxury',
  //       groupSize: 'Couple',
  //       isPublic: true,
  //       createdDate: '2024-01-15',
  //       updatedDate: '2024-01-20',
  //       activities: [
  //         { id: 1, name: 'Eiffel Tower Visit', description: 'Sunset visit to the iconic tower', scheduledDate: '2024-06-02', duration: '2 hours', location: 'Champ de Mars', cost: 50, category: 'Sightseeing' },
  //         { id: 2, name: 'Louvre Museum', description: 'Explore world-famous art collection', scheduledDate: '2024-06-03', duration: '4 hours', location: 'Louvre Palace', cost: 30, category: 'Culture' }
  //       ],
  //       accommodations: [
  //         { id: 1, name: 'Hotel Ritz Paris', description: 'Luxury hotel in Place Vendôme', address: '15 Place Vendôme, 75001 Paris', checkInDate: '2024-06-01', checkOutDate: '2024-06-08', costPerNight: 800, type: 'Hotel' }
  //       ],
  //       transportations: [
  //         { id: 1, type: 'Flight', provider: 'Air France', fromLocation: 'New York', toLocation: 'Paris', departureTime: '2024-06-01T14:00:00', arrivalTime: '2024-06-01T22:00:00', cost: 1200, notes: 'Business class' }
  //       ]
  //     },
  //     {
  //       id: 2,
  //       destination: 'Tokyo, Japan',
  //       title: 'Tokyo Adventure',
  //       startDate: '2024-07-15',
  //       endDate: '2024-07-22',
  //       description: 'Experience the perfect blend of traditional culture and modern innovation in Japan\'s vibrant capital city.',
  //       aiRecommendations: 'AI recommends visiting during cherry blossom season, trying authentic ramen, and exploring the digital art museum.',
  //       budget: 3500,
  //       travelStyle: 'Adventure',
  //       groupSize: 'Solo',
  //       isPublic: true,
  //       createdDate: '2024-02-01',
  //       updatedDate: '2024-02-05',
  //       activities: [
  //         { id: 3, name: 'Senso-ji Temple', description: 'Visit Tokyo\'s oldest temple', scheduledDate: '2024-07-16', duration: '2 hours', location: 'Asakusa', cost: 0, category: 'Culture' },
  //         { id: 4, name: 'TeamLab Borderless', description: 'Digital art museum experience', scheduledDate: '2024-07-18', duration: '3 hours', location: 'Odaiba', cost: 45, category: 'Art' }
  //       ],
  //       accommodations: [
  //         { id: 2, name: 'Shibuya Sky Hotel', description: 'Modern hotel with city views', address: '2-1-1 Shibuya, Shibuya City', checkInDate: '2024-07-15', checkOutDate: '2024-07-22', costPerNight: 150, type: 'Hotel' }
  //       ],
  //       transportations: [
  //         { id: 2, type: 'Flight', provider: 'Japan Airlines', fromLocation: 'Los Angeles', toLocation: 'Tokyo', departureTime: '2024-07-15T10:00:00', arrivalTime: '2024-07-16T14:00:00', cost: 900, notes: 'Economy class' }
  //       ]
  //     },
  //     {
  //       id: 3,
  //       destination: 'Bali, Indonesia',
  //       title: 'Family Beach Paradise',
  //       startDate: '2024-08-10',
  //       endDate: '2024-08-17',
  //       description: 'A perfect family vacation with beautiful beaches, cultural experiences, and activities for all ages.',
  //       aiRecommendations: 'AI suggests visiting Ubud for cultural experiences, enjoying water sports in Sanur, and taking a family cooking class.',
  //       budget: 2800,
  //       travelStyle: 'Family',
  //       groupSize: 'Family',
  //       isPublic: false,
  //       createdDate: '2024-03-10',
  //       updatedDate: '2024-03-15',
  //       activities: [
  //         { id: 5, name: 'Ubud Monkey Forest', description: 'Explore the sacred monkey forest', scheduledDate: '2024-08-12', duration: '2 hours', location: 'Ubud', cost: 15, category: 'Nature' },
  //         { id: 6, name: 'Bali Cooking Class', description: 'Learn to cook traditional Balinese dishes', scheduledDate: '2024-08-13', duration: '4 hours', location: 'Ubud', cost: 60, category: 'Culture' }
  //       ],
  //       accommodations: [
  //         { id: 3, name: 'Sanur Beach Resort', description: 'Family-friendly beachfront resort', address: 'Jalan Danau Tamblingan, Sanur', checkInDate: '2024-08-10', checkOutDate: '2024-08-17', costPerNight: 120, type: 'Resort' }
  //       ],
  //       transportations: [
  //         { id: 3, type: 'Flight', provider: 'Garuda Indonesia', fromLocation: 'Sydney', toLocation: 'Denpasar', departureTime: '2024-08-10T08:00:00', arrivalTime: '2024-08-10T14:00:00', cost: 600, notes: 'Economy class' }
  //       ]
  //     }
  //   ];
  //   this.applyFilters();
  // }

  private setupFilterSubscription(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.applyFilters();
      });
  }

  private applyFilters(): void {
    const filters = this.filterForm.value;
    let filtered = [...this.travelPlans];

    // Search filter
    if (filters.search) {
      const searchTerm = filters.search.toLowerCase();
      filtered = filtered.filter(plan =>
        plan.title.toLowerCase().includes(searchTerm) ||
        plan.destination.toLowerCase().includes(searchTerm) ||
        plan.description?.toLowerCase().includes(searchTerm)
      );
    }

    // Travel style filter
    if (filters.travelStyle) {
      filtered = filtered.filter(plan => plan.travelStyle === filters.travelStyle);
    }

    // Group size filter
    if (filters.groupSize) {
      filtered = filtered.filter(plan => plan.groupSize === filters.groupSize);
    }

    // Date range filter
    if (filters.startDate) {
      filtered = filtered.filter(plan => new Date(plan.startDate) >= new Date(filters.startDate));
    }
    if (filters.endDate) {
      filtered = filtered.filter(plan => new Date(plan.endDate) <= new Date(filters.endDate));
    }

    this.filteredPlans = filtered;
  }

  onTogglePublicOnly(): void {
    this.showPublicOnly = !this.showPublicOnly;
    this.loadTravelPlans();
  }

  onClearFilters(): void {
    this.filterForm.reset();
  }

  onViewDetails(plan: TravelPlan): void {
    this.router.navigate(['/travel/details', plan.id]);
  }

  onEditPlan(plan: TravelPlan): void {
    // TODO: Navigate to edit form
    this.snackBar.open(`Editing: ${plan.title}`, 'Close', { duration: 2000 });
  }

  onDeletePlan(plan: TravelPlan): void {
    // TODO: Show confirmation dialog
    this.snackBar.open(`Deleting: ${plan.title}`, 'Close', { duration: 2000 });
  }

  onSharePlan(plan: TravelPlan): void {
    // TODO: Implement sharing functionality
    this.snackBar.open(`Sharing: ${plan.title}`, 'Close', { duration: 2000 });
  }

  onCreateNewPlan(): void {
    this.router.navigate(['/create-plan']); 
  }

  onRefresh(): void {
    this.loadTravelPlans();
  }

  navigateToDemo(): void {
    this.router.navigate(['/travel/demo']);
  }

  get hasActiveFilters(): boolean {
    const filters = this.filterForm.value;
    return !!(filters.search || filters.travelStyle || filters.groupSize || filters.startDate || filters.endDate);
  }

  get resultCount(): number {
    return this.filteredPlans.length;
  }
}

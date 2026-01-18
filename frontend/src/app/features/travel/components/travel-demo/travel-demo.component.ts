import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { TravelPlan } from '../../models/travel-plan.model';
import { TripCardComponent } from '../trip-card/trip-card.component';

@Component({
  selector: 'app-travel-demo',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    TripCardComponent
  ],
  templateUrl: './travel-demo.component.html',
  styleUrl: './travel-demo.component.scss'
})
export class TravelDemoComponent {
  sampleTravelPlans: TravelPlan[] = [
    {
      id: 1,
      destination: 'Paris, France',
      title: 'Romantic Paris Getaway',
      startDate: '2024-06-01',
      endDate: '2024-06-08',
      description: 'A magical week in the City of Light, exploring iconic landmarks, enjoying world-class cuisine, and creating unforgettable memories with your loved one.',
      aiRecommendations: 'AI suggests visiting the Eiffel Tower at sunset, taking a Seine river cruise, and exploring Montmartre for authentic Parisian charm.',
      budget: 5000,
      travelStyle: 'Luxury',
      groupSize: 'Couple',
      isPublic: true,
      createdDate: '2024-01-15',
      updatedDate: '2024-01-20',
      activities: [
        { id: 1, name: 'Eiffel Tower Visit', description: 'Sunset visit to the iconic tower', scheduledDate: '2024-06-02', duration: '2 hours', location: 'Champ de Mars', cost: 50, category: 'Sightseeing' },
        { id: 2, name: 'Louvre Museum', description: 'Explore world-famous art collection', scheduledDate: '2024-06-03', duration: '4 hours', location: 'Louvre Palace', cost: 30, category: 'Culture' },
        { id: 3, name: 'Seine River Cruise', description: 'Romantic dinner cruise', scheduledDate: '2024-06-04', duration: '3 hours', location: 'Seine River', cost: 120, category: 'Dining' }
      ],
      accommodations: [
        { id: 1, name: 'Hotel Ritz Paris', description: 'Luxury hotel in Place Vendôme', address: '15 Place Vendôme, 75001 Paris', checkInDate: '2024-06-01', checkOutDate: '2024-06-08', costPerNight: 800, type: 'Hotel' }
      ],
      transportations: [
        { id: 1, type: 'Flight', provider: 'Air France', fromLocation: 'New York', toLocation: 'Paris', departureTime: '2024-06-01T14:00:00', arrivalTime: '2024-06-01T22:00:00', cost: 1200, notes: 'Business class' }
      ]
    },
    {
      id: 2,
      destination: 'Tokyo, Japan',
      title: 'Tokyo Adventure',
      startDate: '2024-07-15',
      endDate: '2024-07-22',
      description: 'Experience the perfect blend of traditional culture and modern innovation in Japan\'s vibrant capital city.',
      aiRecommendations: 'AI recommends visiting during cherry blossom season, trying authentic ramen, and exploring the digital art museum.',
      budget: 3500,
      travelStyle: 'Adventure',
      groupSize: 'Solo',
      isPublic: true,
      createdDate: '2024-02-01',
      updatedDate: '2024-02-05',
      activities: [
        { id: 4, name: 'Senso-ji Temple', description: 'Visit Tokyo\'s oldest temple', scheduledDate: '2024-07-16', duration: '2 hours', location: 'Asakusa', cost: 0, category: 'Culture' },
        { id: 5, name: 'Shibuya Crossing', description: 'Experience the world\'s busiest intersection', scheduledDate: '2024-07-17', duration: '1 hour', location: 'Shibuya', cost: 0, category: 'Sightseeing' },
        { id: 6, name: 'TeamLab Borderless', description: 'Digital art museum experience', scheduledDate: '2024-07-18', duration: '3 hours', location: 'Odaiba', cost: 45, category: 'Art' }
      ],
      accommodations: [
        { id: 2, name: 'Shibuya Sky Hotel', description: 'Modern hotel with city views', address: '2-1-1 Shibuya, Shibuya City', checkInDate: '2024-07-15', checkOutDate: '2024-07-22', costPerNight: 150, type: 'Hotel' }
      ],
      transportations: [
        { id: 2, type: 'Flight', provider: 'Japan Airlines', fromLocation: 'Los Angeles', toLocation: 'Tokyo', departureTime: '2024-07-15T10:00:00', arrivalTime: '2024-07-16T14:00:00', cost: 900, notes: 'Economy class' }
      ]
    },
    {
      id: 3,
      destination: 'Bali, Indonesia',
      title: 'Family Beach Paradise',
      startDate: '2024-08-10',
      endDate: '2024-08-17',
      description: 'A perfect family vacation with beautiful beaches, cultural experiences, and activities for all ages.',
      aiRecommendations: 'AI suggests visiting Ubud for cultural experiences, enjoying water sports in Sanur, and taking a family cooking class.',
      budget: 2800,
      travelStyle: 'Family',
      groupSize: 'Family',
      isPublic: false,
      createdDate: '2024-03-10',
      updatedDate: '2024-03-15',
      activities: [
        { id: 7, name: 'Ubud Monkey Forest', description: 'Explore the sacred monkey forest', scheduledDate: '2024-08-12', duration: '2 hours', location: 'Ubud', cost: 15, category: 'Nature' },
        { id: 8, name: 'Bali Cooking Class', description: 'Learn to cook traditional Balinese dishes', scheduledDate: '2024-08-13', duration: '4 hours', location: 'Ubud', cost: 60, category: 'Culture' },
        { id: 9, name: 'Water Sports Day', description: 'Snorkeling and water activities', scheduledDate: '2024-08-14', duration: '6 hours', location: 'Sanur', cost: 80, category: 'Adventure' }
      ],
      accommodations: [
        { id: 3, name: 'Sanur Beach Resort', description: 'Family-friendly beachfront resort', address: 'Jalan Danau Tamblingan, Sanur', checkInDate: '2024-08-10', checkOutDate: '2024-08-17', costPerNight: 120, type: 'Resort' }
      ],
      transportations: [
        { id: 3, type: 'Flight', provider: 'Garuda Indonesia', fromLocation: 'Sydney', toLocation: 'Denpasar', departureTime: '2024-08-10T08:00:00', arrivalTime: '2024-08-10T14:00:00', cost: 600, notes: 'Economy class' }
      ]
    },
    {
      id: 4,
      destination: 'New York City, USA',
      title: 'Budget NYC Explorer',
      startDate: '2024-09-05',
      endDate: '2024-09-12',
      description: 'Explore the Big Apple on a budget with free attractions, affordable eats, and smart transportation choices.',
      aiRecommendations: 'AI recommends visiting Central Park, taking the Staten Island Ferry for free Statue of Liberty views, and exploring Brooklyn Bridge.',
      budget: 1200,
      travelStyle: 'Budget',
      groupSize: 'Solo',
      isPublic: true,
      createdDate: '2024-04-01',
      updatedDate: '2024-04-03',
      activities: [
        { id: 10, name: 'Central Park Walk', description: 'Explore the famous park', scheduledDate: '2024-09-06', duration: '3 hours', location: 'Central Park', cost: 0, category: 'Nature' },
        { id: 11, name: 'Brooklyn Bridge', description: 'Walk across the iconic bridge', scheduledDate: '2024-09-07', duration: '2 hours', location: 'Brooklyn Bridge', cost: 0, category: 'Sightseeing' },
        { id: 12, name: 'Times Square', description: 'Experience the heart of NYC', scheduledDate: '2024-09-08', duration: '1 hour', location: 'Times Square', cost: 0, category: 'Sightseeing' }
      ],
      accommodations: [
        { id: 4, name: 'NYC Hostel', description: 'Budget-friendly hostel in Manhattan', address: '123 42nd Street, New York', checkInDate: '2024-09-05', checkOutDate: '2024-09-12', costPerNight: 45, type: 'Hostel' }
      ],
      transportations: [
        { id: 4, type: 'Flight', provider: 'JetBlue', fromLocation: 'Boston', toLocation: 'New York', departureTime: '2024-09-05T12:00:00', arrivalTime: '2024-09-05T13:30:00', cost: 150, notes: 'Economy class' }
      ]
    }
  ];

  constructor(
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  onViewDetails(plan: TravelPlan): void {
    this.router.navigate(['/travel/details', plan.id]);
  }

  onEditPlan(plan: TravelPlan): void {
    this.snackBar.open(`Editing: ${plan.title}`, 'Close', { duration: 2000 });
  }

  onDeletePlan(plan: TravelPlan): void {
    this.snackBar.open(`Deleting: ${plan.title}`, 'Close', { duration: 2000 });
  }

  onSharePlan(plan: TravelPlan): void {
    this.snackBar.open(`Sharing: ${plan.title}`, 'Close', { duration: 2000 });
  }
}

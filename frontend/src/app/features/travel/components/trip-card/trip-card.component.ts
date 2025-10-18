import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TravelPlan } from '../../models/travel-plan.model';

@Component({
  selector: 'app-trip-card',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltipModule
  ],
  templateUrl: './trip-card.component.html',
  styleUrl: './trip-card.component.scss'
})
export class TripCardComponent {
  @Input() travelPlan!: TravelPlan;
  @Input() showActions: boolean = true;
  @Output() viewDetails = new EventEmitter<TravelPlan>();
  @Output() editPlan = new EventEmitter<TravelPlan>();
  @Output() deletePlan = new EventEmitter<TravelPlan>();
  @Output() sharePlan = new EventEmitter<TravelPlan>();

  get duration(): number {
    const start = new Date(this.travelPlan.startDate);
    const end = new Date(this.travelPlan.endDate);
    return Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
  }

  get formattedDateRange(): string {
    const start = new Date(this.travelPlan.startDate);
    const end = new Date(this.travelPlan.endDate);
    return `${start.toLocaleDateString()} - ${end.toLocaleDateString()}`;
  }

  get budgetDisplay(): string {
    if (!this.travelPlan.budget) return 'Budget not set';
    return `$${this.travelPlan.budget.toLocaleString()}`;
  }

  get travelStyleIcon(): string {
    const style = this.travelPlan.travelStyle?.toLowerCase();
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
    const size = this.travelPlan.groupSize?.toLowerCase();
    switch (size) {
      case 'solo': return 'person';
      case 'couple': return 'favorite';
      case 'family': return 'family_restroom';
      case 'group': return 'group';
      default: return 'people';
    }
  }

  onViewDetails(): void {
    this.viewDetails.emit(this.travelPlan);
  }

  onEditPlan(): void {
    this.editPlan.emit(this.travelPlan);
  }

  onDeletePlan(): void {
    this.deletePlan.emit(this.travelPlan);
  }

  onSharePlan(): void {
    this.sharePlan.emit(this.travelPlan);
  }
}

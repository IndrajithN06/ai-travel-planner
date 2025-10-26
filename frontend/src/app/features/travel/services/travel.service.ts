import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { 
  TravelPlan, 
  CreateTravelPlanRequest, 
  GenerateTravelPlanRequest 
} from '../models/travel-plan.model';

@Injectable({
  providedIn: 'root'
})
export class TravelService {
  private readonly apiUrl = `${environment.apiUrl}/travelplans`;

  constructor(private http: HttpClient) {}

  // Get all travel plans
  getAllTravelPlans(): Observable<TravelPlan[]> {
    return this.http.get<TravelPlan[]>(this.apiUrl);
  }

  // Get public travel plans
  getPublicTravelPlans(): Observable<TravelPlan[]> {
    return this.http.get<TravelPlan[]>(`${this.apiUrl}/public`);
  }

  // Get travel plan by ID
  getTravelPlanById(id: number): Observable<TravelPlan> {
    return this.http.get<TravelPlan>(`${this.apiUrl}/${id}`);
  }

  // Get travel plans by destination
  getTravelPlansByDestination(destination: string): Observable<TravelPlan[]> {
    return this.http.get<TravelPlan[]>(`${this.apiUrl}/destination/${encodeURIComponent(destination)}`);
  }

  // Get travel plans by travel style
  getTravelPlansByStyle(travelStyle: string): Observable<TravelPlan[]> {
    return this.http.get<TravelPlan[]>(`${this.apiUrl}/style/${encodeURIComponent(travelStyle)}`);
  }

  // Get travel plans by date range
  getTravelPlansByDateRange(startDate: string, endDate: string): Observable<TravelPlan[]> {
    const params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);
    
    return this.http.get<TravelPlan[]>(`${this.apiUrl}/date-range`, { params });
  }

  // Create a new travel plan
  createTravelPlan(request: CreateTravelPlanRequest | any): Observable<TravelPlan> {
    return this.http.post<TravelPlan>(`${this.apiUrl}/CreateTravelPlan`, request);
  }

  // Generate AI travel plan
  generateTravelPlan(request: GenerateTravelPlanRequest): Observable<TravelPlan> {
    return this.http.post<TravelPlan>(`${this.apiUrl}/generate`, request);
  }

  // Update travel plan
  updateTravelPlan(id: number, request: Partial<CreateTravelPlanRequest>): Observable<TravelPlan> {
    return this.http.put<TravelPlan>(`${this.apiUrl}/${id}`, request);
  }

  // Delete travel plan
  deleteTravelPlan(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Search travel plans with multiple filters
  searchTravelPlans(filters: {
    destination?: string;
    travelStyle?: string;
    startDate?: string;
    endDate?: string;
    isPublic?: boolean;
  }): Observable<TravelPlan[]> {
    let params = new HttpParams();
    
    if (filters.destination) {
      params = params.set('destination', filters.destination);
    }
    if (filters.travelStyle) {
      params = params.set('travelStyle', filters.travelStyle);
    }
    if (filters.startDate) {
      params = params.set('startDate', filters.startDate);
    }
    if (filters.endDate) {
      params = params.set('endDate', filters.endDate);
    }
    if (filters.isPublic !== undefined) {
      params = params.set('isPublic', filters.isPublic.toString());
    }

    return this.http.get<TravelPlan[]>(`${this.apiUrl}/search`, { params });
  }
}

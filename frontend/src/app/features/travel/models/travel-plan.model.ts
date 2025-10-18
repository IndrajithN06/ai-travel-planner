export interface TravelPlan {
  id: number;
  destination: string;
  title: string;
  startDate: string;
  endDate: string;
  description?: string;
  aiRecommendations?: string;
  budget?: number;
  travelStyle?: string;
  groupSize?: string;
  isPublic: boolean;
  createdDate: string;
  updatedDate?: string;
  activities: Activity[];
  accommodations: Accommodation[];
  transportations: Transportation[];
}

export interface Activity {
  id: number;
  name: string;
  description?: string;
  scheduledDate?: string;
  duration?: string;
  location?: string;
  cost?: number;
  category?: string;
}

export interface Accommodation {
  id: number;
  name: string;
  description?: string;
  address?: string;
  checkInDate: string;
  checkOutDate: string;
  costPerNight?: number;
  type?: string;
}

export interface Transportation {
  id: number;
  type: string;
  provider?: string;
  fromLocation?: string;
  toLocation?: string;
  departureTime?: string;
  arrivalTime?: string;
  cost?: number;
  notes?: string;
}

export interface CreateTravelPlanRequest {
  destination: string;
  title: string;
  startDate: string;
  endDate: string;
  description?: string;
  budget?: number;
  travelStyle?: string;
  groupSize?: string;
  isPublic: boolean;
}

export interface GenerateTravelPlanRequest {
  destination: string;
  startDate: string;
  endDate: string;
  travelStyle?: string;
  groupSize?: string;
  budget?: number;
  preferences?: string;
}

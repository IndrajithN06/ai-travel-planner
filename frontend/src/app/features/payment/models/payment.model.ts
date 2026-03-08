export interface CreatePaymentIntentRequest {
  travelPlanId: number;
  amount: number;
  currency?: string;
  description?: string;
}

export interface PaymentIntentResponse {
  clientSecret: string;
  paymentIntentId: string;
  amount: number;
  currency: string;
}

export interface PaymentResponse {
  id: number;
  travelPlanId: number;
  userId: number;
  stripePaymentIntentId: string;
  stripeCustomerId?: string;
  amount: number;
  currency: string;
  status: string;
  paymentMethod?: string;
  description?: string;
  receiptUrl?: string;
  createdDate: string;
  completedDate?: string;
}

export interface PaymentStatusResponse {
  paymentIntentId: string;
  status: string;
  amount: number;
  currency: string;
  completedDate?: string;
  receiptUrl?: string;
}

export enum PaymentStatus {
  Pending = 'pending',
  Succeeded = 'succeeded',
  Failed = 'failed',
  Canceled = 'canceled',
  Refunded = 'refunded'
}

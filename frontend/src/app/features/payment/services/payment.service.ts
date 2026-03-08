import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, interval } from 'rxjs';
import { switchMap, takeWhile, catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import {
  CreatePaymentIntentRequest,
  PaymentIntentResponse,
  PaymentResponse,
  PaymentStatusResponse
} from '../models/payment.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private readonly apiUrl = `${environment.apiUrl}/payment`;

  constructor(private http: HttpClient) {}

  /**
   * Create a payment intent for a travel plan
   */
  createPaymentIntent(request: CreatePaymentIntentRequest): Observable<PaymentIntentResponse> {
    return this.http.post<PaymentIntentResponse>(`${this.apiUrl}/create-intent`, request);
  }

  /**
   * Get payment status by payment intent ID
   */
  getPaymentStatus(paymentIntentId: string): Observable<PaymentStatusResponse> {
    return this.http.get<PaymentStatusResponse>(`${this.apiUrl}/${paymentIntentId}/status`);
  }

  /**
   * Get payment by ID
   */
  getPaymentById(paymentId: number): Observable<PaymentResponse> {
    return this.http.get<PaymentResponse>(`${this.apiUrl}/${paymentId}`);
  }

  /**
   * Get all payments for the current user
   */
  getMyPayments(): Observable<PaymentResponse[]> {
    return this.http.get<PaymentResponse[]>(`${this.apiUrl}/my-payments`);
  }

  /**
   * Poll payment status until it's no longer pending
   */
  pollPaymentStatus(
    paymentIntentId: string,
    maxAttempts: number = 30,
    intervalMs: number = 2000
  ): Observable<PaymentStatusResponse> {
    return interval(intervalMs).pipe(
      switchMap(() => this.getPaymentStatus(paymentIntentId)),
      takeWhile(
        (status) => status.status === 'pending' || status.status === 'processing',
        true
      ),
      catchError((error) => {
        console.error('Error polling payment status:', error);
        throw error;
      })
    );
  }
}

import { Injectable } from '@angular/core';
import { loadStripe, Stripe, StripeElements, StripeCardElement } from '@stripe/stripe-js';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StripeLoaderService {
  private stripePromise: Promise<Stripe | null> | null = null;
  private stripeInstance: Stripe | null = null;

  constructor() {
    this.initializeStripe();
  }

  /**
   * Initialize Stripe with publishable key
   */
  private async initializeStripe(): Promise<void> {
    const publishableKey = environment.stripePublishableKey;
    
    if (!publishableKey || publishableKey.includes('your_publishable_key')) {
      console.warn('Stripe publishable key not configured. Payment functionality will be limited.');
      return;
    }

    this.stripePromise = loadStripe(publishableKey);
    this.stripeInstance = await this.stripePromise;
  }

  /**
   * Get Stripe instance
   */
  async getStripe(): Promise<Stripe | null> {
    if (!this.stripePromise) {
      await this.initializeStripe();
    }
    return this.stripePromise;
  }

  /**
   * Check if Stripe is available
   */
  isStripeAvailable(): boolean {
    return this.stripeInstance !== null;
  }
}

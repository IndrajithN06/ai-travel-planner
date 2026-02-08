import { Component, OnInit, OnDestroy, Output, EventEmitter, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { PaymentService } from '../../services/payment.service';
import { StripeLoaderService } from '../../services/stripe-loader.service';
import { CreatePaymentIntentRequest, PaymentIntentResponse } from '../../models/payment.model';
import { loadStripe, Stripe, StripeElements, StripeCardElement } from '@stripe/stripe-js';

export interface PaymentFormData {
  travelPlanId: number;
  amount: number;
  currency?: string;
  description?: string;
}

@Component({
  selector: 'app-payment-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './payment-form.component.html',
  styleUrl: './payment-form.component.scss'
})
export class PaymentFormComponent implements OnInit, OnDestroy {
  @Output() paymentSuccess = new EventEmitter<string>();
  @Output() paymentCancel = new EventEmitter<void>();

  travelPlanId!: number;
  amount!: number;
  currency: string = 'USD';
  description?: string;

  paymentForm!: FormGroup;
  loading = false;
  processing = false;
  stripe: Stripe | null = null;
  elements: StripeElements | null = null;
  cardElement: StripeCardElement | null = null;
  cardError: string | null = null;
  clientSecret: string | null = null;

  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private paymentService: PaymentService,
    private stripeLoader: StripeLoaderService,
    private snackBar: MatSnackBar,
    public dialogRef: MatDialogRef<PaymentFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PaymentFormData
  ) {
    // Initialize from dialog data
    this.travelPlanId = data.travelPlanId;
    this.amount = data.amount;
    this.currency = data.currency || 'USD';
    this.description = data.description;

    this.paymentForm = this.fb.group({
      amount: [{ value: this.amount || 0, disabled: true }, [Validators.required, Validators.min(0.01)]]
    });
  }

  async ngOnInit(): Promise<void> {
    await this.initializeStripe();
    this.createPaymentIntent();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Initialize Stripe and create card element
   */
  private async initializeStripe(): Promise<void> {
    try {
      this.stripe = await this.stripeLoader.getStripe();
      
      if (!this.stripe) {
        this.snackBar.open(
          'Stripe is not configured. Please add your Stripe publishable key to environment configuration.',
          'Close',
          { duration: 5000, panelClass: ['error-snackbar'] }
        );
        return;
      }

      this.elements = this.stripe.elements();
      this.cardElement = this.elements.create('card', {
        style: {
          base: {
            fontSize: '16px',
            color: '#424770',
            '::placeholder': {
              color: '#aab7c4',
            },
          },
          invalid: {
            color: '#9e2146',
          },
        },
      });

      // Mount card element - use setTimeout to ensure DOM is ready
      setTimeout(() => {
        const cardElementContainer = document.getElementById('card-element');
        if (cardElementContainer && this.cardElement) {
          this.cardElement.mount('#card-element');
          this.cardElement.on('change', (event) => {
            this.cardError = event.error ? event.error.message : null;
          });
        } else {
          console.error('Card element container not found');
        }
      }, 100);
    } catch (error) {
      console.error('Error initializing Stripe:', error);
      this.snackBar.open('Failed to initialize payment system', 'Close', {
        duration: 5000,
        panelClass: ['error-snackbar']
      });
    }
  }

  /**
   * Create payment intent
   */
  private createPaymentIntent(): void {
    this.loading = true;
    
    const request: CreatePaymentIntentRequest = {
      travelPlanId: this.travelPlanId,
      amount: this.amount,
      currency: this.currency,
      description: this.description
    };

    this.paymentService.createPaymentIntent(request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response: PaymentIntentResponse) => {
          this.clientSecret = response.clientSecret;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error creating payment intent:', error);
          this.snackBar.open(
            error.error?.message || 'Failed to initialize payment. Please try again.',
            'Close',
            { duration: 5000, panelClass: ['error-snackbar'] }
          );
          this.loading = false;
        }
      });
  }

  /**
   * Handle form submission
   */
  async onSubmit(): Promise<void> {
    if (!this.stripe || !this.cardElement || !this.clientSecret) {
      this.snackBar.open('Payment system not ready. Please wait...', 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    if (this.cardError) {
      this.snackBar.open(this.cardError, 'Close', {
        duration: 3000,
        panelClass: ['error-snackbar']
      });
      return;
    }

    this.processing = true;

    try {
      const { error, paymentIntent } = await this.stripe.confirmCardPayment(this.clientSecret, {
        payment_method: {
          card: this.cardElement,
        }
      });

      if (error) {
        this.snackBar.open(error.message || 'Payment failed', 'Close', {
          duration: 5000,
          panelClass: ['error-snackbar']
        });
        this.processing = false;
      } else if (paymentIntent && paymentIntent.status === 'succeeded') {
        this.snackBar.open('Payment successful!', 'Close', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.paymentSuccess.emit(paymentIntent.id);
      }
    } catch (error: any) {
      console.error('Payment error:', error);
      this.snackBar.open(
        error.message || 'An error occurred during payment processing',
        'Close',
        { duration: 5000, panelClass: ['error-snackbar'] }
      );
      this.processing = false;
    }
  }

  /**
   * Cancel payment
   */
  onCancel(): void {
    this.paymentCancel.emit();
    this.dialogRef.close();
  }

  /**
   * Format currency
   */
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: this.currency
    }).format(amount);
  }
}

import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { PaymentService } from '../../services/payment.service';
import { PaymentStatusResponse, PaymentStatus } from '../../models/payment.model';

@Component({
  selector: 'app-payment-status',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './payment-status.component.html',
  styleUrl: './payment-status.component.scss'
})
export class PaymentStatusComponent implements OnInit, OnDestroy {
  @Input() paymentIntentId!: string;

  paymentStatus: PaymentStatusResponse | null = null;
  loading = false;
  error: string | null = null;

  private destroy$ = new Subject<void>();

  constructor(private paymentService: PaymentService) {}

  ngOnInit(): void {
    if (this.paymentIntentId) {
      this.loadPaymentStatus();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Load payment status
   */
  private loadPaymentStatus(): void {
    this.loading = true;
    this.error = null;

    this.paymentService.getPaymentStatus(this.paymentIntentId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (status) => {
          this.paymentStatus = status;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading payment status:', error);
          this.error = 'Failed to load payment status';
          this.loading = false;
        }
      });
  }

  /**
   * Get status icon
   */
  getStatusIcon(): string {
    if (!this.paymentStatus) return 'help_outline';

    switch (this.paymentStatus.status) {
      case PaymentStatus.Succeeded:
        return 'check_circle';
      case PaymentStatus.Failed:
        return 'error';
      case PaymentStatus.Canceled:
        return 'cancel';
      case PaymentStatus.Refunded:
        return 'undo';
      default:
        return 'schedule';
    }
  }

  /**
   * Get status color
   */
  getStatusColor(): string {
    if (!this.paymentStatus) return 'primary';

    switch (this.paymentStatus.status) {
      case PaymentStatus.Succeeded:
        return 'success';
      case PaymentStatus.Failed:
        return 'warn';
      case PaymentStatus.Canceled:
        return 'default';
      case PaymentStatus.Refunded:
        return 'accent';
      default:
        return 'primary';
    }
  }

  /**
   * Get status message
   */
  getStatusMessage(): string {
    if (!this.paymentStatus) return 'Unknown status';

    switch (this.paymentStatus.status) {
      case PaymentStatus.Succeeded:
        return 'Payment Successful';
      case PaymentStatus.Failed:
        return 'Payment Failed';
      case PaymentStatus.Canceled:
        return 'Payment Canceled';
      case PaymentStatus.Refunded:
        return 'Payment Refunded';
      case PaymentStatus.Pending:
        return 'Payment Pending';
      default:
        return 'Processing...';
    }
  }

  /**
   * Format currency
   */
  formatCurrency(amount: number, currency: string = 'USD'): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: currency
    }).format(amount);
  }

  /**
   * Open receipt
   */
  openReceipt(): void {
    if (this.paymentStatus?.receiptUrl) {
      window.open(this.paymentStatus.receiptUrl, '_blank');
    }
  }
}

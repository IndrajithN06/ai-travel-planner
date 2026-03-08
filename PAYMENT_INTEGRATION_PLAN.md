# Payment Integration Plan for AI Travel Planner 💳

## Executive Summary

This document outlines a comprehensive plan for integrating real-time external payment processing into the AI Travel Planner application. The integration will support payment processing for travel plan bookings, activities, accommodations, and transportation services.

---

## 1. Payment Gateway Options

### Recommended: **Stripe** (Primary Choice)
**Why Stripe?**
- ✅ Excellent developer experience and documentation
- ✅ Strong .NET SDK support
- ✅ Real-time webhook support
- ✅ PCI compliance handled by Stripe
- ✅ Supports 135+ currencies
- ✅ Mobile-friendly payment UI
- ✅ Subscription support (for future premium features)
- ✅ 3D Secure support for international payments

**Alternatives:**
- **PayPal**: Good for user familiarity, but less flexible for programmatic control
- **Square**: Good for in-person payments, less ideal for online-only
- **Razorpay**: Good for India/SEA markets
- **Adyen**: Enterprise-grade, more complex setup

---

## 2. Architecture Overview

```
┌─────────────────┐
│  Angular App    │
│  (Frontend)     │
└────────┬────────┘
         │
         │ Payment Intent Creation
         │ Payment Confirmation
         │
┌────────▼────────┐
│  ASP.NET API    │
│  (Backend)      │
│                 │
│  ┌───────────┐  │
│  │ Payment   │  │
│  │ Service   │  │
│  └─────┬─────┘  │
│        │        │
└────────┼────────┘
         │
         │ API Calls
         │ Webhooks
         │
┌────────▼────────┐
│  Stripe API     │
│  (Payment       │
│  Gateway)       │
└─────────────────┘
```

### Payment Flow:
1. **User initiates payment** → Frontend requests payment intent
2. **Backend creates PaymentIntent** → Returns client secret to frontend
3. **Frontend confirms payment** → Uses Stripe.js to process payment
4. **Stripe processes payment** → Sends webhook to backend
5. **Backend updates database** → Marks payment as completed
6. **Frontend receives confirmation** → Updates UI in real-time

---

## 3. Required Libraries & Packages

### Backend (ASP.NET Core 9.0)

#### NuGet Packages:
```xml
<!-- Stripe .NET SDK -->
<PackageReference Include="Stripe.net" Version="45.0.0" />

<!-- For webhook signature verification -->
<PackageReference Include="Microsoft.AspNetCore.Http" Version="2.2.2" />

<!-- For background job processing (optional, for async webhook handling) -->
<PackageReference Include="Hangfire.AspNetCore" Version="1.8.17" />
```

#### Installation Commands:
```bash
cd backend/AITravelPlanner.Api
dotnet add package Stripe.net --version 45.0.0
```

### Frontend (Angular 19)

#### NPM Packages:
```json
{
  "@stripe/stripe-js": "^2.4.0",
  "@stripe/react-stripe-js": "^2.4.0"  // If using React components (not needed for Angular)
}
```

**Note**: For Angular, we'll use `@stripe/stripe-js` directly with Angular's HttpClient. There's no official Angular wrapper, but Stripe.js works perfectly with Angular.

#### Installation Commands:
```bash
cd frontend
npm install @stripe/stripe-js
npm install --save-dev @types/stripe-v3  # TypeScript types (optional)
```

---

## 4. Database Schema Changes

### New Entities Required:

#### 4.1 Payment Entity
```csharp
public class Payment
{
    public int Id { get; set; }
    public int TravelPlanId { get; set; }
    public int UserId { get; set; }
    
    // Stripe-specific fields
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public string StripeCustomerId { get; set; } = string.Empty;
    
    // Payment details
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Status { get; set; } = string.Empty; // pending, succeeded, failed, canceled
    public string PaymentMethod { get; set; } = string.Empty; // card, paypal, etc.
    
    // Metadata
    public string? Description { get; set; }
    public string? ReceiptUrl { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedDate { get; set; }
    
    // Navigation properties
    public virtual TravelPlan TravelPlan { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
```

#### 4.2 PaymentTransaction Entity (for audit trail)
```csharp
public class PaymentTransaction
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public string EventType { get; set; } = string.Empty; // payment_intent.created, payment_intent.succeeded, etc.
    public string StripeEventId { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? RawData { get; set; } // JSON payload from Stripe
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public virtual Payment Payment { get; set; } = null!;
}
```

#### 4.3 Update User Entity
Add Stripe customer ID for future payments:
```csharp
public string? StripeCustomerId { get; set; }
```

---

## 5. Backend Implementation Plan

### 5.1 Project Structure
```
AITravelPlanner.Domain/
  ├── Entities/
  │   ├── Payment.cs (NEW)
  │   ├── PaymentTransaction.cs (NEW)
  │   └── User.cs (UPDATE - add StripeCustomerId)
  ├── DTOs/
  │   ├── PaymentIntentRequestDto.cs (NEW)
  │   ├── PaymentIntentResponseDto.cs (NEW)
  │   └── PaymentDto.cs (NEW)
  └── Interfaces/
      └── IPaymentService.cs (NEW)

AITravelPlanner.Application/
  └── Services/
      └── PaymentService.cs (NEW)

AITravelPlanner.Infrastructure/
  ├── Data/
  │   └── Configurations/
  │       ├── PaymentConfiguration.cs (NEW)
  │       └── PaymentTransactionConfiguration.cs (NEW)
  └── Repositories/
      └── PaymentRepository.cs (NEW)

AITravelPlanner.Api/
  ├── Controllers/
  │   └── PaymentController.cs (NEW)
  └── appsettings.json (UPDATE - add Stripe keys)
```

### 5.2 Configuration (appsettings.json)
```json
{
  "Stripe": {
    "PublishableKey": "pk_test_...",
    "SecretKey": "sk_test_...",
    "WebhookSecret": "whsec_...",
    "Currency": "USD"
  }
}
```

### 5.3 Key Services to Implement

#### PaymentService.cs
**Responsibilities:**
- Create PaymentIntent
- Confirm payment
- Handle webhooks
- Retrieve payment status
- Create/retrieve Stripe customers

**Key Methods:**
```csharp
Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(PaymentIntentRequestDto request);
Task<bool> ConfirmPaymentAsync(string paymentIntentId);
Task<bool> HandleWebhookAsync(string payload, string signature);
Task<PaymentDto> GetPaymentStatusAsync(string paymentIntentId);
Task<string> GetOrCreateStripeCustomerAsync(int userId);
```

#### PaymentController.cs
**Endpoints:**
- `POST /api/payments/create-intent` - Create payment intent
- `POST /api/payments/confirm` - Confirm payment
- `POST /api/payments/webhook` - Stripe webhook endpoint
- `GET /api/payments/{paymentIntentId}/status` - Get payment status
- `GET /api/payments/user/{userId}` - Get user's payment history

### 5.4 Webhook Handling

**Critical Security Considerations:**
1. Verify webhook signature using Stripe's secret
2. Idempotency: Check if event already processed
3. Handle async processing for reliability
4. Log all webhook events for debugging

**Webhook Events to Handle:**
- `payment_intent.succeeded` - Payment completed
- `payment_intent.payment_failed` - Payment failed
- `payment_intent.canceled` - Payment canceled
- `charge.refunded` - Refund processed

---

## 6. Frontend Implementation Plan

### 6.1 Project Structure
```
frontend/src/app/
  ├── features/
  │   └── payment/
  │       ├── components/
  │       │   ├── payment-form/
  │       │   │   ├── payment-form.component.ts
  │       │   │   ├── payment-form.component.html
  │       │   │   └── payment-form.component.scss
  │       │   └── payment-status/
  │       │       └── payment-status.component.ts
  │       ├── services/
  │       │   └── payment.service.ts
  │       ├── models/
  │       │   └── payment.model.ts
  │       └── payment-routing.module.ts
  └── core/
      └── services/
          └── stripe-loader.service.ts (NEW - loads Stripe.js)
```

### 6.2 Key Components

#### PaymentService (Angular)
```typescript
@Injectable({ providedIn: 'root' })
export class PaymentService {
  createPaymentIntent(travelPlanId: number, amount: number): Observable<PaymentIntentResponse>
  confirmPayment(paymentIntentId: string, paymentMethodId: string): Observable<PaymentResult>
  getPaymentStatus(paymentIntentId: string): Observable<PaymentStatus>
}
```

#### PaymentFormComponent
**Features:**
- Load Stripe.js dynamically
- Display payment form using Stripe Elements
- Handle card input securely
- Show payment status in real-time
- Error handling and validation

#### StripeLoaderService
**Purpose:**
- Load Stripe.js library dynamically
- Initialize Stripe instance
- Provide Stripe instance to components

### 6.3 Payment Flow in Frontend

1. **User clicks "Pay Now"** on travel plan
2. **Component calls PaymentService.createPaymentIntent()**
3. **Backend returns client secret**
4. **Component loads Stripe.js and initializes**
5. **User enters card details** (handled by Stripe Elements)
6. **Component calls Stripe.confirmCardPayment()**
7. **Real-time status updates** via polling or SignalR (optional)
8. **Show success/failure message**

---

## 7. Real-Time Payment Status Updates

### Option 1: **Polling** (Simplest)
- Frontend polls payment status every 2-3 seconds
- Stop polling when payment succeeds/fails
- Easy to implement, but less efficient

### Option 2: **SignalR** (Recommended for real-time)
- Backend sends webhook → Updates database → Pushes to SignalR hub
- Frontend receives real-time updates
- More efficient and provides true real-time experience

### Option 3: **WebSocket** (Advanced)
- Custom WebSocket implementation
- More control but more complex

**Recommended: Start with Polling, upgrade to SignalR later**

---

## 8. Security Considerations

### 8.1 PCI Compliance
- ✅ **Never store card details** - Stripe handles this
- ✅ **Use Stripe Elements** - Card input never touches your server
- ✅ **HTTPS only** - Required for production
- ✅ **Webhook signature verification** - Critical for security

### 8.2 API Security
- ✅ **JWT Authentication** - Already implemented
- ✅ **Rate limiting** - Prevent abuse
- ✅ **Input validation** - Validate all payment requests
- ✅ **Idempotency keys** - Prevent duplicate payments

### 8.3 Environment Variables
- ✅ **Never commit API keys** - Use environment variables
- ✅ **Separate test/live keys** - Use test keys in development
- ✅ **Rotate keys regularly** - Security best practice

---

## 9. Testing Strategy

### 9.1 Test Cards (Stripe Test Mode)
```
Success: 4242 4242 4242 4242
Decline: 4000 0000 0000 0002
3D Secure: 4000 0025 0000 3155
```

### 9.2 Test Scenarios
- ✅ Successful payment
- ✅ Failed payment (insufficient funds)
- ✅ 3D Secure authentication
- ✅ Payment cancellation
- ✅ Refund processing
- ✅ Webhook handling
- ✅ Network failures

### 9.3 Testing Tools
- **Stripe CLI** - Test webhooks locally
- **Stripe Dashboard** - Monitor test payments
- **Postman/Thunder Client** - API testing

---

## 10. Step-by-Step Implementation Guide

### Phase 1: Backend Setup (Week 1)
1. ✅ Install Stripe.NET package
2. ✅ Create Payment entity
3. ✅ Create PaymentTransaction entity
4. ✅ Update User entity (add StripeCustomerId)
5. ✅ Create database migrations
6. ✅ Add Stripe configuration to appsettings.json
7. ✅ Create IPaymentService interface
8. ✅ Implement PaymentService
9. ✅ Create PaymentController
10. ✅ Add webhook endpoint

### Phase 2: Frontend Setup (Week 1-2)
1. ✅ Install @stripe/stripe-js
2. ✅ Create payment models
3. ✅ Create PaymentService
4. ✅ Create StripeLoaderService
5. ✅ Create PaymentFormComponent
6. ✅ Integrate payment form into travel plan details
7. ✅ Add payment status display

### Phase 3: Integration & Testing (Week 2)
1. ✅ Connect frontend to backend
2. ✅ Test payment flow end-to-end
3. ✅ Implement webhook handling
4. ✅ Add error handling
5. ✅ Test with Stripe test cards
6. ✅ Implement payment status polling

### Phase 4: Real-Time Updates (Week 3 - Optional)
1. ✅ Install SignalR packages
2. ✅ Create PaymentHub
3. ✅ Update webhook handler to push to SignalR
4. ✅ Update frontend to receive SignalR updates
5. ✅ Replace polling with SignalR

### Phase 5: Production Readiness (Week 3-4)
1. ✅ Switch to live Stripe keys
2. ✅ Add comprehensive error handling
3. ✅ Add logging and monitoring
4. ✅ Security audit
5. ✅ Performance testing
6. ✅ Documentation

---

## 11. Additional Features (Future Enhancements)

### 11.1 Payment Methods
- Save payment methods for future use
- Multiple payment methods (card, bank transfer, etc.)
- Payment method management UI

### 11.2 Subscriptions
- Premium plan subscriptions
- Recurring payments
- Subscription management

### 11.3 Refunds
- Partial refunds
- Full refunds
- Refund history

### 11.4 Multi-Currency
- Currency conversion
- Display prices in user's currency
- Handle currency-specific payment methods

### 11.5 Payment Analytics
- Payment dashboard
- Revenue reports
- Payment success rate tracking

---

## 12. Cost Considerations

### Stripe Pricing (as of 2024)
- **2.9% + $0.30** per successful card charge
- **No monthly fees**
- **No setup fees**
- **Free for first $1M in revenue** (Stripe Atlas)

### Additional Costs
- **SignalR hosting** (if using Azure SignalR Service)
- **Database storage** (minimal for payment records)
- **SSL certificate** (usually free with hosting)

---

## 13. Required API Keys & Setup

### Stripe Account Setup
1. **Create Stripe Account**: https://stripe.com
2. **Get API Keys**:
   - Test keys: Available immediately
   - Live keys: After account verification
3. **Configure Webhooks**:
   - Add webhook endpoint URL
   - Select events to listen to
   - Get webhook signing secret

### Environment Configuration
```bash
# Development (.env or appsettings.Development.json)
STRIPE_PUBLISHABLE_KEY=pk_test_...
STRIPE_SECRET_KEY=sk_test_...
STRIPE_WEBHOOK_SECRET=whsec_...

# Production (Environment Variables)
STRIPE_PUBLISHABLE_KEY=pk_live_...
STRIPE_SECRET_KEY=sk_live_...
STRIPE_WEBHOOK_SECRET=whsec_...
```

---

## 14. Monitoring & Logging

### What to Monitor
- Payment success rate
- Failed payment reasons
- Webhook delivery status
- Average payment processing time
- Refund rate

### Logging Strategy
- Log all payment attempts
- Log webhook events
- Log errors with full context
- Use structured logging (Serilog recommended)

---

## 15. Compliance & Legal

### Required Disclosures
- Terms of Service
- Privacy Policy
- Refund Policy
- Payment processing disclosure

### Regional Considerations
- **GDPR** (EU): Handle customer data properly
- **PCI DSS**: Stripe handles most requirements
- **Tax compliance**: May need to handle tax calculation
- **Regional payment methods**: Consider local payment methods

---

## 16. Resources & Documentation

### Official Documentation
- **Stripe .NET SDK**: https://stripe.com/docs/api/dotnet
- **Stripe.js**: https://stripe.com/docs/js
- **Stripe Webhooks**: https://stripe.com/docs/webhooks
- **Stripe Testing**: https://stripe.com/docs/testing

### Helpful Tools
- **Stripe CLI**: https://stripe.com/docs/stripe-cli
- **Stripe Dashboard**: https://dashboard.stripe.com
- **Stripe Testing Cards**: https://stripe.com/docs/testing

---

## 17. Quick Start Checklist

### Immediate Next Steps:
- [ ] Create Stripe account
- [ ] Get test API keys
- [ ] Install Stripe.NET package (backend)
- [ ] Install @stripe/stripe-js (frontend)
- [ ] Create Payment entity
- [ ] Create PaymentService
- [ ] Create PaymentController
- [ ] Create payment form component
- [ ] Test with Stripe test cards
- [ ] Set up webhook endpoint
- [ ] Test webhook handling

---

## Summary

This plan provides a comprehensive roadmap for integrating Stripe payment processing into your AI Travel Planner application. The implementation follows best practices for security, scalability, and user experience.

**Recommended Approach:**
1. Start with **Stripe** as the payment gateway
2. Implement **basic payment flow** first (Phase 1-3)
3. Add **real-time updates** later (Phase 4)
4. Enhance with **advanced features** as needed (Phase 5)

**Estimated Timeline:** 2-3 weeks for full implementation

**Key Success Factors:**
- Proper webhook handling
- Secure payment processing
- Good error handling
- Real-time status updates
- Comprehensive testing

---

**Questions or need clarification?** Review the Stripe documentation or consult with the development team.

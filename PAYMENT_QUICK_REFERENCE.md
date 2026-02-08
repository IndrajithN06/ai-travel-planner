# Payment Integration - Quick Reference Guide 🚀

## Required Libraries & APIs

### Backend (ASP.NET Core 9.0)

#### NuGet Package
```bash
dotnet add package Stripe.net --version 45.0.0
```

**Package Details:**
- **Name**: Stripe.net
- **Version**: 45.0.0 (latest stable)
- **NuGet**: https://www.nuget.org/packages/Stripe.net/
- **Purpose**: Official Stripe .NET SDK for server-side payment processing

#### Optional Packages (for advanced features)
```bash
# For background job processing (webhook handling)
dotnet add package Hangfire.AspNetCore --version 1.8.17

# For SignalR (real-time updates)
dotnet add package Microsoft.AspNetCore.SignalR --version 9.0.0
```

---

### Frontend (Angular 19)

#### NPM Package
```bash
npm install @stripe/stripe-js
```

**Package Details:**
- **Name**: @stripe/stripe-js
- **Version**: ^2.4.0 (latest)
- **NPM**: https://www.npmjs.com/package/@stripe/stripe-js
- **Purpose**: Stripe.js library for client-side payment processing

#### TypeScript Types (Optional but Recommended)
```bash
npm install --save-dev @types/stripe-v3
```

---

## Required APIs & Endpoints

### Stripe API Endpoints (Used by SDK)

#### Payment Intents API
- **Create Payment Intent**: `POST https://api.stripe.com/v1/payment_intents`
- **Retrieve Payment Intent**: `GET https://api.stripe.com/v1/payment_intents/{id}`
- **Confirm Payment Intent**: `POST https://api.stripe.com/v1/payment_intents/{id}/confirm`
- **Cancel Payment Intent**: `POST https://api.stripe.com/v1/payment_intents/{id}/cancel`

#### Customers API
- **Create Customer**: `POST https://api.stripe.com/v1/customers`
- **Retrieve Customer**: `GET https://api.stripe.com/v1/customers/{id}`

#### Webhooks API
- **Webhook Events**: `POST https://your-api.com/api/payments/webhook`
- **Event Types**: `payment_intent.succeeded`, `payment_intent.payment_failed`, etc.

---

### Your Backend API Endpoints (To Create)

#### Payment Controller Endpoints
```
POST   /api/payments/create-intent     - Create payment intent
POST   /api/payments/confirm            - Confirm payment
POST   /api/payments/webhook            - Stripe webhook endpoint
GET    /api/payments/{id}/status         - Get payment status
GET    /api/payments/user/{userId}       - Get user payment history
POST   /api/payments/refund              - Process refund (future)
```

---

## API Keys Required

### Stripe API Keys

#### Test Mode (Development)
```
Publishable Key: pk_test_...
Secret Key: sk_test_...
Webhook Secret: whsec_...
```

#### Live Mode (Production)
```
Publishable Key: pk_live_...
Secret Key: sk_live_...
Webhook Secret: whsec_...
```

**Where to Get:**
1. Sign up at https://stripe.com
2. Go to Developers → API Keys
3. Copy test keys (available immediately)
4. Request live keys (after account verification)

---

## Code Examples

### Backend: Create Payment Intent
```csharp
using Stripe;

var options = new PaymentIntentCreateOptions
{
    Amount = (long)(amount * 100), // Convert to cents
    Currency = "usd",
    Customer = customerId,
    Metadata = new Dictionary<string, string>
    {
        { "travel_plan_id", travelPlanId.ToString() },
        { "user_id", userId.ToString() }
    }
};

var service = new PaymentIntentService();
var paymentIntent = service.Create(options);
return paymentIntent.ClientSecret;
```

### Frontend: Initialize Stripe
```typescript
import { loadStripe } from '@stripe/stripe-js';

const stripe = await loadStripe('pk_test_...');
const { error, paymentIntent } = await stripe.confirmCardPayment(
  clientSecret,
  {
    payment_method: {
      card: cardElement,
      billing_details: {
        name: 'Customer Name',
      },
    },
  }
);
```

---

## Webhook Events to Handle

| Event Type | Description | Action |
|------------|-------------|--------|
| `payment_intent.succeeded` | Payment completed successfully | Update payment status to "succeeded" |
| `payment_intent.payment_failed` | Payment failed | Update payment status to "failed" |
| `payment_intent.canceled` | Payment canceled | Update payment status to "canceled" |
| `charge.refunded` | Refund processed | Update payment status to "refunded" |

---

## Testing Cards

| Card Number | Scenario | CVC | Expiry |
|-------------|----------|-----|--------|
| 4242 4242 4242 4242 | Success | Any 3 digits | Any future date |
| 4000 0000 0000 0002 | Card declined | Any 3 digits | Any future date |
| 4000 0025 0000 3155 | 3D Secure required | Any 3 digits | Any future date |
| 4000 0000 0000 9995 | Insufficient funds | Any 3 digits | Any future date |

**More test cards**: https://stripe.com/docs/testing

---

## Configuration Files

### Backend: appsettings.json
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

### Frontend: environment.ts
```typescript
export const environment = {
  production: false,
  stripePublishableKey: 'pk_test_...',
  apiUrl: 'http://localhost:5015'
};
```

---

## Installation Commands Summary

### Backend
```bash
cd backend/AITravelPlanner.Api
dotnet add package Stripe.net --version 45.0.0
```

### Frontend
```bash
cd frontend
npm install @stripe/stripe-js
npm install --save-dev @types/stripe-v3
```

---

## Key Documentation Links

- **Stripe .NET SDK**: https://stripe.com/docs/api/dotnet
- **Stripe.js Reference**: https://stripe.com/docs/js
- **Payment Intents API**: https://stripe.com/docs/api/payment_intents
- **Webhooks Guide**: https://stripe.com/docs/webhooks
- **Testing Guide**: https://stripe.com/docs/testing
- **Stripe Dashboard**: https://dashboard.stripe.com

---

## Support & Resources

- **Stripe Support**: https://support.stripe.com
- **Stripe Community**: https://github.com/stripe/stripe-dotnet
- **Stack Overflow**: Tag `stripe-payments` or `stripe.net`

---

**Next Steps**: See `PAYMENT_INTEGRATION_PLAN.md` for detailed implementation guide.

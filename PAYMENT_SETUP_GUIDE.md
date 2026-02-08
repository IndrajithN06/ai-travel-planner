# Payment Integration Setup Guide 🚀

## Overview

The payment integration is now complete! This guide will help you set up Stripe and start processing payments.

---

## ✅ What's Been Implemented

### Backend (ASP.NET Core)
- ✅ Payment entities (Payment, PaymentTransaction)
- ✅ Payment service with Stripe integration
- ✅ Payment repository
- ✅ Payment controller with all endpoints
- ✅ Webhook handling for real-time payment updates
- ✅ Database schema ready

### Frontend (Angular)
- ✅ Payment models and interfaces
- ✅ Payment service for API communication
- ✅ Stripe loader service
- ✅ Payment form component (beautiful UI)
- ✅ Payment status component
- ✅ Integrated into travel plan details page

---

## 🔧 Setup Instructions

### Step 1: Get Stripe API Keys

1. **Create a Stripe Account**
   - Go to https://stripe.com
   - Sign up for a free account
   - Complete account verification

2. **Get Your API Keys**
   - Navigate to: Developers → API Keys
   - Copy your **Test** keys (for development):
     - Publishable Key: `pk_test_...`
     - Secret Key: `sk_test_...`
   - For production, use **Live** keys after account verification

3. **Set Up Webhooks** (Important!)
   - Go to: Developers → Webhooks
   - Click "Add endpoint"
   - Endpoint URL: `https://your-api-url.com/api/payment/webhook`
   - Select events to listen to:
     - `payment_intent.succeeded`
     - `payment_intent.payment_failed`
     - `payment_intent.canceled`
     - `charge.refunded`
   - Copy the **Webhook Signing Secret**: `whsec_...`

---

### Step 2: Configure Backend

1. **Update `appsettings.json`** (or `appsettings.Development.json`):
   ```json
   {
     "Stripe": {
       "PublishableKey": "pk_test_your_actual_key_here",
       "SecretKey": "sk_test_your_actual_key_here",
       "WebhookSecret": "whsec_your_webhook_secret_here",
       "Currency": "USD"
     }
   }
   ```

2. **For Production**, use environment variables:
   ```bash
   STRIPE_PUBLISHABLE_KEY=pk_live_...
   STRIPE_SECRET_KEY=sk_live_...
   STRIPE_WEBHOOK_SECRET=whsec_...
   ```

---

### Step 3: Configure Frontend

1. **Update `environment.ts`**:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'http://localhost:5015/api',
     stripePublishableKey: 'pk_test_your_actual_key_here' // Replace with your key
   };
   ```

2. **For Production**, update `environment.prod.ts`:
   ```typescript
   export const environment = {
     production: true,
     apiUrl: 'https://your-api-url.com/api',
     stripePublishableKey: 'pk_live_your_actual_key_here'
   };
   ```

---

### Step 4: Database Migration

The database will automatically create the payment tables when you first run the application (using `EnsureCreated()`).

**To manually verify:**
1. Run the backend application
2. Check your database - you should see:
   - `Payments` table
   - `PaymentTransactions` table
   - `Users` table should have `StripeCustomerId` column

**If you prefer using migrations:**
```bash
cd backend
dotnet ef migrations add AddPaymentTables
dotnet ef database update
```

---

## 🧪 Testing

### Test Cards (Stripe Test Mode)

Use these test card numbers in the payment form:

| Card Number | Scenario | CVC | Expiry |
|-------------|----------|-----|--------|
| `4242 4242 4242 4242` | Success | Any 3 digits | Any future date |
| `4000 0000 0000 0002` | Card declined | Any 3 digits | Any future date |
| `4000 0025 0000 3155` | 3D Secure required | Any 3 digits | Any future date |
| `4000 0000 0000 9995` | Insufficient funds | Any 3 digits | Any future date |

### Testing Locally

1. **Start Backend:**
   ```bash
   cd backend
   dotnet run --project AITravelPlanner.Api
   ```

2. **Start Frontend:**
   ```bash
   cd frontend
   npm start
   ```

3. **Test Payment Flow:**
   - Navigate to a travel plan details page
   - Click "Pay Now" button
   - Enter test card: `4242 4242 4242 4242`
   - Use any future expiry date and any 3-digit CVC
   - Complete payment

### Testing Webhooks Locally

Use **Stripe CLI** to forward webhooks to your local server:

```bash
# Install Stripe CLI
# Windows: Download from https://github.com/stripe/stripe-cli/releases
# Mac: brew install stripe/stripe-cli/stripe
# Linux: See Stripe docs

# Login
stripe login

# Forward webhooks
stripe listen --forward-to localhost:5015/api/payment/webhook
```

This will give you a webhook signing secret to use in `appsettings.Development.json`.

---

## 📱 Using the Payment UI

### For Users:

1. **Navigate to Travel Plan Details**
   - Go to any travel plan
   - View the budget summary

2. **Initiate Payment**
   - Click "Pay Now" button (in header or budget section)
   - Payment dialog opens

3. **Enter Card Details**
   - Card number, expiry, CVC
   - All handled securely by Stripe

4. **Complete Payment**
   - Click "Pay [Amount]"
   - Wait for confirmation
   - View payment status

5. **View Receipt**
   - After successful payment
   - Click "View Receipt" to see Stripe receipt

---

## 🔒 Security Notes

1. **Never commit API keys to Git**
   - Use environment variables
   - Add to `.gitignore`

2. **Use HTTPS in Production**
   - Required for Stripe
   - Never use HTTP with real payments

3. **Webhook Security**
   - Always verify webhook signatures
   - Already implemented in `PaymentService`

4. **PCI Compliance**
   - Stripe handles PCI compliance
   - Card details never touch your server
   - All handled by Stripe Elements

---

## 🐛 Troubleshooting

### Payment Form Not Loading
- **Check:** Stripe publishable key in `environment.ts`
- **Check:** Browser console for errors
- **Solution:** Ensure key starts with `pk_test_` or `pk_live_`

### Payment Intent Creation Fails
- **Check:** Backend logs for errors
- **Check:** Stripe secret key in `appsettings.json`
- **Check:** Network tab for API errors
- **Solution:** Verify API keys are correct

### Webhooks Not Working
- **Check:** Webhook endpoint is accessible
- **Check:** Webhook secret in configuration
- **Check:** Stripe dashboard → Webhooks → Events
- **Solution:** Use Stripe CLI for local testing

### Database Errors
- **Check:** Connection string in `appsettings.json`
- **Check:** Database exists and is accessible
- **Solution:** Run `EnsureCreated()` or apply migrations

---

## 📊 API Endpoints

### Payment Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/payment/create-intent` | Create payment intent | Yes |
| GET | `/api/payment/{id}/status` | Get payment status | Yes |
| GET | `/api/payment/{paymentId}` | Get payment by ID | Yes |
| GET | `/api/payment/my-payments` | Get user's payments | Yes |
| POST | `/api/payment/webhook` | Stripe webhook | No (but verified) |

---

## 🎨 UI Features

- ✅ Beautiful, modern payment form
- ✅ Real-time card validation
- ✅ Loading states
- ✅ Error handling
- ✅ Success confirmation
- ✅ Payment status display
- ✅ Receipt viewing
- ✅ Responsive design

---

## 📚 Additional Resources

- **Stripe Documentation:** https://stripe.com/docs
- **Stripe Testing:** https://stripe.com/docs/testing
- **Stripe Dashboard:** https://dashboard.stripe.com
- **Stripe CLI:** https://stripe.com/docs/stripe-cli

---

## ✅ Next Steps

1. ✅ Add your Stripe API keys
2. ✅ Test with test cards
3. ✅ Set up webhooks
4. ✅ Test end-to-end payment flow
5. ✅ Deploy to production
6. ✅ Switch to live keys

---

**Need Help?** Check the main `PAYMENT_INTEGRATION_PLAN.md` for detailed architecture and implementation details.

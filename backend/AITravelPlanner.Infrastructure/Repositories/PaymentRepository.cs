using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AITravelPlanner.Domain.Entities;
using AITravelPlanner.Domain.Interfaces;
using AITravelPlanner.Infrastructure.Data;

namespace AITravelPlanner.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments
                .Include(p => p.TravelPlan)
                .Include(p => p.User)
                .Include(p => p.Transactions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId)
        {
            return await _context.Payments
                .Include(p => p.TravelPlan)
                .Include(p => p.User)
                .Include(p => p.Transactions)
                .FirstOrDefaultAsync(p => p.StripePaymentIntentId == stripePaymentIntentId);
        }

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(int userId)
        {
            return await _context.Payments
                .Include(p => p.TravelPlan)
                .Include(p => p.Transactions)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByTravelPlanIdAsync(int travelPlanId)
        {
            return await _context.Payments
                .Include(p => p.User)
                .Include(p => p.Transactions)
                .Where(p => p.TravelPlanId == travelPlanId)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<PaymentTransaction> AddTransactionAsync(PaymentTransaction transaction)
        {
            _context.PaymentTransactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> TransactionExistsAsync(string stripeEventId)
        {
            return await _context.PaymentTransactions
                .AnyAsync(pt => pt.StripeEventId == stripeEventId);
        }
    }
}

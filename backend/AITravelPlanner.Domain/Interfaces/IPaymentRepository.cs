using System.Collections.Generic;
using System.Threading.Tasks;
using AITravelPlanner.Domain.Entities;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByStripePaymentIntentIdAsync(string stripePaymentIntentId);
        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetByTravelPlanIdAsync(int travelPlanId);
        Task<Payment> UpdateAsync(Payment payment);
        Task<PaymentTransaction> AddTransactionAsync(PaymentTransaction transaction);
        Task<bool> TransactionExistsAsync(string stripeEventId);
    }
}

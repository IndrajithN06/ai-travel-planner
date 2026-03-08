using System.Collections.Generic;
using System.Threading.Tasks;
using AITravelPlanner.Domain.DTOs;

namespace AITravelPlanner.Domain.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponse> CreatePaymentIntentAsync(CreatePaymentIntentRequest request, int userId);
        Task<PaymentStatusResponse> GetPaymentStatusAsync(string paymentIntentId);
        Task<PaymentResponse> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<PaymentResponse>> GetUserPaymentsAsync(int userId);
        Task<bool> HandleWebhookAsync(string payload, string signature);
    }
}

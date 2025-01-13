using PastPaperRepository.Application.Models.Payment;

namespace PastPaperRepository.Application.Services.Payments;

public interface IPaymentService
{
    Task<bool> CreatePaymentAsync(CreatePaymentModel model, CancellationToken cancellationToken = default);
}
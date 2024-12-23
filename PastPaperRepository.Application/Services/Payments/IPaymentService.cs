using PastPaperRepository.Application.Models.Payment;
using PastPaperRepository.Application.Repositories.Payments;

namespace PastPaperRepository.Application.Services.Payments;

public interface IPaymentService
{
    Task<bool> CreatePaymentAsync(CreatePaymentModel model, CancellationToken cancellationToken = default);
}
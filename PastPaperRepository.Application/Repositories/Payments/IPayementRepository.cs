using PastPaperRepository.Application.Models.Payment;

namespace PastPaperRepository.Application.Repositories.Payments;

public interface IPayementRepository
{
    Task<bool> CreatePaymentAsync(CreatePaymentModel model, CancellationToken cancellationToken = default);
}
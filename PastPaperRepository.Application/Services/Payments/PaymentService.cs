using PastPaperRepository.Application.Models.Payment;
using PastPaperRepository.Application.Repositories.Payments;

namespace PastPaperRepository.Application.Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly IPayementRepository _paymentRepository;

    public PaymentService(IPayementRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public Task<bool> CreatePaymentAsync(CreatePaymentModel model, CancellationToken cancellationToken = default)
    {
        return _paymentRepository.CreatePaymentAsync(model, cancellationToken);
    }
}
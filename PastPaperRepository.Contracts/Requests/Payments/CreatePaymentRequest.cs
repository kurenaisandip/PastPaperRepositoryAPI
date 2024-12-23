using System.Runtime.InteropServices.JavaScript;

namespace PastPaperRepository.Contracts.Requests.Payments;

public class CreatePaymentRequest
{
    public long UserId { get; init; }
    public string ProductName { get; init; } = default!;
    public long Price { get; init; } = default!;
    public DateTime ValidUntil { get; init; }
}
namespace PastPaperRepository.Application.Models.Payment;

public class CreatePaymentModel
{
    public long UserId { get; init; }
    public string ProductName { get; init; } = default!;
    public long Price { get; init; } = default!;
    public DateTime ValidUntil { get; init; }
}
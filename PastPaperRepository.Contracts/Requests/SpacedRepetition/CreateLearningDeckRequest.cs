namespace PastPaperRepository.Contracts.Requests.SpacedRepetition;

public class CreateLearningDeckRequest
{
    public long UserId { get; init; }
    public string PastPaperId { get; init; }
    public string Status { get; init; }
    public DateTime AddedDate { get; init; }
    public DateTime NextReviewDate { get; init; }
}
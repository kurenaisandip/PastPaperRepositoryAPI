namespace PastPaperRepository.Application.Models.SpacedRepetition;

public class LearningDeck
{
    public long UserId { get; init; }
    public string PastPaperId { get; init; }
    public DateTime AddedDate { get; init; }
    public DateTime NextReviewDate { get; init; }
    public string Status { get; init; }

    #region nullable properties

    public long? QuestionNumber { get; init; }
    public long? Score { get; init; }

    #endregion
}
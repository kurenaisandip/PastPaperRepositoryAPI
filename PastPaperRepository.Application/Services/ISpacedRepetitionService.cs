using PastPaperRepository.Application.Models.SpacedRepetition;

namespace PastPaperRepository.Application.Services;

public interface ISpacedRepetitionService
{
    Task<bool> AddToLearningDeckAsync(LearningDeck request, CancellationToken token = default);
    Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token);
}
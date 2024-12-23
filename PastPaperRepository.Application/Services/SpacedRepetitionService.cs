using PastPaperRepository.Application.Models;
using PastPaperRepository.Application.Models.SpacedRepetition;
using PastPaperRepository.Application.Repositories;

namespace PastPaperRepository.Application.Services;

public class SpacedRepetitionService : ISpacedRepetitionService
{
    private readonly ISpacedRepetitionRepository _spacedRepetitionRepository;
    public SpacedRepetitionService(ISpacedRepetitionRepository spacedRepetitionRepository)
    {
        _spacedRepetitionRepository = spacedRepetitionRepository;
    }

    public Task<bool> AddToLearningDeckAsync(LearningDeck request, CancellationToken token = default)
    {
        return _spacedRepetitionRepository.AddToLearningDeckAsync(request, token);
    }

    public Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token)
    {
        return _spacedRepetitionRepository.GetLearningDeckAsync(userId, token);
    }

    public Task<List<QuestionAnswers>> ShowQuestionAnswerAsync(string pastPaperId, long userId,
        CancellationToken token = default)
    {
        return _spacedRepetitionRepository.ShowQuestionAnswerAsync(pastPaperId, userId, token);
    }
}
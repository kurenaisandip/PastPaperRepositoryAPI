using PastPaperRepository.Application.Models.SpacedRepetition;
using QuestionAnswers = PastPaperRepository.Application.Services.QuestionAnswers;

namespace PastPaperRepository.Application.Repositories;

public interface ISpacedRepetitionRepository
{
    Task<bool> AddToLearningDeckAsync(LearningDeck request, CancellationToken token = default);
    Task<List<DeckViewModel>> GetLearningDeckAsync(long userId, CancellationToken token = default);
    
    Task<List<QuestionAnswers>> ShowQuestionAnswerAsync( string UserId, long pastPaperId, CancellationToken token = default);
}
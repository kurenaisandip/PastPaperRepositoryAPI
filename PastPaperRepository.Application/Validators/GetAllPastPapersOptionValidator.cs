using FluentValidation;
using PastPaperRepository.Application.Models;

namespace PastPaperRepository.Application.Validators;

public class GetAllPastPapersOptionValidator: AbstractValidator<GetAllPastPapersOptions>
{
    public GetAllPastPapersOptionValidator()
    {
        RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.UtcNow.Year);
    }
}
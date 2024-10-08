using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.Application.Repository;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class PastPaperController : ControllerBase
{
    private readonly IPastPaperRepository _pastPaperRepository;

    public PastPaperController(IPastPaperRepository pastPaperRepository)
    {
        _pastPaperRepository = pastPaperRepository;
    }
}
    
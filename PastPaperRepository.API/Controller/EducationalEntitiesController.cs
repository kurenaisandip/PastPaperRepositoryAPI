using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Contracts.Requests.EducationalEntities;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class EducationalEntitiesController : ControllerBase
{
    public readonly IEducationalEntitiesRepository _repository;

    public EducationalEntitiesController(IEducationalEntitiesRepository repository)
    {
        _repository = repository;
    }

    [HttpPost(ApiEndPoints.EducationalEntities.CreateRole)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var role = request.MapToRoles();
        
        await _repository.CreateRoleAsync(role);
        
        var response = role.MapToResponseRole();

        return Ok(response);
    }
}
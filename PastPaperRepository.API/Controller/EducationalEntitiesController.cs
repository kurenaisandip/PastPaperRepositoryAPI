using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Services.EducationalEntities;
using PastPaperRepository.Contracts.Requests.EducationalEntities;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class EducationalEntitiesController : ControllerBase
{
    public readonly IEducationalEntitiesService _service;

    public EducationalEntitiesController(IEducationalEntitiesService service)
    {
        _service = service;
    }

    [HttpPost(ApiEndPoints.EducationalEntities.CreateRole)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken token)
    {
        if (request == null)
        {
            return BadRequest("Request body cannot be null.");
        }
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Role name is required and cannot be empty.");
        }
        var role = request.MapToRoles();
        
        await _service.CreateRoleAsync(role, token);
        
        var response = role.MapToResponseRole();

        return Ok(response);
    }

    [HttpPost(ApiEndPoints.EducationalEntities.CreateSchool)]
    public async Task<IActionResult> CreateSchool([FromBody] CreateSchoolRequest request, CancellationToken token)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        if (string.IsNullOrWhiteSpace(request.Name) && string.IsNullOrWhiteSpace(request.Address))
        {
            return BadRequest("School name and location are required and cannot be empty.");
        }

        var school = request.MapToSchool();
        
        var result = await _service.CreateSchoolAsync(school, token);

        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
    
}
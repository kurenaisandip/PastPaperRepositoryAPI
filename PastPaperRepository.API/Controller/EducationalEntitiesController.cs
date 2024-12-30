using Microsoft.AspNetCore.Mvc;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Services.EducationalEntities;
using PastPaperRepository.Contracts.Requests.EducationalEntities;

namespace PastPaperRepository.API.Controller;

[ApiController]
public class EducationalEntitiesController : ControllerBase
{
    private readonly IEducationalEntitiesService _service;

    public EducationalEntitiesController(IEducationalEntitiesService service)
    {
        _service = service;
    }

    [HttpPost(ApiEndPoints.EducationalEntities.CreateRole)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request, CancellationToken token)
    {
        throw new Exception("Test exception for Sentry");
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
    
    [HttpPost(ApiEndPoints.EducationalEntities.CreateSubject)]
    public async Task<IActionResult>CreateSubject([FromBody] CreateSubjectRequest request, CancellationToken token)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        if (string.IsNullOrWhiteSpace(request.SubjectName))
        {
            return BadRequest("Subject name is required and cannot be empty.");
        }

        var subject = request.MapToSubject();
        
     var result = await _service.CreateSubjectAsync(subject, token);

     if (!result)
     {
         return BadRequest();
     }
        return Ok();
    }
    
        
    [HttpPost(ApiEndPoints.EducationalEntities.CreateCategories)]
    public async Task<IActionResult>CreateCategory([FromBody] CreateCategoriesRequest request, CancellationToken token)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        if (string.IsNullOrWhiteSpace(request.CategoryName))
        {
            return BadRequest("Subject name is required and cannot be empty.");
        }

        var categories = request.MapToCategories();
        
        var result = await _service.CreateCategoryAsync(categories, token);

        if (!result)
        {
            return BadRequest();
        }
        return Ok();
    }
    
}
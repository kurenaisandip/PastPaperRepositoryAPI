using FluentValidation;
using PastPaperRepository.Contracts.Responses;

namespace PastPaperRepository.API.Mapping;

public class ValidationMappingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationMappingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //pass the context to the next middleware if sucessfull
            await _next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400; //bad request
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = exception.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}
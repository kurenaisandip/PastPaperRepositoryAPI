using Microsoft.Extensions.DependencyInjection;
using PastPaperRepository.Application.Repository;

namespace PastPaperRepository.Application.ApplicationService;

public static class ApplicationServiceCollectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPastPaperRepository, Repository.PastPaperRepository>();
        return services;
    }
    
}
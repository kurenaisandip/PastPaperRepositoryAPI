using System.Data;
using Microsoft.Extensions.DependencyInjection;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Repositories;

namespace PastPaperRepository.Application.ApplicationService;

public static class ApplicationServiceCollectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPastPaperRepository, Repositories.PastPaperRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));
        services.AddSingleton<DbInitalizer>();
        return services;
    }
    
}
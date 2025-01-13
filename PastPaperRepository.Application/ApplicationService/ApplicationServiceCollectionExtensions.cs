using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Repositories.Payments;
using PastPaperRepository.Application.Services;
using PastPaperRepository.Application.Services.EducationalEntities;
using PastPaperRepository.Application.Services.Payments;

namespace PastPaperRepository.Application.ApplicationService;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPastPaperRepository, Repositories.PastPaperRepository>();
        services.Decorate<IPastPaperRepository, CachedPastPaperRepository>();
        services.AddSingleton<IPastPaperService, PastPaperService>();
        services.AddSingleton<IEducationalEntitiesRepository, EducationalEntitiesRepository>();
        services.AddSingleton<IEducationalEntitiesService, EducationalEntitiesService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

        services.AddSingleton<IUserLoginRepository, UserLoginRepository>();
        services.AddSingleton<ISpacedRepetitionService, SpacedRepetitionService>();

        services.AddSingleton<ISpacedRepetitionRepository, SpacedRepetitionRepository>();
        services.AddSingleton<IPayementRepository, PaymentRepository>();
        services.AddSingleton<IPaymentService, PaymentService>();


        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));
        services.AddSingleton<DbInitalizer>();
        return services;
    }
}
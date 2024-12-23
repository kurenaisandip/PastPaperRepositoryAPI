﻿using System.Data;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PastPaperRepository.Application.Database;
using PastPaperRepository.Application.Repositories;
using PastPaperRepository.Application.Services;

namespace PastPaperRepository.Application.ApplicationService;

public static class ApplicationServiceCollectionExtensions
{

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IPastPaperRepository, Repositories.PastPaperRepository>();
        services.AddSingleton<IPastPaperService, Services.PastPaperService>();
        services.AddSingleton<IEducationalEntitiesRepository, EducationalEntitiesRepository>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);

        services.AddSingleton<IUserLoginRepository, UserLoginRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));
        services.AddSingleton<DbInitalizer>();
        return services;
    }
    
}
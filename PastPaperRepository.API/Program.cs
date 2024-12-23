using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PastPaperRepository.API.Auth;
using PastPaperRepository.API.Health;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.API.Swagger;
using PastPaperRepository.Application.ApplicationService;
using PastPaperRepository.Application.Database;
using Stripe;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(x =>
    {
        x.DefaultApiVersion = new ApiVersion(1.0);
        x.AssumeDefaultVersionWhenUnspecified = true;
        x.ReportApiVersions = true;
    })
    .AddMvc().AddApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddControllers();
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValue>());

var config = builder.Configuration;

//JWT configuration
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = jwtIssuer,
            ValidIssuer = jwtIssuer,
        };
    });

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AuthConstants.AdminUserPolicyName,
        policy => policy.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

    x.AddPolicy(AuthConstants.TrustedMemberPolicyName,
        policy => policy.RequireAssertion(context =>
            context.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
            context.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" })
        ));
});

// not a good way to DI in the application
// builder.Services.AddSingleton<IPastPaperRepository, PastPaperRepository>();
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

var app = builder.Build();

StripeConfiguration.ApiKey = app.Configuration["Stripe:SecretKey"];

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            x.SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

builder.Services.AddResponseCaching();

app.MapHealthChecks("_health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.UseResponseCaching();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitalizer>();
await dbInitializer.InitializeAsync();

app.Run();
using System.Text;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using PastPaperRepository.API;
using PastPaperRepository.API.Auth;
using PastPaperRepository.API.Health;
using PastPaperRepository.API.Mapping;
using PastPaperRepository.API.Swagger;
using PastPaperRepository.Application.ApplicationService;
using PastPaperRepository.Application.Database;
using Sentry.Infrastructure;
using Sentry.OpenTelemetry;
using Stripe;
using Swashbuckle.AspNetCore.SwaggerGen;
using ProfilingIntegration = Sentry.Profiling.ProfilingIntegration;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    options.AddConsoleExporter();   
    options.SetResourceBuilder(ResourceBuilder.CreateEmpty().AddService("PastPaperRepository.API").AddAttributes(new Dictionary<string, object>()
    {
        ["deployment.environment"] = builder.Environment.EnvironmentName,
    }));

    options.IncludeScopes = true;
    
    options.IncludeFormattedMessage = true;

    options.AddOtlpExporter(a =>
        {
            a.Protocol = OtlpExportProtocol.HttpProtobuf;
            a.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
            a.Headers = "X-Seq-ApiKey=sJtJA0gJhqPIhN2ZLBbC";
            // a.AddHeader("X-Seq-ApiKey", "sJtJA0gJhqPIhN2ZLBbC");
        }
    );
});




// Add services to the container
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1.0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
}).AddMvc().AddApiExplorer();


builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    string connection = builder.Configuration.GetConnectionString("Redis");
    options.Configuration = connection;
});
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValue>());

var config = builder.Configuration;

// Add CORS configuration
string _defaultCorsPolicyName = "localhost";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _defaultCorsPolicyName,
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// JWT configuration
var jwtIssuer = config.GetSection("Jwt:Issuer").Get<string>();
var jwtAudience = config.GetSection("Jwt:Audience").Get<string>();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidAudience = jwtAudience,
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

// Application and Database setup
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

builder.Services.AddResponseCaching();

builder.Services.AddSingleton<MailgunEmailSender>(sp => new MailgunEmailSender(
    builder.Configuration["Mailgun:ApiKey"]!,
    builder.Configuration["Mailgun:Domain"]!
));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
            tracerProviderBuilder
                .AddAspNetCoreInstrumentation() // <-- Adds ASP.NET Core telemetry sources
                .AddHttpClientInstrumentation() // <-- Adds HttpClient telemetry sources
                .AddSentry() // <-- Configure OpenTelemetry to send trace information to Sentry
    );

builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://5135dc7f165984f95071655459d3c59f@o4508546203713536.ingest.us.sentry.io/4508547039166464";
    options.TracesSampleRate = 1.0;
    options.UseOpenTelemetry();
    options.Debug = true;
    options.AutoSessionTracking = true;
    options.ProfilesSampleRate = 1.0;
    options.AddIntegration(new ProfilingIntegration(TimeSpan.FromMilliseconds(500)));
});


SentrySdk.Init(options =>
{
    // A Sentry Data Source Name (DSN) is required.
    // See https://docs.sentry.io/product/sentry-basics/dsn-explainer/
    // You can set it in the SENTRY_DSN environment variable, or you can set it in code here.
    options.Dsn = "https://5135dc7f165984f95071655459d3c59f@o4508546203713536.ingest.us.sentry.io/4508547039166464";

    // When debug is enabled, the Sentry client will emit detailed debugging information to the console.
    // This might be helpful, or might interfere with the normal operation of your application.
    // We enable it here for demonstration purposes when first trying Sentry.
    // You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
    options.Debug = true;
    
    options.DiagnosticLevel = SentryLevel.Debug;
    
    options.DiagnosticLogger = new FileDiagnosticLogger("D:/Applications/Sentry/sentry-diagnostic.log");

    // This option is recommended. It enables Sentry's "Release Health" feature.
    options.AutoSessionTracking = true;

    // Set TracesSampleRate to 1.0 to capture 100%
    // of transactions for tracing.
    // We recommend adjusting this value in production.
    options.TracesSampleRate = 1.0;

    // Sample rate for profiling, applied on top of othe TracesSampleRate,
    // e.g. 0.2 means we want to profile 20 % of the captured transactions.
    // We recommend adjusting this value in production.
    options.ProfilesSampleRate = 1.0;
    // Requires NuGet package: Sentry.Profiling
    // Note: By default, the profiler is initialized asynchronously. This can
    // be tuned by passing a desired initialization timeout to the constructor.
    options.AddIntegration(new ProfilingIntegration(
        // During startup, wait up to 500ms to profile the app startup code.
        // This could make launching the app a bit slower so comment it out if you
        // prefer profiling to start asynchronously
        TimeSpan.FromMilliseconds(500)
    ));
});
// SentrySdk.Init(options =>
// {
//     // A Sentry Data Source Name (DSN) is required.
//     // See https://docs.sentry.io/product/sentry-basics/dsn-explainer/
//     // You can set it in the SENTRY_DSN environment variable, or you can set it in code here.
//     options.Dsn = "https://5135dc7f165984f95071655459d3c59f@o4508546203713536.ingest.us.sentry.io/4508547039166464";
//
//     // When debug is enabled, the Sentry client will emit detailed debugging information to the console.
//     // This might be helpful, or might interfere with the normal operation of your application.
//     // We enable it here for demonstration purposes when first trying Sentry.
//     // You shouldn't do this in your applications unless you're troubleshooting issues with Sentry.
//     options.Debug = true;
//
//     // This option is recommended. It enables Sentry's "Release Health" feature.
//     options.AutoSessionTracking = true;
//
//     // Set TracesSampleRate to 1.0 to capture 100%
//     // of transactions for tracing.
//     // We recommend adjusting this value in production.
//     options.TracesSampleRate = 1.0;
//
//     // Sample rate for profiling, applied on top of othe TracesSampleRate,
//     // e.g. 0.2 means we want to profile 20 % of the captured transactions.
//     // We recommend adjusting this value in production.
//     options.ProfilesSampleRate = 1.0;
//     // Requires NuGet package: Sentry.Profiling
//     // Note: By default, the profiler is initialized asynchronously. This can
//     // be tuned by passing a desired initialization timeout to the constructor.
//     options.AddIntegration(new ProfilingIntegration(
//         // During startup, wait up to 500ms to profile the app startup code.
//         // This could make launching the app a bit slower so comment it out if you
//         // prefer profiling to start asynchronously
//         TimeSpan.FromMilliseconds(500)
//     ));
// });
//
// SentrySdk.CaptureMessage("Something went wrong");




var app = builder.Build();

StripeConfiguration.ApiKey = app.Configuration["Stripe:SecretKey"];

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            x.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

app.UseHttpsRedirection();

// Apply CORS middleware
app.UseCors(_defaultCorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCaching();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapHealthChecks("_health");
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitalizer>();
await dbInitializer.InitializeAsync();

app.Run();



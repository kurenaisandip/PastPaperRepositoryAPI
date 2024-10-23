using PastPaperRepository.Application.ApplicationService;
using PastPaperRepository.Application.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = builder.Configuration;

// not a good way to DI in the application
// builder.Services.AddSingleton<IPastPaperRepository, PastPaperRepository>();
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitalizer>();
await dbInitializer.InitializeAsync();

app.Run();


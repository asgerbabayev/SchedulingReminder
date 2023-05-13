using Hangfire;
using ShedulingReminders.Application;
using ShedulingReminders.Infrastructure;
using ShedulingReminders.Infrastructure.Persistance;
using ShedulingReminders.WebUI;
using ShedulingReminders.WebUI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddWebUIService();
var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    AppDbContextInitialiser initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();
    await initializer.InitializeAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("/hangfire");

app.Run();

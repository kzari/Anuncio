using Hangfire;
using Hangfire.Console;
using Lopes.Jobs.Api;
using Lopes.Jobs.Api.Log;
using Lopes.Infra.IoC;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//Hangfire
builder.Services.AddHangfire(x => 
{
    x.UseSqlServerStorage("DbLopesnet");
    x.UseConsole();
});
builder.Services.AddHangfireServer();

ServiceConfiguration.Configure<HangFireLog>(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



WebApplication? app = builder.Build();

new Startup().Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();

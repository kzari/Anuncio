using Hangfire;
using Hangfire.Console;
using Lopes.SC.Jobs.Api;
using Lopes.SC.Jobs.Api.Log;
using Lopes.SC.Infra.IoC;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//Hangfire
builder.Services.AddHangfire(x => 
{
    x.UseSqlServerStorage(@"Data Source=LPS-SI-DEV02\SQLCORP_HML;Enlist=false;Initial Catalog=DbLopesNet;User id=usrapp; password=Lopesnet2010;Pooling=true;Application Name=LopesNetCRM; Connection Timeout=1200;");
    x.UseConsole();
});
builder.Services.AddHangfireServer();

ServiceConfiguration.Configure<HangFireLog>(builder.Services);

builder.Services.AddSingleton<IHostLifetime, NoopConsoleLifetime>();

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

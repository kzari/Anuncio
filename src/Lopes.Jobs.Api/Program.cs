using Hangfire;
using Hangfire.Console;
using Lopes.Jobs.Api;
using Lopes.Jobs.Api.Log;
using Lopes.Infra.IoC;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.



IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json")
    .Build();

//Hangfire
builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(configuration.GetConnectionString("DbLopesnet"));
    x.UseConsole();
});
builder.Services.AddHangfireServer();

ConfiguracaoServicos.ConfigurarServicos<HangFireLog>(configuration, builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc();

WebApplication? app = builder.Build();

new Startup().Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

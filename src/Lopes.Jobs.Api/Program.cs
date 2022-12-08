using Hangfire;
using Hangfire.Console;
using Lopes.Jobs.Api;
using Lopes.Jobs.Api.Log;
using Lopes.Acesso.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Lopes.Acesso.Application;

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

//builder.Services.AddTransient<ITokenService, JwtTokenService>();

builder.Services.AddControllers(config =>
{
    //AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
    //    //.RequireAuthenticatedUser()
    //    .Build();
    //config.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc();

ConfigurarAutenticacao(builder);

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


static void ConfigurarAutenticacao(WebApplicationBuilder builder)
{
    byte[]? key = Encoding.ASCII.GetBytes(JwtTokenService.SECRET);

    builder.Services.AddAuthentication(_ =>
    {
        _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(_ =>
    {
        _.RequireHttpsMetadata = false;
        _.SaveToken = true;
        _.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    //builder.Services.AddAuthorization(_ =>
    //{
    //    //_.FallbackPolicy = new AuthorizationPolicyBuilder()
    //    //    //.RequireAuthenticatedUser()
    //    //    .Build();
    //});
}
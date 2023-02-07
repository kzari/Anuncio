using Julio.Acesso.Application;
using Julio.Acesso.MemoryCache;
using Julio.Anuncio.IoC;
using Julio.Botmaker.IoC;
using Julio.Domain.Common.IoC;
using Julio.Domain.Commons.Cache;
using Julio.Infra.Common;
using Julio.Jobs.Web.Log;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;


IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.json")
    .Build();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var ioc = new ConfiguradorIoC(new BotmakerIoC(TipoBaseDados.Producao),
                              new AnuncioIoC(TipoBaseDados.Hml));
ioc.Configurar<Log>(configuration, builder.Services);

ioc.ServiceCollection.AddMemoryCache();
ioc.ServiceCollection.AddSingleton<ICacheService, MemoryCacheService>();

ConfigurarAutenticacao(builder);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();


app.UseStatusCodePages(async context => {
    HttpRequest request = context.HttpContext.Request;
    HttpResponse response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    // you may also check requests path to do this only for specific methods       
    // && request.Path.Value.StartsWith("/specificPath")

    {
        response.Redirect("/Login");
    }
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();




static void ConfigurarAutenticacao(WebApplicationBuilder builder)
{
    byte[]? key = Encoding.ASCII.GetBytes(JwtTokenService.SECRET);

    builder.Services.AddAuthentication(_ =>
    {
        _.DefaultAuthenticateScheme = JwtTokenService.JwtAuthenticationScheme;
        _.DefaultChallengeScheme = JwtTokenService.JwtAuthenticationScheme;
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
    //    _.FallbackPolicy = new AuthorizationPolicyBuilder()
    //        .RequireAuthenticatedUser()
    //        .Build();
    //});

}

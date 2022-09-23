using Hangfire;
using Lopes.Jobs.Api.Configuracoes;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder = ConfiguracaoServicosApi.Configurar(builder);

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

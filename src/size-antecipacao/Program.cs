using Microsoft.EntityFrameworkCore;
using size.Carrinho.Data.Context;
using size.CatalogoRecebiveis.Data.Context;
using size.fichaCadastral.Data.Context;
using size.Operacao.Data.Context;
using size_antecipacao.Configurations;
using size_antecipacao.Infrastructure;

IConfiguration configuration = null;
try
{
    var builder = WebApplication.CreateBuilder(args);       
    configuration = builder.Configuration;

    // Configurar Kestrel para aceitar requisições de qualquer origem (necessário para Codespaces)
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenAnyIP(5075);
    });

    builder.Services.AdicionarServicos(configuration);
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    // Registrar o DatabaseSeeder
    builder.Services.AddScoped<DatabaseSeeder>();
    
    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        
        try
        {
            var fichaCadastralContext = services.GetRequiredService<FichaCadastralContext>();
            fichaCadastralContext.Database.Migrate();
            logger.LogInformation("Migrations do FichaCadastral aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations do FichaCadastral.");
            throw;
        }

        try
        {
            var catalogoRecebiveisContext = services.GetRequiredService<CatalogoRecebiveisContext>();
            catalogoRecebiveisContext.Database.Migrate();
            logger.LogInformation("Migrations do CatalogoRecebiveis aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations do CatalogoRecebiveis.");
            throw;
        }

        try
        {
            var carrinhoContext = services.GetRequiredService<CarrinhoContext>();
            carrinhoContext.Database.Migrate();
            logger.LogInformation("Migrations do Carrinho aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations do Carrinho.");
            throw;
        }

        try
        {
            var operacaoContext = services.GetRequiredService<OperacaoContext>();
            operacaoContext.Database.Migrate();
            logger.LogInformation("Migrations do Operacao aplicadas com sucesso.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro ao aplicar as migrations do Operacao.");
            throw;
        }

        // Executar seed do banco após migrations
        try
        {
            logger.LogInformation("?? Iniciando seed do banco de dados...");
            var seeder = services.GetRequiredService<DatabaseSeeder>();
            seeder.SeedDatabaseAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "? Erro ao executar seed do banco de dados.");
            // Não propaga exceção para não interromper a aplicação
        }
    }

    // Sempre usar Swagger em desenvolvimento (incluindo Codespaces)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Redirecionar raiz para Swagger (útil para Codespaces)
    app.MapGet("/", () => Results.Redirect("/swagger"));

    // Não usar HTTPS redirect em desenvolvimento (Codespaces usa proxy próprio)
    // app.UseHttpsRedirection();
    
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{

    throw;
}

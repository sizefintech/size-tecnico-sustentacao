using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace size_antecipacao.Infrastructure
{
    public class DatabaseSeeder
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(IConfiguration configuration, ILogger<DatabaseSeeder> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogWarning("Connection string not found. Skipping database seeding.");
                    return;
                }

                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "script.sql");
                
                if (!File.Exists(scriptPath))
                {
                    _logger.LogWarning($"Script SQL não encontrado em: {scriptPath}");
                    return;
                }

                _logger.LogInformation("?? Iniciando seed do banco de dados...");

                var scriptContent = await File.ReadAllTextAsync(scriptPath);
                
                // Divide o script em comandos individuais (separados por GO)
                var commands = scriptContent
                    .Split(new[] { "\nGO\n", "\rGO\r", "\r\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var commandText in commands)
                {
                    if (string.IsNullOrWhiteSpace(commandText))
                        continue;

                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = commandText;
                        command.CommandTimeout = 300; // 5 minutos
                        
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Erro ao executar comando SQL: {ex.Message}");
                        // Continua executando os demais comandos
                    }
                }

                _logger.LogInformation("? Seed do banco de dados concluído!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Erro ao executar seed do banco de dados");
                // Não propaga a exceção para não interromper a aplicação
            }
        }
    }
}

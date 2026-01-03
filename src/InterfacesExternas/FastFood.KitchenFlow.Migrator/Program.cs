using FastFood.KitchenFlow.Infra.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

Console.WriteLine("=== KitchenFlow Migrator ===");
Console.WriteLine();

try
{
    // Carregar configuração com prioridade: variáveis de ambiente > appsettings.Development.json > appsettings.json
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    // Extrair connection string com prioridade: variável de ambiente > appsettings
    var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
        ?? configuration.GetConnectionString("DefaultConnection");

    // Validar connection string
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("ERRO: Connection string não encontrada!");
        Console.WriteLine("Configure a connection string via:");
        Console.WriteLine("  - Variável de ambiente: ConnectionStrings__DefaultConnection");
        Console.WriteLine("  - appsettings.Development.json (não commitado)");
        Console.WriteLine("  - appsettings.json (fallback)");
        Console.ResetColor();
        return 1;
    }

    // Extrair host da connection string para exibição (opcional)
    var host = ExtractHostFromConnectionString(connectionString);
    Console.WriteLine($"Connection string encontrada (Host: {host})");
    Console.WriteLine();

    // Configurar DbContext
    var optionsBuilder = new DbContextOptionsBuilder<KitchenFlowDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    // Criar instância do DbContext
    using var context = new KitchenFlowDbContext(optionsBuilder.Options);

    // Verificar migrations pendentes
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    var pendingMigrationsList = pendingMigrations.ToList();

    if (pendingMigrationsList.Any())
    {
        Console.WriteLine($"Migrations pendentes encontradas: {pendingMigrationsList.Count}");
        foreach (var migration in pendingMigrationsList)
        {
            Console.WriteLine($"  - {migration}");
        }
        Console.WriteLine();

        // Aplicar migrations
        Console.WriteLine("Aplicando migrations...");
        await context.Database.MigrateAsync();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✓ Migrations aplicadas com sucesso!");
        Console.ResetColor();
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Nenhuma migration pendente.");
        Console.WriteLine();
    }

    // Listar migrations aplicadas
    var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
    var appliedMigrationsList = appliedMigrations.ToList();

    if (appliedMigrationsList.Any())
    {
        Console.WriteLine($"Migrations aplicadas: {appliedMigrationsList.Count}");
        foreach (var migration in appliedMigrationsList)
        {
            Console.WriteLine($"  - {migration}");
        }
    }
    else
    {
        Console.WriteLine("Nenhuma migration aplicada ainda.");
    }

    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("=== Migrator executado com sucesso ===");
    Console.ResetColor();
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERRO ao executar migrations:");
    Console.WriteLine($"  {ex.Message}");
    Console.ResetColor();
    
    if (ex.InnerException != null)
    {
        Console.WriteLine();
        Console.WriteLine("Erro interno:");
        Console.WriteLine($"  {ex.InnerException.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("Stack trace:");
    Console.WriteLine(ex.StackTrace);
    
    return 1;
}

/// <summary>
/// Extrai o host da connection string para exibição.
/// </summary>
static string ExtractHostFromConnectionString(string connectionString)
{
    try
    {
        var parts = connectionString.Split(';');
        var hostPart = parts.FirstOrDefault(p => p.TrimStart().StartsWith("Host=", StringComparison.OrdinalIgnoreCase));
        if (hostPart != null)
        {
            return hostPart.Split('=')[1].Trim();
        }
    }
    catch
    {
        // Ignorar erros ao extrair host
    }
    
    return "N/A";
}

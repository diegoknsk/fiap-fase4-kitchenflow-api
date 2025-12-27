var builder = WebApplication.CreateBuilder(args);

// Verificar se ASPNETCORE_URLS contém apenas HTTP (sem HTTPS)
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? builder.Configuration["Urls"] ?? "";
var isHttpOnly = !string.IsNullOrEmpty(urls) && 
                 urls.Split(';').All(u => u.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase));

// Quando for HTTP-only, configurar o Kestrel explicitamente para usar apenas HTTP
// Isso garante que não haja tentativa de usar HTTPS, mesmo que outras configurações sugiram isso
if (isHttpOnly)
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Extrair a porta da primeira URL (padrão 80 se não especificado)
        var firstUrl = urls.Split(';').FirstOrDefault()?.Trim() ?? "http://+:80";
        var port = 80;
        
        // Extrair porta da URL (formato: http://+:80 ou http://*:80)
        var lastColonIndex = firstUrl.LastIndexOf(':');
        if (lastColonIndex > 0 && lastColonIndex < firstUrl.Length - 1)
        {
            var portStr = firstUrl.Substring(lastColonIndex + 1);
            if (int.TryParse(portStr, out var parsedPort))
            {
                port = parsedPort;
            }
        }
        
        // Configurar apenas HTTP na porta especificada (sem HTTPS)
        options.ListenAnyIP(port);
    });
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FastFood.KitchenFlow.Api",
        Version = "v1"
    });
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FastFood.KitchenFlow.Api v1");
    c.RoutePrefix = "swagger";
});

// Apenas redirecionar HTTPS se estiver em desenvolvimento E não for HTTP-only
if (app.Environment.IsDevelopment() && !isHttpOnly)
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

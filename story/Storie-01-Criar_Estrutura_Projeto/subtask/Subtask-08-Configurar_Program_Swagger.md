# Subtask 08: Configurar Program.cs da API com Swagger

## Descrição
Configurar o `Program.cs` da API para habilitar Swagger/OpenAPI, configurar CORS básico (se necessário), e garantir que a API está pronta para desenvolvimento. Esta configuração estabelece a base para documentação e testes da API.

## Passos de implementação
- Abrir `Program.cs` da API
- Garantir que Swagger está habilitado:
  - `builder.Services.AddEndpointsApiExplorer()`
  - `builder.Services.AddSwaggerGen()`
  - `app.UseSwagger()` e `app.UseSwaggerUI()` no pipeline
- Configurar controllers:
  - `builder.Services.AddControllers()`
  - `app.MapControllers()` no pipeline
- Configurar JSON options (se necessário):
  - `builder.Services.ConfigureHttpJsonOptions(...)`
- Configurar portas e URLs no `appsettings.json`:
  - HTTP: `http://localhost:5000`
  - HTTPS: `https://localhost:5001`
- Remover código de exemplo desnecessário (WeatherForecast, etc.)

## Como testar
- Executar `dotnet run` no projeto Api
- Acessar `https://localhost:5001/swagger` (ou porta configurada)
- Verificar que Swagger UI abre e mostra o endpoint `/api/health`
- Testar o endpoint através do Swagger
- Verificar que não há erros no console

## Critérios de aceite
- Swagger habilitado e acessível
- Swagger UI mostra o endpoint `/api/health`
- Controllers configurados corretamente
- API inicia sem erros
- Endpoint pode ser testado através do Swagger
- Código de exemplo removido
- `appsettings.json` configurado com portas adequadas


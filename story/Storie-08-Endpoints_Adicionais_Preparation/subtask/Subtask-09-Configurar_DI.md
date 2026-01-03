# Subtask 09: Configurar Dependency Injection

## Descrição
Configurar Dependency Injection no `Program.cs` da API para registrar os novos UseCases de Preparation.

## Passos de implementação
- Abrir `Program.cs` do projeto `FastFood.KitchenFlow.Api`
- Adicionar registros de DI:
  - `builder.Services.AddScoped<GetPreparationsUseCase>();`
  - `builder.Services.AddScoped<StartPreparationUseCase>();`
  - `builder.Services.AddScoped<FinishPreparationUseCase>();`
- Verificar que `IPreparationRepository` já está registrado (da Story 06)
- Verificar que `CreatePreparationUseCase` já está registrado (da Story 06)
- Verificar que `KitchenFlowDbContext` já está registrado (da Story 04)
- Adicionar `using` statements necessários:
  - `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`

## Referências
- **OrderHub**: Verificar configuração de DI
- **Auth**: Verificar configuração de DI
- Seguir padrão de lifetime (Scoped para repositories, Transient ou Scoped para UseCases)

## Como testar
- Executar `dotnet build` na API
- Executar `dotnet run` e verificar que inicia sem erros
- Verificar que dependências são resolvidas corretamente

## Critérios de aceite
- UseCases registrados:
  - `GetPreparationsUseCase`
  - `StartPreparationUseCase`
  - `FinishPreparationUseCase`
- `IPreparationRepository` já registrado (da Story 06)
- `CreatePreparationUseCase` já registrado (da Story 06)
- `KitchenFlowDbContext` já registrado (da Story 04)
- API compila sem erros
- API inicia sem erros
- Dependências são resolvidas corretamente
- Configuração segue padrão do OrderHub/Auth

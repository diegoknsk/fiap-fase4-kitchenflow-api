# Subtask 07: Configurar Dependency Injection

## Descrição
Configurar Dependency Injection no `Program.cs` da API para registrar o Repository, UseCase e outras dependências necessárias.

## Passos de implementação
- Abrir `Program.cs` do projeto `FastFood.KitchenFlow.Api`
- Adicionar registros de DI:
  - `builder.Services.AddScoped<IPreparationRepository, PreparationRepository>();`
  - `builder.Services.AddScoped<CreatePreparationUseCase>();`
- Verificar que `KitchenFlowDbContext` já está registrado (da Story 04)
- Adicionar `using` statements necessários:
  - `FastFood.KitchenFlow.Application.Ports`
  - `FastFood.KitchenFlow.Application.UseCases.PreparationManagement`
  - `FastFood.KitchenFlow.Infra.Persistence.Repositories`

## Referências
- **OrderHub**: Verificar configuração de DI
- **Auth**: Verificar configuração de DI
- Seguir padrão de lifetime (Scoped para repositories, Transient ou Scoped para UseCases)

## Como testar
- Executar `dotnet build` na API
- Executar `dotnet run` e verificar que inicia sem erros
- Verificar que dependências são resolvidas corretamente

## Critérios de aceite
- `IPreparationRepository` registrado como Scoped
- `PreparationRepository` implementa `IPreparationRepository`
- `CreatePreparationUseCase` registrado
- `KitchenFlowDbContext` já registrado (da Story 04)
- API compila sem erros
- API inicia sem erros
- Dependências são resolvidas corretamente
- Configuração segue padrão do OrderHub/Auth

# Subtask 02: Criar estrutura de pastas para testes

## Descrição
Criar a estrutura completa de pastas no projeto `FastFood.KitchenFlow.Tests.Unit` que espelha a estrutura do código de produção, facilitando a localização e manutenção dos testes.

## Passos de implementação
- Criar estrutura de pastas no projeto `FastFood.KitchenFlow.Tests.Unit`:
  - `Application/UseCases/PreparationManagement/`
  - `Application/UseCases/DeliveryManagement/`
  - `InterfacesExternas/Controllers/`
- Verificar que a estrutura espelha o código de produção:
  - `Application/UseCases/` → `src/Core/FastFood.KitchenFlow.Application/UseCases/`
  - `InterfacesExternas/Controllers/` → `src/InterfacesExternas/FastFood.KitchenFlow.Api/Controllers/`
- Manter estrutura existente de `Domain/Entities/` (já criada na Story 03)

## Estrutura de pastas esperada

```
FastFood.KitchenFlow.Tests.Unit/
├── Domain/
│   └── Entities/
│       ├── DeliveryManagement/
│       │   └── DeliveryTests.cs (já existe)
│       └── PreparationManagement/
│           └── PreparationTests.cs (já existe)
├── Application/
│   └── UseCases/
│       ├── PreparationManagement/
│       │   ├── CreatePreparationUseCaseTests.cs
│       │   ├── GetPreparationsUseCaseTests.cs
│       │   ├── StartPreparationUseCaseTests.cs
│       │   └── FinishPreparationUseCaseTests.cs
│       └── DeliveryManagement/
│           ├── CreateDeliveryUseCaseTests.cs
│           ├── GetReadyDeliveriesUseCaseTests.cs
│           └── FinalizeDeliveryUseCaseTests.cs
└── InterfacesExternas/
    └── Controllers/
        ├── PreparationControllerTests.cs
        └── DeliveryControllerTests.cs
```

## Observações importantes
- **Espelhar estrutura**: A estrutura de testes deve espelhar a estrutura do código de produção
- **Manter organização**: Cada camada (Domain, Application, InterfacesExternas) em sua própria pasta
- **UseCases agrupados**: UseCases agrupados por domínio (PreparationManagement, DeliveryManagement)
- **Controllers separados**: Cada Controller tem seu próprio arquivo de teste

## Como testar
- Verificar que todas as pastas foram criadas
- Verificar que a estrutura espelha o código de produção
- Executar `dotnet build` para garantir que não há erros

## Critérios de aceite
- Estrutura de pastas criada conforme especificado
- Pastas `Application/UseCases/PreparationManagement/` criadas
- Pastas `Application/UseCases/DeliveryManagement/` criadas
- Pastas `InterfacesExternas/Controllers/` criadas
- Estrutura espelha código de produção
- `dotnet build` executa sem erros

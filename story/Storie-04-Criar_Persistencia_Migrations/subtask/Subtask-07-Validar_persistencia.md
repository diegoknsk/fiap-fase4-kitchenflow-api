# Subtask 07: Validar estrutura de persistência

## Descrição
Validar que toda a estrutura de persistência está correta, compila sem erros, segue os padrões arquiteturais e está pronta para uso.

## Passos de implementação
- Executar `dotnet build` na solução completa para verificar compilação
- Verificar estrutura de pastas no projeto `Infra.Persistence`:
  - `Entities/` com `PreparationEntity.cs` e `DeliveryEntity.cs`
  - `Configurations/` com `PreparationConfiguration.cs` e `DeliveryConfiguration.cs`
  - `KitchenFlowDbContext.cs` na raiz
  - `Migrations/` com migration inicial
- Verificar namespaces:
  - `FastFood.KitchenFlow.Infra.Persistence.Entities`
  - `FastFood.KitchenFlow.Infra.Persistence.Configurations`
  - `FastFood.KitchenFlow.Infra.Persistence`
- Verificar que DbContext está configurado no `Program.cs` da API
- Verificar que connection string está configurada em `appsettings.json`
- Validar que entidades de persistência são separadas das entidades de domínio
- Verificar que configurações mapeiam corretamente:
  - `OrderSnapshot` como `jsonb`
  - FK `Delivery.PreparationId` → `Preparation.Id`
  - Índices criados
- Executar análise estática (se disponível) para verificar code smells

## Como testar
- Executar `dotnet build` na solução completa
- Executar `dotnet test` (se houver testes de persistência)
- Verificar logs de compilação para warnings
- Validar estrutura de arquivos

## Critérios de aceite
- Solução completa compila sem erros (`dotnet build` executa com sucesso)
- Estrutura de pastas está correta:
  - `Entities/` com entidades de persistência
  - `Configurations/` com configurações de mapeamento
  - `Migrations/` com migration inicial
  - `KitchenFlowDbContext.cs` na raiz
- Namespaces estão corretos
- Entidades de persistência são separadas das entidades de domínio
- Configurações mapeiam corretamente:
  - `OrderSnapshot` como `jsonb`
  - FK configurada corretamente
  - Índices criados
- DbContext configurado no `Program.cs` da API
- Connection string configurada em `appsettings.json`
- Migration inicial criada e compila sem erros
- Estrutura segue padrão do Auth
- Código está limpo, sem code smells óbvios

## Checklist de Validação
- [ ] `dotnet build` executa sem erros
- [ ] Estrutura de pastas está correta
- [ ] Namespaces estão corretos
- [ ] Entidades de persistência criadas
- [ ] Configurações de mapeamento criadas
- [ ] DbContext criado e configurado
- [ ] Migration inicial criada
- [ ] Connection string configurada
- [ ] Código segue padrão arquitetural
- [ ] Separação entre domínio e persistência mantida

## Observações
- **Validação final**: Esta subtask serve como validação final antes de prosseguir para a próxima story
- **Próxima story**: Ajustar o projeto Migrator para executar as migrations criadas
- **Documentação**: Se necessário, adicionar comentários XML nas classes principais

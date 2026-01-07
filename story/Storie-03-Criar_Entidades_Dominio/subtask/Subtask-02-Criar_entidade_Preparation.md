# Subtask 02: Criar entidade Preparation com validações

## Descrição
Criar a entidade de domínio `Preparation` no projeto `FastFood.KitchenFlow.Domain` com todas as propriedades, métodos de negócio e validações necessárias. A entidade deve seguir o padrão de entidades ricas do OrderHub e adaptar a lógica do monolito para o novo modelo com `OrderSnapshot`.

## Passos de implementação
- Criar classe `Preparation` em `Entities/PreparationManagement/Preparation.cs`
- Implementar propriedades:
  - `Id` (Guid) - chave primária, privado setter
  - `OrderId` (Guid) - ID do pedido, privado setter
  - `Status` (EnumPreparationStatus) - status atual, privado setter
  - `CreatedAt` (DateTime) - data/hora de criação, privado setter
  - `OrderSnapshot` (string) - snapshot JSON imutável do pedido, privado setter
- Implementar construtor privado sem parâmetros (para EF Core, se necessário)
- Implementar factory method estático `Create(Guid orderId, string orderSnapshot)`:
  - Validar que `orderId` não é vazio
  - Validar que `orderSnapshot` não é nulo ou vazio
  - Criar nova instância com `Id = Guid.NewGuid()`
  - Definir `Status = EnumPreparationStatus.Received`
  - Definir `CreatedAt = DateTime.UtcNow`
  - Retornar instância de Preparation
- Implementar método `StartPreparation()`:
  - Validar que status atual é `Received`
  - Lançar exceção se status não permitir transição
  - Alterar status para `InProgress`
- Implementar método `FinishPreparation()`:
  - Validar que status atual é `InProgress`
  - Lançar exceção se status não permitir transição
  - Alterar status para `Finished`
- Adicionar validações usando padrão similar ao monolito (DomainValidation ou exceções diretas)
- Referenciar enum `EnumPreparationStatus` criado na Subtask 01
- Adicionar documentação XML na classe e métodos (opcional, mas recomendado)

## Referências
- **Monolito**: `c:\Projetos\Fiap\fiap-fase3-aplicacao\fiap-fastfood\02-Core\FastFood.Domain\Entities\PreparationManagement\Preparation.cs`
- **OrderHub**: `c:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Core\FastFood.OrderHub.Domain\Entities\OrderManagement\Order.cs`
- Adaptar padrão de validações e métodos de negócio

## Como testar
- **Testes unitários** (serão criados na Subtask 05):
  - Teste de criação via `Create()` com parâmetros válidos
  - Teste de criação com `orderId` vazio (deve lançar exceção)
  - Teste de criação com `orderSnapshot` nulo/vazio (deve lançar exceção)
  - Teste de transição `Received` → `InProgress` (válida)
  - Teste de transição `InProgress` → `Finished` (válida)
  - Teste de transição inválida `Received` → `Finished` (deve lançar exceção)
  - Teste de transição inválida `Finished` → `InProgress` (deve lançar exceção)
- **Compilação**: Executar `dotnet build` no projeto Domain
- **Validação manual**: Verificar que a entidade pode ser instanciada e métodos funcionam

## Critérios de aceite
- Classe `Preparation` criada em `Entities/PreparationManagement/Preparation.cs`
- Propriedades implementadas com setters privados:
  - `Id` (Guid)
  - `OrderId` (Guid)
  - `Status` (EnumPreparationStatus)
  - `CreatedAt` (DateTime)
  - `OrderSnapshot` (string)
- Factory method `Create()` implementado com validações
- Método `StartPreparation()` implementado:
  - Valida transição `Received` → `InProgress`
  - Lança exceção para transições inválidas
- Método `FinishPreparation()` implementado:
  - Valida transição `InProgress` → `Finished`
  - Lança exceção para transições inválidas
- Validações de negócio implementadas (orderId não vazio, orderSnapshot não nulo/vazio)
- Projeto Domain compila sem erros
- Nenhuma dependência externa (EF Core, ASP.NET, etc.)
- Código segue padrão do OrderHub (entidades ricas com métodos de negócio)
- Namespace: `FastFood.KitchenFlow.Domain.Entities.PreparationManagement`

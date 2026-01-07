# Subtask 01: Criar DTOs do OrderSnapshot

## Descrição
Criar classes DTO (Data Transfer Object) para representar a estrutura do `OrderSnapshot` que será recebido do Payment e armazenado como JSON no banco de dados.

## Passos de implementação
- Criar pasta `DTOs/` no projeto `FastFood.KitchenFlow.Application` (se não existir)
- Criar classe `OrderSnapshotDto` em `DTOs/OrderSnapshotDto.cs`:
  - Propriedades:
    - `OrderId` (Guid) - ID do pedido
    - `OrderCode` (string?) - Código do pedido (opcional)
    - `CustomerId` (Guid?) - ID do cliente (opcional)
    - `TotalPrice` (decimal) - Valor total do pedido
    - `CreatedAt` (DateTime) - Data/hora de criação do pedido
    - `Items` (List<OrderItemSnapshotDto>) - Lista de itens
    - `PaymentId` (Guid) - ID do pagamento
    - `PaidAt` (DateTime) - Data/hora do pagamento
  - Adicionar atributos de validação (Data Annotations) se necessário
  - Adicionar documentação XML
- Criar classe `OrderItemSnapshotDto` em `DTOs/OrderItemSnapshotDto.cs`:
  - Propriedades:
    - `ProductId` (Guid) - ID do produto
    - `ProductName` (string) - Nome do produto
    - `Quantity` (int) - Quantidade
    - `UnitPrice` (decimal) - Preço unitário
    - `FinalPrice` (decimal) - Preço final (com ingredientes)
    - `Ingredients` (List<OrderIngredientSnapshotDto>?) - Ingredientes (opcional)
    - `Observation` (string?) - Observações (opcional)
  - Adicionar documentação XML
- Criar classe `OrderIngredientSnapshotDto` em `DTOs/OrderIngredientSnapshotDto.cs`:
  - Propriedades:
    - `Name` (string) - Nome do ingrediente
    - `Price` (decimal) - Preço do ingrediente
    - `Quantity` (int) - Quantidade
  - Adicionar documentação XML
- Namespace: `FastFood.KitchenFlow.Application.DTOs`

## Referências
- **OrderHub**: `c:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Core\FastFood.OrderHub.Application\DTOs\`
  - `OrderDto.cs`
  - `OrderedProductDto.cs`
  - `OrderedProductIngredientDto.cs`
- Adaptar estrutura do OrderHub para OrderSnapshot (snapshot imutável)

## Observações importantes
- **DTOs são para validação**: Usados para validar estrutura do JSON antes de armazenar como string
- **OrderSnapshot no domínio**: Continua sendo string, DTOs são apenas para validação/deserialização
- **Compatibilidade**: Estrutura deve ser compatível com OrderHub para facilitar integração

## Como testar
- Executar `dotnet build` no projeto Application
- Criar teste unitário básico de serialização/deserialização JSON
- Verificar que DTOs podem ser instanciados

## Critérios de aceite
- Classe `OrderSnapshotDto` criada:
  - Propriedades: OrderId, OrderCode, CustomerId, TotalPrice, CreatedAt, Items, PaymentId, PaidAt
  - Lista de Items (OrderItemSnapshotDto)
- Classe `OrderItemSnapshotDto` criada:
  - Propriedades: ProductId, ProductName, Quantity, UnitPrice, FinalPrice, Ingredients, Observation
  - Lista de Ingredients (OrderIngredientSnapshotDto, opcional)
- Classe `OrderIngredientSnapshotDto` criada:
  - Propriedades: Name, Price, Quantity
- Projeto Application compila sem erros
- Namespace correto: `FastFood.KitchenFlow.Application.DTOs`
- DTOs seguem estrutura do OrderHub (adaptada para snapshot)

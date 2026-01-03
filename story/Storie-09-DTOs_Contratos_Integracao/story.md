# Storie-09: Criar DTOs e Contratos de Integração Payment → KitchenFlow

## Descrição
Como desenvolvedor, quero criar DTOs (Data Transfer Objects) e documentar os contratos de integração entre o microserviço Payment e o KitchenFlow, incluindo a estrutura do `OrderSnapshot` e validações, para garantir que a comunicação entre os serviços seja clara, consistente e bem documentada.

## Objetivo
Criar DTOs e documentar contratos de integração para:
- Definir estrutura do `OrderSnapshot` (JSON do pedido)
- Documentar contrato do endpoint `POST /api/preparations`
- Criar classes DTO para validação e deserialização do `OrderSnapshot`
- Documentar exemplos de payload e respostas
- Garantir que Payment e KitchenFlow usem o mesmo contrato

## Escopo Técnico
- **Tecnologias**: .NET 8, JSON Serialization
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Application` (DTOs, contratos)
  - `FastFood.KitchenFlow.Api` (validações, documentação)
  - Documentação (README ou arquivo de contrato)
- **Referências de arquitetura**:
  - `c:\Projetos\Fiap\fiap-fase4-orderhub-api` (estrutura de pedidos)
  - Verificar estrutura de Order no OrderHub para definir OrderSnapshot

## Contrato de Integração

### Endpoint: POST /api/preparations

**Chamado por**: Microserviço Payment  
**Quando**: Após confirmação de pagamento aprovado  
**Método**: HTTP POST  
**URL**: `{kitchenflow-url}/api/preparations`

### Request Body

```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "orderSnapshot": {
    "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "orderCode": "ORD-2024-001",
    "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa7",
    "totalPrice": 45.50,
    "createdAt": "2024-01-01T10:00:00Z",
    "items": [
      {
        "productId": "3fa85f64-5717-4562-b3fc-2c963f66afa8",
        "productName": "Hambúrguer Clássico",
        "quantity": 2,
        "unitPrice": 15.00,
        "finalPrice": 30.00,
        "ingredients": [
          {
            "name": "Pão",
            "price": 0.00,
            "quantity": 1
          }
        ],
        "observation": "Sem cebola"
      }
    ],
    "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa9",
    "paidAt": "2024-01-01T10:05:00Z"
  }
}
```

### Response 201 Created

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afaa",
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": 0,
  "createdAt": "2024-01-01T10:05:30Z",
  "message": "Preparação criada com sucesso"
}
```

### Response 400 Bad Request

```json
{
  "errors": {
    "orderId": ["OrderId é obrigatório"],
    "orderSnapshot": ["OrderSnapshot é obrigatório"]
  },
  "message": "Dados inválidos"
}
```

### Response 409 Conflict

```json
{
  "message": "Preparação já existe para este pedido",
  "preparationId": "3fa85f64-5717-4562-b3fc-2c963f66afaa"
}
```

## Estrutura do OrderSnapshot

O `OrderSnapshot` é um JSON que representa o estado completo do pedido no momento do pagamento. Deve conter todas as informações necessárias para:
- Exibição no display da cozinha
- Referência para entrega
- Auditoria

### Campos Obrigatórios
- `orderId` (Guid/string) - ID do pedido
- `orderCode` (string, opcional) - Código do pedido
- `customerId` (Guid/string, opcional) - ID do cliente
- `totalPrice` (decimal) - Valor total do pedido
- `createdAt` (DateTime) - Data/hora de criação do pedido
- `items` (array) - Lista de itens do pedido
- `paymentId` (Guid/string) - ID do pagamento
- `paidAt` (DateTime) - Data/hora do pagamento

### Estrutura de Item
- `productId` (Guid/string) - ID do produto
- `productName` (string) - Nome do produto
- `quantity` (int) - Quantidade
- `unitPrice` (decimal) - Preço unitário
- `finalPrice` (decimal) - Preço final (com ingredientes)
- `ingredients` (array, opcional) - Ingredientes adicionais/removidos
- `observation` (string, opcional) - Observações do cliente

## Subtasks

- [ ] [Subtask 01: Criar DTOs do OrderSnapshot](./subtask/Subtask-01-Criar_DTOs_OrderSnapshot.md)
- [ ] [Subtask 02: Criar validações para OrderSnapshot](./subtask/Subtask-02-Criar_validacoes.md)
- [ ] [Subtask 03: Documentar contrato de integração](./subtask/Subtask-03-Documentar_contrato.md)
- [ ] [Subtask 04: Criar exemplos de payload](./subtask/Subtask-04-Criar_exemplos.md)
- [ ] [Subtask 05: Validar estrutura completa](./subtask/Subtask-05-Validar_estrutura.md)

## Critérios de Aceite da História

- [ ] DTOs criados para OrderSnapshot:
  - `OrderSnapshotDto` - estrutura principal
  - `OrderItemSnapshotDto` - item do pedido
  - `OrderIngredientSnapshotDto` - ingrediente do item
- [ ] Validações implementadas:
  - Campos obrigatórios validados
  - Tipos de dados validados
  - Estrutura JSON validada
- [ ] Documentação criada:
  - Contrato de integração documentado
  - Exemplos de request/response
  - Estrutura do OrderSnapshot documentada
- [ ] Exemplos de payload criados:
  - Exemplo completo
  - Exemplo mínimo
  - Exemplos de erro
- [ ] Estrutura validada:
  - DTOs podem ser serializados/deserializados
  - Validações funcionam corretamente
  - Documentação está completa

## Observações Arquiteturais

### DTOs vs Entidades de Domínio
- **DTOs**: Representam dados de integração, podem ter validações de formato
- **Entidades de Domínio**: Representam conceitos de negócio, têm regras de negócio
- **OrderSnapshot**: Armazenado como string JSON no domínio, mas pode ser validado/deserializado usando DTOs

### Validação do OrderSnapshot
- **Na API**: Validar formato JSON e estrutura básica
- **No UseCase**: Validar que OrderSnapshot não é nulo/vazio
- **No Domínio**: OrderSnapshot é tratado como string imutável

### Contrato de Integração
- **Versionamento**: Considerar versionamento futuro do contrato
- **Compatibilidade**: Manter compatibilidade com versões anteriores
- **Documentação**: Swagger deve documentar o contrato

### Análise dos Projetos de Referência

**OrderHub (estrutura de Order):**
- Verificar estrutura de Order e OrderedProduct no OrderHub
- Adaptar para OrderSnapshot (snapshot imutável)

**Aplicação no KitchenFlow:**
- DTOs na Application para validação
- OrderSnapshot armazenado como string no domínio
- Validação na camada de API/Application

### Estrutura do OrderSnapshot (Baseada no OrderHub)

Baseado na estrutura do OrderHub, o OrderSnapshot deve conter:
- Informações do pedido (Id, Code, CustomerId, TotalPrice, CreatedAt)
- Lista de itens (ProductId, ProductName, Quantity, UnitPrice, FinalPrice)
- Ingredientes de cada item (Name, Price, Quantity)
- Informações de pagamento (PaymentId, PaidAt)

# Contrato de Integração Payment → KitchenFlow

## Visão Geral

Este documento descreve o contrato de integração entre o microserviço **Payment** e o microserviço **KitchenFlow** para criação de preparações após confirmação de pagamento.

## Endpoint

### POST /api/preparations

**Chamado por**: Microserviço Payment  
**Quando**: Após confirmação de pagamento aprovado  
**Método**: HTTP POST  
**URL**: `{kitchenflow-url}/api/preparations`

### Descrição do Fluxo

1. Cliente realiza pedido no OrderHub
2. OrderHub cria pedido e publica mensagem na SQS
3. Payment consome mensagem e processa pagamento
4. Após pagamento aprovado, Payment chama este endpoint
5. KitchenFlow cria preparação e retorna resposta

## Request

### Headers

```
Content-Type: application/json
```

### Body

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

### Campos do Request

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `orderId` | Guid (string) | Sim | Identificador único do pedido |
| `orderSnapshot` | Object (JSON) | Sim | Snapshot completo do pedido no momento do pagamento |

### Estrutura do OrderSnapshot

O `OrderSnapshot` é um JSON que representa o estado completo do pedido no momento do pagamento. Deve conter todas as informações necessárias para:
- Exibição no display da cozinha
- Referência para entrega
- Auditoria

#### Campos Obrigatórios do OrderSnapshot

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `orderId` | Guid (string) | ID do pedido (deve corresponder ao `orderId` do request) |
| `totalPrice` | decimal | Valor total do pedido |
| `createdAt` | DateTime (ISO 8601) | Data/hora de criação do pedido |
| `items` | Array | Lista de itens do pedido (mínimo 1 item) |
| `paymentId` | Guid (string) | ID do pagamento |
| `paidAt` | DateTime (ISO 8601) | Data/hora do pagamento |

#### Campos Opcionais do OrderSnapshot

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `orderCode` | string | Código do pedido (ex: "ORD-2024-001") |
| `customerId` | Guid (string) | ID do cliente |

#### Estrutura de Item (OrderItemSnapshotDto)

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `productId` | Guid (string) | Sim | ID do produto |
| `productName` | string | Sim | Nome do produto |
| `quantity` | int | Sim | Quantidade (deve ser > 0) |
| `unitPrice` | decimal | Sim | Preço unitário (deve ser >= 0) |
| `finalPrice` | decimal | Sim | Preço final com ingredientes (deve ser >= 0) |
| `ingredients` | Array | Não | Lista de ingredientes customizados |
| `observation` | string | Não | Observações do cliente sobre o item |

#### Estrutura de Ingrediente (OrderIngredientSnapshotDto)

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `name` | string | Sim | Nome do ingrediente |
| `price` | decimal | Sim | Preço do ingrediente (deve ser >= 0) |
| `quantity` | int | Sim | Quantidade (deve ser > 0) |

## Response

### 201 Created

Retornado quando a preparação é criada com sucesso.

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afaa",
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": 0,
  "createdAt": "2024-01-01T10:05:30Z",
  "message": "Preparação criada com sucesso"
}
```

**Campos do Response:**

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `id` | Guid (string) | Identificador único da preparação |
| `orderId` | Guid (string) | ID do pedido |
| `status` | int | Status da preparação (0=Received, 1=InProgress, 2=Finished) |
| `createdAt` | DateTime (ISO 8601) | Data/hora de criação da preparação |
| `message` | string | Mensagem de sucesso |

### 400 Bad Request

Retornado quando os dados do request são inválidos.

```json
{
  "errors": {
    "orderId": ["OrderId é obrigatório"],
    "orderSnapshot": ["OrderSnapshot é obrigatório"]
  },
  "message": "Dados inválidos"
}
```

**Possíveis causas:**
- Campos obrigatórios ausentes
- Tipos de dados incorretos
- JSON malformado no OrderSnapshot
- OrderId do OrderSnapshot não corresponde ao OrderId do request
- Validações de negócio falhadas (ex: quantidade <= 0, preço < 0)

### 409 Conflict

Retornado quando já existe uma preparação para o OrderId informado (idempotência).

```json
{
  "message": "Preparação já existe para este pedido",
  "preparationId": "3fa85f64-5717-4562-b3fc-2c963f66afaa",
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Comportamento de Idempotência:**
- Se o Payment chamar o endpoint múltiplas vezes com o mesmo `orderId`, o KitchenFlow retornará 409 Conflict após a primeira chamada bem-sucedida
- Isso garante que não serão criadas preparações duplicadas

### 500 Internal Server Error

Retornado quando ocorre um erro interno no servidor.

```json
{
  "message": "Erro interno do servidor.",
  "error": "Detalhes do erro (apenas em ambiente de desenvolvimento)"
}
```

## Regras de Negócio

### Quando Chamar o Endpoint

- **Apenas após pagamento aprovado**: O endpoint deve ser chamado somente quando o pagamento foi confirmado e aprovado
- **Uma vez por pedido**: Idealmente, o endpoint deve ser chamado uma vez por pedido, mas o sistema suporta idempotência

### Validações Esperadas

1. **OrderId**: Deve ser um GUID válido e não vazio
2. **OrderSnapshot**: Deve ser um JSON válido e deserializável
3. **Correspondência de IDs**: O `orderId` do request deve corresponder ao `orderId` dentro do `orderSnapshot`
4. **Campos obrigatórios**: Todos os campos obrigatórios do OrderSnapshot devem estar presentes
5. **Tipos de dados**: Tipos devem estar corretos (Guid, decimal, DateTime, int, string)
6. **Valores válidos**: 
   - Quantidades devem ser > 0
   - Preços devem ser >= 0
   - Lista de items deve conter pelo menos 1 item

### Comportamento de Idempotência

- Se uma preparação já existe para o `orderId` informado, o sistema retorna **409 Conflict**
- O Payment pode chamar o endpoint múltiplas vezes sem criar duplicatas
- O `orderId` é usado como chave única para verificação de idempotência

## Exemplos de Integração

### Exemplo de Chamada HTTP

```http
POST /api/preparations HTTP/1.1
Host: kitchenflow-api.example.com
Content-Type: application/json

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

### Exemplo com cURL

```bash
curl -X POST https://kitchenflow-api.example.com/api/preparations \
  -H "Content-Type: application/json" \
  -d @preparation-request.json
```

## Diagrama de Sequência

```
Payment                    KitchenFlow
  |                              |
  |-- POST /api/preparations -->|
  |     (OrderId + Snapshot)     |
  |                              |
  |                              |-- Validar OrderSnapshot
  |                              |-- Verificar idempotência
  |                              |-- Criar Preparation
  |                              |
  |<-- 201 Created --------------|
  |     (Preparation ID)         |
  |                              |
```

## Versionamento

### Versão Atual: v1

- **Endpoint**: `/api/preparations`
- **Formato**: JSON
- **Data de Criação**: 2024-01-01

### Considerações para Versões Futuras

- Adicionar suporte a versionamento via header: `X-API-Version: v1`
- Manter compatibilidade com versões anteriores
- Documentar breaking changes claramente

## Referências

- **Exemplos de Payload**: Ver arquivos em `docs/examples/`
- **Swagger**: Documentação interativa disponível em `/swagger` (ambiente de desenvolvimento)
- **DTOs**: Classes DTOs definidas em `FastFood.KitchenFlow.Application.DTOs`

## Suporte

Para dúvidas ou problemas relacionados à integração, entre em contato com o time de desenvolvimento do KitchenFlow.

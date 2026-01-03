# Subtask 04: Criar exemplos de payload

## Descrição
Criar arquivos de exemplo com payloads JSON para facilitar testes e integração do Payment com KitchenFlow.

## Passos de implementação
- Criar pasta `docs/examples/` (se não existir)
- Criar arquivo `docs/examples/preparation-request-complete.json`:
  - Exemplo completo de request com todos os campos
  - OrderSnapshot completo com múltiplos itens
  - Ingredientes customizados
  - Observações
- Criar arquivo `docs/examples/preparation-request-minimal.json`:
  - Exemplo mínimo de request (apenas campos obrigatórios)
  - OrderSnapshot com um item simples
- Criar arquivo `docs/examples/preparation-response-success.json`:
  - Exemplo de response 201 Created
- Criar arquivo `docs/examples/preparation-response-error.json`:
  - Exemplo de response 400 Bad Request
  - Exemplo de response 409 Conflict
- Adicionar comentários nos arquivos JSON explicando cada campo
- Referenciar exemplos na documentação principal

## Referências
- **Estrutura do OrderHub**: Usar estrutura real de pedidos como base
- **Exemplos reais**: Criar exemplos que façam sentido no contexto do negócio

## Observações importantes
- **Exemplos válidos**: Todos os exemplos devem ser JSON válidos
- **Exemplos realistas**: Usar dados que façam sentido no contexto
- **Comentários**: Adicionar comentários explicativos quando necessário

## Como testar
- Validar que arquivos JSON são válidos
- Testar deserialização dos exemplos
- Verificar que exemplos estão referenciados na documentação

## Critérios de aceite
- Arquivo `preparation-request-complete.json` criado:
  - Exemplo completo com todos os campos
  - Múltiplos itens
  - Ingredientes customizados
- Arquivo `preparation-request-minimal.json` criado:
  - Exemplo mínimo (campos obrigatórios)
  - Um item simples
- Arquivo `preparation-response-success.json` criado:
  - Response 201 Created
- Arquivo `preparation-response-error.json` criado:
  - Responses de erro (400, 409)
- Arquivos JSON são válidos
- Exemplos estão referenciados na documentação
- Exemplos são realistas e úteis

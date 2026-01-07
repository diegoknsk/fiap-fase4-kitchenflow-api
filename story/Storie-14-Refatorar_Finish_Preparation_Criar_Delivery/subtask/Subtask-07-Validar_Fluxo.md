# Subtask 07: Validar fluxo completo

## Descrição
Validar que o fluxo completo está funcionando corretamente: finish de preparação cria delivery automaticamente, response contém DeliveryId, delivery aparece na listagem e pode ser finalizado.

## Passos de implementação
- Executar a aplicação e acessar Swagger
- Testar endpoint `POST /api/preparations/{id}/finish`:
  - Criar uma preparação (se necessário)
  - Iniciar a preparação (status → InProgress)
  - Finalizar a preparação
  - **Validar**: Response contém `deliveryId`
  - **Validar**: Status code 200 OK
  - **Validar**: Mensagem de sucesso
- Testar endpoint `GET /api/deliveries/ready`:
  - Listar deliveries prontas
  - **Validar**: O delivery criado aparece na listagem
  - **Validar**: Status do delivery é `ReadyForPickup` (1)
  - **Validar**: `PreparationId` está correto
- Testar endpoint `POST /api/deliveries/{id}/finalize`:
  - Usar o `deliveryId` retornado no finish
  - Finalizar o delivery
  - **Validar**: Status code 200 OK
  - **Validar**: Status do delivery é `Finalized` (2)
  - **Validar**: `FinalizedAt` está preenchido
- Testar idempotência:
  - Tentar finalizar a mesma preparação novamente (se possível)
  - **Validar**: Não cria delivery duplicado
  - **Validar**: Retorna o mesmo `DeliveryId`
- Verificar logs e erros:
  - Não deve haver erros no console
  - Não deve haver warnings relevantes
- Validar documentação Swagger:
  - Response do finish deve mostrar `deliveryId`
  - Endpoint `POST /api/deliveries` não deve mais aparecer
  - Endpoints de delivery restantes devem estar documentados

## Referências
- **Swagger**: Usar Swagger UI para testes manuais
- **Postman/Insomnia**: Alternativa para testes de API
- **Logs**: Verificar logs da aplicação para erros

## Observações importantes
- **Fluxo completo**: Testar todo o fluxo do início ao fim
- **Cenários de erro**: Testar também cenários de erro (preparation não encontrada, status inválido, etc.)
- **Idempotência**: Validar que não cria deliveries duplicados
- **Performance**: Verificar que não há impacto significativo na performance
- **Dados**: Verificar que os dados estão corretos em todas as etapas

## Checklist de Validação

- [ ] Finish de preparação cria delivery automaticamente
- [ ] Response do finish contém `deliveryId`
- [ ] Delivery aparece em `GET /api/deliveries/ready`
- [ ] Delivery tem status `ReadyForPickup` (1)
- [ ] Delivery pode ser finalizado via `POST /api/deliveries/{id}/finalize`
- [ ] Idempotência funciona (não cria duplicado)
- [ ] Endpoint `POST /api/deliveries` foi removido
- [ ] Swagger documenta corretamente os endpoints
- [ ] Não há erros nos logs
- [ ] Testes unitários passam
- [ ] Código segue padrões do projeto

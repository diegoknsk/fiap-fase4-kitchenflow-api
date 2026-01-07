# Subtask 05: Remover endpoint CreateDelivery do DeliveryController

## Descrição
Remover o endpoint `POST /api/deliveries` do `DeliveryController`, pois o delivery agora é criado automaticamente no finish da preparação.

## Passos de implementação
- Abrir classe `DeliveryController` em `Api/Controllers/DeliveryController.cs`
- Remover `CreateDeliveryUseCase` do construtor:
  - Remover parâmetro `CreateDeliveryUseCase createDeliveryUseCase`
  - Remover campo privado `_createDeliveryUseCase`
  - Remover atribuição no construtor
- Remover método `CreateDelivery`:
  - Remover todo o método `CreateDelivery` (linhas 37-66 aproximadamente)
  - Remover documentação XML do método
  - Remover atributos `[HttpPost]`, `[ProducesResponseType]`
- Verificar que os endpoints restantes estão funcionais:
  - `GET /api/deliveries/ready` - deve continuar funcionando
  - `POST /api/deliveries/{id}/finalize` - deve continuar funcionando
- Verificar que não há referências quebradas
- Atualizar documentação XML do controller (se necessário)

## Referências
- **Controller atual**: Verificar estrutura existente
- **Padrão**: Manter padrão dos outros Controllers do projeto
- **Swagger**: O endpoint será automaticamente removido da documentação Swagger

## Observações importantes
- **UseCase mantido**: O `CreateDeliveryUseCase` pode continuar registrado no DI se for usado internamente pelo `FinishPreparationUseCase`
- **Endpoints mantidos**: Não remover `GetReadyDeliveries` e `FinalizeDelivery`
- **Documentação**: Atualizar comentários do controller se necessário
- **Testes**: Os testes do endpoint removido devem ser removidos ou atualizados (ver Subtask 06)

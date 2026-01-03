# Subtask 03: Criar cenário BDD do fluxo completo

## Descrição
Criar cenário BDD (Behavior-Driven Development) usando SpecFlow que documenta e testa o fluxo completo do KitchenFlow, desde a criação de preparação até a finalização da entrega.

## Passos de implementação
- Verificar que SpecFlow está configurado no projeto `FastFood.KitchenFlow.Tests.Bdd`
- Criar arquivo de feature `KitchenFlowCompleteFlow.feature`:
  - Cenário: "Fluxo completo de preparação e entrega"
  - Steps em Gherkin:
    ```
    Dado que o Payment confirma um pagamento para o pedido "ORD-001"
    Quando o Payment chama POST /api/preparations com OrderId e OrderSnapshot
    Então uma Preparation é criada com status Received
    
    Dado que existe uma Preparation com status Received
    Quando o sistema chama POST /api/preparations/{id}/start
    Então a Preparation fica com status InProgress
    
    Dado que existe uma Preparation com status InProgress
    Quando o sistema chama POST /api/preparations/{id}/finish
    Então a Preparation fica com status Finished
    
    Dado que existe uma Preparation com status Finished
    Quando o sistema chama POST /api/deliveries com PreparationId
    Então uma Delivery é criada com status ReadyForPickup
    
    Dado que existe uma Delivery com status ReadyForPickup
    Quando o sistema chama POST /api/deliveries/{id}/finalize
    Então a Delivery fica com status Finalized
    ```
- Criar classe de steps `KitchenFlowCompleteFlowSteps.cs`:
  - Implementar steps do Gherkin
  - Usar banco de dados de teste
  - Fazer chamadas HTTP reais aos endpoints
  - Verificar respostas e dados no banco
- Configurar contexto compartilhado entre steps:
  - Armazenar IDs criados (PreparationId, DeliveryId)
  - Compartilhar dados entre steps
- Adicionar cenários de erro:
  - Preparação duplicada
  - Transições de status inválidas
  - Dados inválidos

## Referências
- **SpecFlow**: Documentação oficial do SpecFlow para .NET
- **BDD**: Padrão Given-When-Then

## Observações importantes
- **Gherkin claro**: Steps devem ser claros e legíveis
- **Implementação real**: Steps devem fazer chamadas HTTP reais
- **Contexto compartilhado**: Compartilhar dados entre steps do mesmo cenário

## Como testar
- Executar `dotnet test` no projeto BDD
- Verificar que cenário BDD passa
- Verificar que report do SpecFlow é gerado

## Critérios de aceite
- Arquivo `.feature` criado:
  - Cenário do fluxo completo documentado
  - Steps em Gherkin claros
- Classe de steps criada:
  - Todos os steps implementados
  - Steps fazem chamadas HTTP reais
  - Steps verificam resultados
- Cenário BDD passa:
  - `dotnet test` executa com sucesso
  - Fluxo completo é testado
- Report do SpecFlow é gerado
- Cenário está documentado e testável

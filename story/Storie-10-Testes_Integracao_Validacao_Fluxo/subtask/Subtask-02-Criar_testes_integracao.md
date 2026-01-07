# Subtask 02: Criar testes de integração dos endpoints

## Descrição
Criar testes de integração que testam os endpoints principais do KitchenFlow usando banco de dados real (ou em memória) e verificando que toda a camada funciona corretamente.

## Passos de implementação
- Criar pasta `Integration/` no projeto `FastFood.KitchenFlow.Tests.Unit` ou criar projeto separado
- Criar classe `PreparationControllerIntegrationTests.cs`:
  - Teste: `POST /api/preparations - ShouldCreatePreparationSuccessfully`
  - Teste: `POST /api/preparations - ShouldReturn409WhenPreparationExists`
  - Teste: `POST /api/preparations - ShouldReturn400WhenDataInvalid`
  - Teste: `GET /api/preparations - ShouldReturnPagedList`
  - Teste: `POST /api/preparations/{id}/start - ShouldStartPreparation`
  - Teste: `POST /api/preparations/{id}/finish - ShouldFinishPreparation`
- Criar classe `DeliveryControllerIntegrationTests.cs`:
  - Teste: `POST /api/deliveries - ShouldCreateDeliverySuccessfully`
  - Teste: `POST /api/deliveries - ShouldReturn400WhenPreparationNotFinished`
  - Teste: `GET /api/deliveries/ready - ShouldReturnReadyDeliveries`
  - Teste: `POST /api/deliveries/{id}/finalize - ShouldFinalizeDelivery`
- Usar `WebApplicationFactory` ou `TestServer` para testar API:
  - Configurar API com banco de teste
  - Fazer chamadas HTTP reais
  - Verificar respostas HTTP
- Verificar no banco de dados:
  - Dados foram persistidos corretamente
  - Status foi atualizado corretamente
- Organizar testes usando `[Fact]` ou `[Theory]`
- Adicionar nomes descritivos

## Referências
- **ASP.NET Core Testing**: `WebApplicationFactory` ou `TestServer`
- **xUnit**: Padrão de testes de integração

## Observações importantes
- **Testes reais**: Usar chamadas HTTP reais, não mocks
- **Banco de dados**: Usar banco de teste (Testcontainers ou local)
- **Isolamento**: Limpar dados entre testes

## Como testar
- Executar `dotnet test` no projeto de testes
- Verificar que todos os testes passam
- Verificar que banco de dados é usado corretamente

## Critérios de aceite
- Testes de integração criados:
  - PreparationControllerIntegrationTests com testes dos endpoints
  - DeliveryControllerIntegrationTests com testes dos endpoints
- Testes usam banco de dados real (ou em memória)
- Testes fazem chamadas HTTP reais
- Testes verificam respostas HTTP e dados no banco
- Todos os testes passam (`dotnet test` executa com sucesso)
- Testes seguem padrão AAA (Arrange-Act-Assert)
- Testes têm nomes descritivos

# Subtask 05: Validar padronização e testes

## Descrição
Validar que toda a padronização foi implementada corretamente, que não há mais mensagens hardcoded nas controllers, e que todos os testes continuam passando.

## Passos de implementação

### 1. Verificar ausência de mensagens hardcoded
- Buscar por strings hardcoded nas controllers:
  - `"Dados inválidos."`
  - `"Erro interno do servidor."`
  - Qualquer outra mensagem hardcoded
- Verificar que todas as mensagens vêm de exceções ou use cases

### 2. Verificar estrutura de exceções
- Verificar que `ValidationException` e `InternalServerErrorException` foram criadas
- Verificar que exceções têm documentação XML completa
- Verificar que exceções seguem padrão das outras exceções do projeto

### 3. Verificar Use Cases
- Verificar que todos os Use Cases validam `inputModel == null`
- Verificar que validações básicas lançam `ValidationException`
- Verificar que validações específicas de negócio mantidas
- Verificar que exceções de negócio mantidas

### 4. Verificar Controllers
- Verificar que validação de ModelState foi removida
- Verificar que controllers tratam `ValidationException`
- Verificar que controllers tratam `InternalServerErrorException`
- Verificar que catch genérico usa mensagem padrão
- Verificar que todas as mensagens vêm de exceções

### 5. Executar testes
- Executar todos os testes unitários
- Executar todos os testes de integração
- Verificar que todos os testes passam
- Verificar cobertura de testes (se aplicável)

### 6. Validar respostas da API
- Testar endpoints manualmente ou via testes
- Verificar que respostas seguem padrão `ApiResponse<T>`
- Verificar que mensagens são descritivas e vêm de exceções
- Verificar que estrutura JSON está correta

### 7. Verificar compilação
- Executar `dotnet build` em todos os projetos
- Verificar que não há erros de compilação
- Verificar que não há warnings relevantes

## Checklist de validação

### Exceções
- [ ] `ValidationException` criada e documentada
- [ ] `InternalServerErrorException` criada e documentada
- [ ] Exceções seguem padrão do projeto

### Use Cases
- [ ] `CreatePreparationUseCase` valida InputModel
- [ ] `GetPreparationsUseCase` valida InputModel
- [ ] `StartPreparationUseCase` valida InputModel
- [ ] `FinishPreparationUseCase` valida InputModel
- [ ] `CreateDeliveryUseCase` valida InputModel
- [ ] `GetReadyDeliveriesUseCase` valida InputModel
- [ ] `FinalizeDeliveryUseCase` valida InputModel
- [ ] Validações básicas lançam `ValidationException`
- [ ] Validações específicas de negócio mantidas

### Controllers
- [ ] `PreparationController` não tem validação de ModelState
- [ ] `PreparationController` trata `ValidationException`
- [ ] `PreparationController` trata `InternalServerErrorException`
- [ ] `DeliveryController` não tem validação de ModelState
- [ ] `DeliveryController` trata `ValidationException`
- [ ] `DeliveryController` trata `InternalServerErrorException`
- [ ] Nenhuma mensagem hardcoded nas controllers

### Mensagens
- [ ] Todas as mensagens vêm de exceções
- [ ] Mensagens são descritivas
- [ ] Mensagens seguem padrão estabelecido

### Testes
- [ ] Todos os testes unitários passam
- [ ] Todos os testes de integração passam
- [ ] Cobertura de testes mantida (se aplicável)

### Compilação
- [ ] Projeto Application compila sem erros
- [ ] Projeto Api compila sem erros
- [ ] Projeto Tests compila sem erros
- [ ] Sem warnings relevantes

### Respostas da API
- [ ] Respostas seguem padrão `ApiResponse<T>`
- [ ] Estrutura JSON está correta
- [ ] Mensagens são descritivas

## Comandos para validação

```bash
# Compilar todos os projetos
dotnet build

# Executar testes unitários
dotnet test src/tests/FastFood.KitchenFlow.Tests.Unit

# Executar testes de integração
dotnet test src/tests/FastFood.KitchenFlow.Tests.Integration

# Buscar mensagens hardcoded
grep -r "Dados inválidos" src/InterfacesExternas
grep -r "Erro interno do servidor" src/InterfacesExternas
```

## Referências
- Seguir padrão estabelecido na Story 13
- Manter compatibilidade com estrutura existente

## Observações importantes
- **Validação completa**: Garantir que toda a padronização foi implementada
- **Testes**: Todos os testes devem continuar passando
- **Mensagens**: Nenhuma mensagem hardcoded deve permanecer
- **Documentação**: Verificar que documentação XML está atualizada

## Critérios de aceite
- [ ] Todas as validações do checklist concluídas
- [ ] Nenhuma mensagem hardcoded nas controllers
- [ ] Todas as mensagens vêm de exceções ou use cases
- [ ] Todos os testes passam
- [ ] Código compila sem erros
- [ ] Respostas da API seguem padrão estabelecido

# Subtask 04: Criar exceções de domínio (se necessário)

## Descrição
Analisar se são necessárias exceções de domínio customizadas para as validações das entidades Preparation e Delivery. Se necessário, criar classes de exceção seguindo o padrão do projeto de referência.

## Passos de implementação
- Analisar padrão de exceções do monolito:
  - Verificar como `DomainValidation.ThrowIf()` funciona
  - Verificar se há exceções customizadas de domínio
- Analisar padrão de exceções do OrderHub:
  - Verificar se há exceções de domínio customizadas
  - Verificar padrão de validações
- Decidir abordagem:
  - **Opção 1**: Usar exceções padrão do .NET (`ArgumentException`, `InvalidOperationException`)
  - **Opção 2**: Criar exceções customizadas de domínio (`DomainException`, `InvalidStatusTransitionException`)
- Se necessário criar exceções customizadas:
  - Criar pasta `Common/Exceptions/` (se ainda não existir)
  - Criar classe base `DomainException` (se necessário)
  - Criar exceções específicas (ex: `InvalidStatusTransitionException`, `InvalidPreparationStateException`)
- Atualizar entidades Preparation e Delivery para usar as exceções criadas
- Adicionar documentação XML nas exceções

## Referências
- **Monolito**: Verificar uso de `DomainValidation.ThrowIf()` e exceções de domínio
- **OrderHub**: Verificar padrão de validações e exceções
- **Padrão .NET**: Usar exceções padrão quando possível (`ArgumentException`, `InvalidOperationException`)

## Como testar
- **Testes unitários**: Verificar que exceções são lançadas corretamente nas validações
- **Compilação**: Executar `dotnet build` no projeto Domain
- **Validação manual**: Testar cenários que devem lançar exceções

## Critérios de aceite
- Decisão tomada sobre abordagem de exceções (padrão .NET ou customizadas)
- Se exceções customizadas foram criadas:
  - Exceções criadas em `Common/Exceptions/`
  - Exceções seguem padrão do projeto de referência
  - Exceções têm mensagens descritivas
- Entidades Preparation e Delivery usam exceções consistentemente
- Projeto Domain compila sem erros
- Nenhuma dependência externa nas exceções
- Testes validam que exceções são lançadas corretamente

## Observações
- **Recomendação**: Começar com exceções padrão do .NET (`InvalidOperationException` para transições de status inválidas, `ArgumentException` para parâmetros inválidos)
- **Exceções customizadas**: Criar apenas se houver necessidade específica ou se o padrão do projeto exigir
- **Mensagens**: Exceções devem ter mensagens claras e descritivas para facilitar debugging

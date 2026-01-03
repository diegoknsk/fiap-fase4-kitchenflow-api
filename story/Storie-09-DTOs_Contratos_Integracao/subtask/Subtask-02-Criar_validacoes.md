# Subtask 02: Criar validações para OrderSnapshot

## Descrição
Criar validações para garantir que o `OrderSnapshot` recebido do Payment está no formato correto e contém todos os dados necessários.

## Passos de implementação
- Adicionar Data Annotations aos DTOs:
  - `OrderSnapshotDto`:
    - `[Required]` em `OrderId`
    - `[Required]` em `TotalPrice`
    - `[Required]` em `CreatedAt`
    - `[Required]` em `Items` (lista não pode ser vazia)
    - `[Required]` em `PaymentId`
    - `[Required]` em `PaidAt`
  - `OrderItemSnapshotDto`:
    - `[Required]` em `ProductId`
    - `[Required]` em `ProductName`
    - `[Range(1, int.MaxValue)]` em `Quantity`
    - `[Range(0, double.MaxValue)]` em `UnitPrice` e `FinalPrice`
  - `OrderIngredientSnapshotDto`:
    - `[Required]` em `Name`
    - `[Range(0, double.MaxValue)]` em `Price`
    - `[Range(1, int.MaxValue)]` em `Quantity`
- Criar método de validação no UseCase `CreatePreparationUseCase`:
  - Validar que `orderSnapshot` (string) pode ser deserializado para `OrderSnapshotDto`
  - Validar estrutura básica (campos obrigatórios)
  - Se inválido, retornar erro 400 com detalhes
- (Opcional) Criar classe de validação customizada se necessário
- Adicionar tratamento de erros de deserialização JSON

## Referências
- **ASP.NET Core**: Usar Data Annotations para validação
- **JSON**: Usar `System.Text.Json` ou `Newtonsoft.Json` para deserialização

## Observações importantes
- **Validação na API**: Validar formato JSON básico
- **Validação no UseCase**: Validar estrutura e campos obrigatórios
- **Ordem**: Validar antes de criar entidade de domínio
- **Erros**: Retornar mensagens claras de erro

## Como testar
- Executar `dotnet build` no projeto Application
- Criar teste unitário de validação:
  - Teste com OrderSnapshot válido
  - Teste com OrderSnapshot inválido (campos faltando)
  - Teste com JSON malformado

## Critérios de aceite
- Data Annotations adicionadas aos DTOs:
  - Campos obrigatórios marcados com `[Required]`
  - Campos numéricos com `[Range]` quando apropriado
- Validação no UseCase implementada:
  - Deserialização JSON validada
  - Estrutura validada
  - Erros retornados adequadamente
- Projeto Application compila sem erros
- Validações funcionam corretamente
- Mensagens de erro são claras

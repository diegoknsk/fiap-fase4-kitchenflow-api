# Subtask 03: Documentar contrato de integração

## Descrição
Criar documentação completa do contrato de integração entre Payment e KitchenFlow, incluindo estrutura de dados, exemplos e regras de negócio.

## Passos de implementação
- Criar arquivo de documentação em `docs/INTEGRATION_CONTRACT.md`:
  - Seção: **Contrato de Integração Payment → KitchenFlow**
  - Endpoint: `POST /api/preparations`
  - Descrição do fluxo
  - Estrutura do Request
  - Estrutura do Response
  - Códigos de status HTTP
  - Tratamento de erros
  - Idempotência
- Documentar estrutura do `OrderSnapshot`:
  - Campos obrigatórios
  - Campos opcionais
  - Tipos de dados
  - Exemplos de valores
- Documentar regras de negócio:
  - Quando chamar o endpoint
  - Validações esperadas
  - Comportamento de idempotência
- Adicionar exemplos de integração:
  - Exemplo de chamada HTTP
  - Exemplo de payload completo
  - Exemplo de payload mínimo
- Adicionar diagrama de sequência (opcional, texto ou mermaid)

## Referências
- **Swagger**: Documentação automática via Swagger
- **README**: Documentação adicional em markdown

## Observações importantes
- **Documentação clara**: Deve ser fácil para o time do Payment entender
- **Exemplos práticos**: Incluir exemplos reais de uso
- **Versionamento**: Considerar versionamento futuro

## Como testar
- Verificar que documentação está completa
- Validar exemplos de JSON
- Verificar que diagramas (se houver) estão corretos

## Critérios de aceite
- Arquivo `INTEGRATION_CONTRACT.md` criado em `docs/`
- Contrato documentado:
  - Endpoint documentado
  - Request/Response documentados
  - Códigos de status documentados
  - Tratamento de erros documentado
- Estrutura do OrderSnapshot documentada:
  - Campos obrigatórios
  - Campos opcionais
  - Tipos de dados
- Exemplos incluídos:
  - Exemplo completo
  - Exemplo mínimo
- Regras de negócio documentadas
- Documentação está clara e completa

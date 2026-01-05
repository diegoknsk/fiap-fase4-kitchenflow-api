# Subtask 08: Validar Configurações no Cluster

## Descrição
Validar que as configurações de autenticação estão corretas no cluster Kubernetes, garantindo que são idênticas ao OrderHub e que as variáveis de ambiente estão configuradas corretamente.

## Passos de implementação
- Verificar arquivo de deployment Kubernetes (se existir):
  - `deployment.yaml` ou similar
  - Verificar variáveis de ambiente:
    - `Authentication__Cognito__Region` ou `COGNITO__REGION`
    - `Authentication__Cognito__UserPoolId` ou `COGNITO__USERPOOLID`
    - `Authentication__Cognito__ClientId` ou `COGNITO__CLIENTID`
- Comparar com configurações do OrderHub:
  - Verificar que os valores são **idênticos** (mesmo UserPoolId, ClientId, Region)
  - Verificar que as variáveis de ambiente têm os mesmos nomes
- Verificar ConfigMap/Secrets do Kubernetes:
  - Valores sensíveis devem estar em Secrets
  - Valores não sensíveis podem estar em ConfigMap
- Testar no cluster (após deploy):
  - Fazer requisição sem token → Deve retornar 401
  - Fazer requisição com token Cognito válido → Deve retornar 200/201
  - Verificar logs da aplicação para erros de autenticação
- Validar que o mesmo token Cognito funciona em OrderHub e KitchenFlow

## Referências
- **OrderHub deployment**: Verificar arquivos de deployment do OrderHub no repositório
- Configurações devem ser idênticas pois compartilham o mesmo Cognito User Pool

## Pré-requisitos
- Acesso ao cluster Kubernetes
- Permissões para ver ConfigMaps e Secrets
- Token Cognito válido para testes

## Como testar
- Usar `kubectl` para verificar variáveis de ambiente:
  ```bash
  kubectl get deployment kitchenflow-api -o yaml
  kubectl describe deployment kitchenflow-api
  ```
- Fazer requisições HTTP para o serviço no cluster
- Verificar logs:
  ```bash
  kubectl logs -f deployment/kitchenflow-api
  ```

## Critérios de aceite
- Variáveis de ambiente configuradas no deployment:
  - `Authentication__Cognito__Region` ou `COGNITO__REGION` → Valor correto
  - `Authentication__Cognito__UserPoolId` ou `COGNITO__USERPOOLID` → Valor correto (igual ao OrderHub)
  - `Authentication__Cognito__ClientId` ou `COGNITO__CLIENTID` → Valor correto (igual ao OrderHub)
- Configurações são **idênticas** ao OrderHub
- Aplicação no cluster:
  - Inicia sem erros
  - Endpoints retornam 401 sem token (exceto CreatePreparation)
  - Endpoints retornam 200/201 com token Cognito válido
  - Mesmo token Cognito funciona em OrderHub e KitchenFlow
- Logs não mostram erros de autenticação/configuração

## Observações
- Se as configurações não forem idênticas, pode haver problemas de autenticação
- Valores sensíveis devem estar em Secrets do Kubernetes, não hardcoded
- Validar que o mesmo Cognito User Pool está sendo usado em ambos os serviços

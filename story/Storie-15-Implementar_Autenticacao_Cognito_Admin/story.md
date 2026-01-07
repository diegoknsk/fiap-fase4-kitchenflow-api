# Storie-15: Implementar Autenticação Cognito e Admin

## Descrição
Como desenvolvedor, quero implementar o esquema de autenticação Cognito e Admin idêntico ao projeto OrderHub, garantindo que todas as configurações sejam iguais pois os serviços ficarão no mesmo cluster e terão as mesmas chaves. Todas as controllers devem ter autenticação Cognito com política Admin, exceto o endpoint CreatePreparation que deve permitir acesso anônimo, pois será chamado de outra API.

## Objetivo
Implementar autenticação e autorização completa seguindo exatamente o padrão do projeto `C:\Projetos\Fiap\fiap-fase4-orderhub-api`:
- Configurar autenticação JWT Bearer para Customer (se necessário)
- Configurar autenticação JWT Bearer para Cognito (Admin)
- Configurar políticas de autorização (Admin)
- Aplicar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]` em todos os endpoints
- Manter `CreatePreparation` como anônimo (sem autenticação)
- Garantir que todas as configurações sejam idênticas ao OrderHub

## Escopo Técnico
- **Tecnologias**: .NET 8, ASP.NET Core, Microsoft.AspNetCore.Authentication.JwtBearer
- **Camadas afetadas**: 
  - `FastFood.KitchenFlow.Infra` (configurações de autenticação)
  - `FastFood.KitchenFlow.Api` (Program.cs, Controllers)
- **Referências do projeto OrderHub**:
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\`
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\Program.cs`
  - `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\InterfacesExternas\FastFood.OrderHub.Api\Controllers\`

## Mudanças Necessárias

### 1. Criar Estrutura de Autenticação na Infra
- Criar pasta `FastFood.KitchenFlow.Infra\Auth`
- Copiar e adaptar os arquivos de configuração do OrderHub:
  - `JwtAuthenticationConfig.cs` (se necessário para Customer)
  - `CognitoAuthenticationConfig.cs` (obrigatório)
  - `AuthorizationConfig.cs` (obrigatório)
  - `JwtOptions.cs` (se necessário)
  - `CognitoOptions.cs` (obrigatório)

### 2. Configurar Program.cs
- Adicionar `JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler()` no início
- Configurar `JwtOptions` para Customer (se necessário)
- Configurar autenticação:
  - `.AddCustomerJwtBearer(builder.Configuration)` (se necessário)
  - `.AddCognitoJwtBearer(builder.Configuration)` (obrigatório)
- Configurar políticas de autorização:
  - `.AddAuthorizationPolicies()` (obrigatório)
- Adicionar `app.UseAuthentication()` antes de `app.UseAuthorization()`

### 3. Atualizar Controllers
- **PreparationController**:
  - `CreatePreparation`: Manter sem `[Authorize]` (acesso anônimo)
  - `GetPreparations`: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `StartPreparation`: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinishPreparation`: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- **DeliveryController**:
  - `GetReadyDeliveries`: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinalizeDelivery`: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- **HealthController**: Manter sem autenticação (se existir)

### 4. Configurar appsettings.json
- Adicionar seção `Authentication:Cognito`:
  - `Region`: "us-east-1"
  - `UserPoolId`: "" (será preenchido via variável de ambiente)
  - `ClientId`: "" (será preenchido via variável de ambiente)
  - `ClockSkewMinutes`: 5
- Adicionar seção `JwtCustomer` (se necessário):
  - `Issuer`: "FastFood.Auth"
  - `Audience`: "FastFood.API"
  - `SecretKey`: "" (será preenchido via variável de ambiente)

### 5. Configurar Variáveis de Ambiente
- `COGNITO__REGION` (ou via appsettings)
- `COGNITO__USERPOOLID` (ou via appsettings)
- `COGNITO__CLIENTID` (ou via appsettings)
- `JwtCustomer__SecretKey` (se necessário)

## Arquivos a Criar/Copiar

### FastFood.KitchenFlow.Infra/Auth/

#### CognitoOptions.cs
```csharp
namespace FastFood.KitchenFlow.Infra.Auth
{
    public sealed class CognitoOptions
    {
        public const string SectionName = "Authentication:Cognito";
        public string UserPoolId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-1";
        public int? ClockSkewMinutes { get; set; } = 5;
        public string Authority => $"https://cognito-idp.{Region}.amazonaws.com/{UserPoolId}";
    }
}
```

#### CognitoAuthenticationConfig.cs
- Copiar exatamente de `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\CognitoAuthenticationConfig.cs`
- Adaptar namespace para `FastFood.KitchenFlow.Infra.Auth`

#### AuthorizationConfig.cs
- Copiar exatamente de `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\AuthorizationConfig.cs`
- Adaptar namespace para `FastFood.KitchenFlow.Infra.Auth`
- Manter apenas a política `Admin` (se Customer não for necessário)

#### JwtAuthenticationConfig.cs (Opcional - apenas se Customer JWT for necessário)
- Copiar de `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\JwtAuthenticationConfig.cs`
- Adaptar namespace para `FastFood.KitchenFlow.Infra.Auth`

#### JwtOptions.cs (Opcional - apenas se Customer JWT for necessário)
- Copiar de `C:\Projetos\Fiap\fiap-fase4-orderhub-api\src\Infra\FastFood.OrderHub.Infra\Auth\JwtOptions.cs`
- Adaptar namespace para `FastFood.KitchenFlow.Infra.Auth`

## Endpoints Afetados

### POST /api/preparations
**Mudança**: Nenhuma. Mantém acesso anônimo (sem `[Authorize]`).

**Justificativa**: Este endpoint será chamado por outra API (OrderHub) quando o pagamento for confirmado, portanto não deve exigir autenticação.

### GET /api/preparations
**Mudança**: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`.

**Antes**: Sem autenticação
**Depois**: Requer token Cognito com política Admin

### POST /api/preparations/take-next
**Mudança**: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`.

**Antes**: Sem autenticação
**Depois**: Requer token Cognito com política Admin

### POST /api/preparations/{id}/finish
**Mudança**: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`.

**Antes**: Sem autenticação
**Depois**: Requer token Cognito com política Admin

### GET /api/deliveries/ready
**Mudança**: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`.

**Antes**: Sem autenticação
**Depois**: Requer token Cognito com política Admin

### POST /api/deliveries/{id}/finalize
**Mudança**: Adicionar `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`.

**Antes**: Sem autenticação
**Depois**: Requer token Cognito com política Admin

## Configuração do Program.cs

### Antes
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ... outras configurações ...

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
```

### Depois
```csharp
using FastFood.KitchenFlow.Infra.Auth;

// Configurar JWT Security Token Handler
JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure JWT options (se Customer JWT for necessário)
builder.Services.Configure<JwtOptions>("Customer", builder.Configuration.GetSection("JwtCustomer"));

// Configure authentication
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCustomerJwtBearer(builder.Configuration) // Opcional - apenas se Customer JWT for necessário
    .AddCognitoJwtBearer(builder.Configuration);

// Configure authorization policies
builder.Services.AddAuthorizationPolicies();

// ... outras configurações ...

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

app.UseAuthentication(); // Adicionar antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();
```

## Configuração do appsettings.json

### Adicionar
```json
{
  "Authentication": {
    "Cognito": {
      "Region": "us-east-1",
      "UserPoolId": "",
      "ClientId": "",
      "ClockSkewMinutes": 5
    }
  },
  "JwtCustomer": {
    "Issuer": "FastFood.Auth",
    "Audience": "FastFood.API",
    "SecretKey": ""
  }
}
```

**Nota**: Os valores vazios serão preenchidos via variáveis de ambiente no cluster Kubernetes.

## Variáveis de Ambiente (Kubernetes)

As seguintes variáveis devem ser configuradas no cluster (iguais ao OrderHub):
- `Authentication__Cognito__Region` (ou `COGNITO__REGION`)
- `Authentication__Cognito__UserPoolId` (ou `COGNITO__USERPOOLID`)
- `Authentication__Cognito__ClientId` (ou `COGNITO__CLIENTID`)
- `JwtCustomer__SecretKey` (se Customer JWT for necessário)

## Dependências NuGet

Adicionar ao projeto `FastFood.KitchenFlow.Infra`:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (versão compatível com .NET 8)
- `System.IdentityModel.Tokens.Jwt` (versão compatível)

## Subtasks

- [ ] [Subtask 01: Criar estrutura de autenticação na Infra](./subtask/Subtask-01-Criar_Estrutura_Autenticacao_Infra.md)
- [ ] [Subtask 02: Implementar CognitoAuthenticationConfig](./subtask/Subtask-02-Implementar_CognitoAuthenticationConfig.md)
- [ ] [Subtask 03: Implementar AuthorizationConfig](./subtask/Subtask-03-Implementar_AuthorizationConfig.md)
- [ ] [Subtask 04: Configurar Program.cs com autenticação](./subtask/Subtask-04-Configurar_Program_Autenticacao.md)
- [ ] [Subtask 05: Atualizar Controllers com Authorize](./subtask/Subtask-05-Atualizar_Controllers_Authorize.md)
- [ ] [Subtask 06: Configurar appsettings.json](./subtask/Subtask-06-Configurar_appsettings.md)
- [ ] [Subtask 07: Testar autenticação localmente](./subtask/Subtask-07-Testar_Autenticacao_Local.md)
- [ ] [Subtask 08: Validar configurações no cluster](./subtask/Subtask-08-Validar_Configuracoes_Cluster.md)

## Critérios de Aceite da História

- [ ] Estrutura de autenticação criada na Infra:
  - Pasta `FastFood.KitchenFlow.Infra/Auth` criada
  - Arquivos `CognitoOptions.cs`, `CognitoAuthenticationConfig.cs`, `AuthorizationConfig.cs` implementados
  - Namespaces adaptados corretamente
- [ ] `Program.cs` configurado:
  - `JwtAuthenticationConfig.ConfigureJwtSecurityTokenHandler()` chamado no início
  - Autenticação Cognito configurada via `.AddCognitoJwtBearer()`
  - Políticas de autorização configuradas via `.AddAuthorizationPolicies()`
  - `app.UseAuthentication()` adicionado antes de `app.UseAuthorization()`
- [ ] Controllers atualizados:
  - `CreatePreparation`: Sem `[Authorize]` (acesso anônimo)
  - `GetPreparations`: Com `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `StartPreparation`: Com `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinishPreparation`: Com `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `GetReadyDeliveries`: Com `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
  - `FinalizeDelivery`: Com `[Authorize(AuthenticationSchemes = "Cognito", Policy = "Admin")]`
- [ ] `appsettings.json` configurado:
  - Seção `Authentication:Cognito` adicionada
  - Valores configurados corretamente
- [ ] Dependências NuGet adicionadas:
  - `Microsoft.AspNetCore.Authentication.JwtBearer`
  - `System.IdentityModel.Tokens.Jwt`
- [ ] Testes de autenticação:
  - Endpoint `CreatePreparation` acessível sem token
  - Demais endpoints retornam 401 sem token
  - Endpoints retornam 200 com token Cognito válido
  - Endpoints retornam 403 com token Cognito sem política Admin
- [ ] Configurações idênticas ao OrderHub:
  - Mesmas variáveis de ambiente
  - Mesmas configurações de Cognito
  - Mesma estrutura de código

## Observações Arquiteturais

### Clean Architecture
- **Infra**: Contém configurações de autenticação (adapters de infraestrutura)
- **Api**: Usa as configurações da Infra via extension methods
- **Controllers**: Aplicam atributos de autorização, mas não contêm lógica de autenticação

### Segurança
- **Token Cognito**: Validado automaticamente pelo middleware JWT Bearer
- **Política Admin**: Valida claim `scope` com valor `aws.cognito.signin.user.admin`
- **Acesso Anônimo**: Apenas `CreatePreparation` permite acesso sem autenticação

### Configuração
- **Variáveis de Ambiente**: Preferidas para valores sensíveis (UserPoolId, ClientId)
- **appsettings.json**: Usado para valores não sensíveis (Region, ClockSkewMinutes)
- **Mesmo Cluster**: Configurações devem ser idênticas ao OrderHub para compartilhar o mesmo Cognito User Pool

### Integração
- **OrderHub → KitchenFlow**: `CreatePreparation` será chamado sem autenticação quando o pagamento for confirmado
- **Admin → KitchenFlow**: Todos os demais endpoints requerem autenticação Cognito com política Admin

---

## ✅ Story Concluída

**Data de Conclusão**: [A preencher após implementação]

### Resumo da Implementação

[Será preenchido após conclusão]

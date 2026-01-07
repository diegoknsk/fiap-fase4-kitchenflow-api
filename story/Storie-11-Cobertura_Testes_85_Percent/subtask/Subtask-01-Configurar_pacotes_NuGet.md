# Subtask 01: Configurar pacotes NuGet do projeto de testes

## Descrição
Configurar o projeto `FastFood.KitchenFlow.Tests.Unit` com todos os pacotes NuGet necessários para testes unitários, cobertura de código e integração com SonarCloud, seguindo as lições aprendidas do projeto OrderHub.

## Passos de implementação
- Abrir arquivo `src/tests/FastFood.KitchenFlow.Tests.Unit/FastFood.KitchenFlow.Tests.Unit.csproj`
- Adicionar/atualizar pacotes NuGet obrigatórios:
  - `Microsoft.NET.Test.Sdk` (Version 17.8.0)
  - `xunit` (Version 2.6.2)
  - `xunit.runner.visualstudio` (Version 2.5.4)
  - `coverlet.collector` (Version 6.0.0)
  - `coverlet.msbuild` (Version 6.0.0)
  - `Moq` (Version 4.20.70)
  - `FluentAssertions` (Version 6.12.0)
- Verificar que os pacotes estão corretamente configurados
- Executar `dotnet restore` para baixar os pacotes
- Executar `dotnet build` para verificar compilação

## Estrutura do .csproj esperada

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="xunit" Version="2.6.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.4">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.msbuild" Version="6.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Moq" Version="4.20.70" />
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\Core\FastFood.KitchenFlow.Domain\FastFood.KitchenFlow.Domain.csproj" />
    <ProjectReference Include="..\..\Core\FastFood.KitchenFlow.Application\FastFood.KitchenFlow.Application.csproj" />
    <ProjectReference Include="..\..\Core\FastFood.KitchenFlow.CrossCutting\FastFood.KitchenFlow.CrossCutting.csproj" />
    <ProjectReference Include="..\..\Infra\FastFood.KitchenFlow.Infra.Persistence\FastFood.KitchenFlow.Infra.Persistence.csproj" />
    <ProjectReference Include="..\..\Infra\FastFood.KitchenFlow.Infra\FastFood.KitchenFlow.Infra.csproj" />
    <ProjectReference Include="..\..\InterfacesExternas\FastFood.KitchenFlow.Api\FastFood.KitchenFlow.Api.csproj" />
  </ItemGroup>

</Project>
```

## Observações importantes
- **Versões específicas**: Usar as versões exatas mencionadas para garantir compatibilidade
- **coverlet.msbuild**: Necessário para gerar relatórios de cobertura durante o build
- **coverlet.collector**: Necessário para coletar cobertura durante execução de testes
- **Moq**: Framework de mocking para isolar dependências
- **FluentAssertions**: Biblioteca para assertions mais legíveis
- **ProjectReference para Api**: Necessário para testar Controllers

## Como testar
- Executar `dotnet restore` no projeto de testes
- Executar `dotnet build` no projeto de testes
- Verificar que não há erros de compilação
- Executar `dotnet test` (deve passar mesmo sem testes, apenas validar estrutura)

## Critérios de aceite
- Todos os pacotes NuGet adicionados com versões corretas
- Projeto compila sem erros
- `dotnet restore` executa com sucesso
- `dotnet build` executa com sucesso
- `dotnet test` executa com sucesso (mesmo sem testes ainda)
- ProjectReference para Api adicionado (necessário para testes de Controllers)

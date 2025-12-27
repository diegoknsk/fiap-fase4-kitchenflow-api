# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar arquivos .csproj e fazer restore (otimização de cache)
COPY src/Core/FastFood.KitchenFlow.Domain/FastFood.KitchenFlow.Domain.csproj src/Core/FastFood.KitchenFlow.Domain/
COPY src/Core/FastFood.KitchenFlow.Application/FastFood.KitchenFlow.Application.csproj src/Core/FastFood.KitchenFlow.Application/
COPY src/Core/FastFood.KitchenFlow.CrossCutting/FastFood.KitchenFlow.CrossCutting.csproj src/Core/FastFood.KitchenFlow.CrossCutting/
COPY src/Infra/FastFood.KitchenFlow.Infra/FastFood.KitchenFlow.Infra.csproj src/Infra/FastFood.KitchenFlow.Infra/
COPY src/Infra/FastFood.KitchenFlow.Infra.Persistence/FastFood.KitchenFlow.Infra.Persistence.csproj src/Infra/FastFood.KitchenFlow.Infra.Persistence/
COPY src/InterfacesExternas/FastFood.KitchenFlow.Migrator/FastFood.KitchenFlow.Migrator.csproj src/InterfacesExternas/FastFood.KitchenFlow.Migrator/

# Restaurar dependências do Migrator (isso também restaura dependências transitivas)
RUN dotnet restore src/InterfacesExternas/FastFood.KitchenFlow.Migrator/FastFood.KitchenFlow.Migrator.csproj --verbosity quiet

# Copiar todo o código
COPY . .

# Publicar o Migrator
RUN dotnet publish src/InterfacesExternas/FastFood.KitchenFlow.Migrator/FastFood.KitchenFlow.Migrator.csproj -c Release -o /out \
    /p:CopyOutputSymbolsToPublishDirectory=false \
    /p:CopyOutputXmlDocumentationToPublishDirectory=false

# Copiar migrations se existirem
RUN mkdir -p /migrations && \
    if [ -d "src/Infra/FastFood.KitchenFlow.Infra.Persistence/Migrations" ]; then \
        cp -r src/Infra/FastFood.KitchenFlow.Infra.Persistence/Migrations/* /migrations/ 2>/dev/null || true; \
    fi

# Etapa 2: imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Copiar aplicação publicada
COPY --from=build /out ./

# Copiar arquivo de configuração
COPY src/InterfacesExternas/FastFood.KitchenFlow.Migrator/appsettings.json ./appsettings.json

# Copiar migrations se existirem
COPY --from=build /migrations ./Migrations 2>/dev/null || true

# Segurança: Rodar como não-root (seguindo regras do projeto)
RUN chown -R 1001:1001 /app
USER 1001:1001

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "FastFood.KitchenFlow.Migrator.dll"]


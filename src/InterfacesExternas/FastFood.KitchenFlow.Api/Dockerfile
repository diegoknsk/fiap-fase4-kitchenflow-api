# Etapa 1: build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar arquivos .csproj e fazer restore (otimização de cache)
COPY src/Core/FastFood.KitchenFlow.Domain/FastFood.KitchenFlow.Domain.csproj src/Core/FastFood.KitchenFlow.Domain/
COPY src/Core/FastFood.KitchenFlow.Application/FastFood.KitchenFlow.Application.csproj src/Core/FastFood.KitchenFlow.Application/
COPY src/Core/FastFood.KitchenFlow.CrossCutting/FastFood.KitchenFlow.CrossCutting.csproj src/Core/FastFood.KitchenFlow.CrossCutting/
COPY src/Infra/FastFood.KitchenFlow.Infra/FastFood.KitchenFlow.Infra.csproj src/Infra/FastFood.KitchenFlow.Infra/
COPY src/Infra/FastFood.KitchenFlow.Infra.Persistence/FastFood.KitchenFlow.Infra.Persistence.csproj src/Infra/FastFood.KitchenFlow.Infra.Persistence/
COPY src/InterfacesExternas/FastFood.KitchenFlow.Api/FastFood.KitchenFlow.Api.csproj src/InterfacesExternas/FastFood.KitchenFlow.Api/

RUN dotnet restore src/InterfacesExternas/FastFood.KitchenFlow.Api/FastFood.KitchenFlow.Api.csproj

# Copiar todo o código
COPY . .

# Publicar a aplicação
RUN dotnet publish src/InterfacesExternas/FastFood.KitchenFlow.Api/FastFood.KitchenFlow.Api.csproj -c Release -o /app/publish \
    /p:CopyOutputSymbolsToPublishDirectory=false \
    /p:CopyOutputXmlDocumentationToPublishDirectory=false

# Etapa 2: imagem de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

ENTRYPOINT ["dotnet", "FastFood.KitchenFlow.Api.dll"]


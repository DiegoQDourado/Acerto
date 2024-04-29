FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet_build
WORKDIR /src
COPY "AutenticationApi.csproj" "AutenticationApi.csproj"

RUN dotnet restore "AutenticationApi.csproj" --no-cache

FROM dotnet_build AS dotnet_publish
WORKDIR /src
COPY . .
RUN dotnet publish "AutenticationApi.csproj" -c Release -o /app 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=dotnet_publish /app .

# Usar esta porta no container se for uma API. Caso contrário, delete estas linhas.
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

RUN echo "dotnet AutenticationApi.dll" >> entrypoint.sh \
  && chmod a+x entrypoint.sh

ENTRYPOINT ["sh", "entrypoint.sh"]

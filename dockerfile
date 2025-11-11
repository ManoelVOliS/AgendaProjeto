FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY Agenda.Api/*.csproj ./Agenda.Api/
COPY Agenda.Core/*.csproj ./Agenda.Core/
COPY Agenda.Infrastructure/*.csproj ./Agenda.Infrastructure/

RUN dotnet restore "Agenda.Api/Agenda.Api.csproj"

COPY . .

WORKDIR "/src/Agenda.Api"
RUN dotnet build "Agenda.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Agenda.Api.dll"]
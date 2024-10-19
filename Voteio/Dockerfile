FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY *.sln ./
COPY Voteio/*.csproj ./Voteio/

RUN dotnet restore

COPY Voteio/. ./Voteio/

RUN dotnet build -c Release --no-restore

RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT ["dotnet", "Voteio.dll"]

#(opcional)
RUN apt-get update && apt-get install -y mysql-client

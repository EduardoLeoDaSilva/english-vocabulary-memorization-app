
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["EnglishVocabularyMemorization.csproj", "./"]
RUN dotnet restore "EnglishVocabularyMemorization.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EnglishVocabularyMemorization.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnglishVocabularyMemorization.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:3000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnglishVocabularyMemorization.dll"]

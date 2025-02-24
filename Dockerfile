# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY *.sln .
COPY StealAllTheCats.Domain/*.csproj ./StealAllTheCats.Domain/
COPY StealAllTheCats.Application/*.csproj ./StealAllTheCats.Application/
COPY StealAllTheCats.Infrastructure/*.csproj ./StealAllTheCats.Infrastructure/
COPY StealAllTheCats.WebApi/*.csproj ./StealAllTheCats.WebApi/
COPY StealAllTheCats.Tests/*.csproj ./StealAllTheCats.Tests/
RUN dotnet restore

# Copy the rest of the application code
COPY StealAllTheCats.Domain/. ./StealAllTheCats.Domain/
COPY StealAllTheCats.Application/. ./StealAllTheCats.Application/
COPY StealAllTheCats.Infrastructure/. ./StealAllTheCats.Infrastructure/
COPY StealAllTheCats.WebApi/. ./StealAllTheCats.WebApi/
COPY StealAllTheCats.Tests/. ./StealAllTheCats.Tests/

# Build the application
RUN dotnet publish StealAllTheCats.WebApi/StealAllTheCats.WebApi.csproj -c Release -o out

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Set environment variable to ensure ASP.NET Core listens on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose the port the application runs on
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "StealAllTheCats.WebApi.dll"]

# Steal All The Cats Project

##  Prerequisites
Microsoft SQL Server need to be installed on your machine.
* Create a server with name **steallallthecatsdb**

* Run EF Migrations

  ```dotnet ef migrations add YourMigrationName --project .\StealAllTheCats.Infrastructure\ --startup-project .\StealAllTheCats.WebApi\```
* Apply Migrations

  ```dotnet ef database update --project StealAllTheCats.Infrastructure --startup-project StealAllTheCats.WebApi```

# Running the Application

## Local

### Running the Application Locally

```dotnet run --project .\StealAllTheCats.WebApi\```

### Building the Application Locally

```dotnet build StealAllTheCats.sln```

## Using Docker

### Building The Application Image

```docker build -t stealallthecats:latest .```

### Creating an Application Container
```docker run -d -p 8080:8080 stealallthecats:latest```

Now you will be able to navigate at http://localhost:8080/swagger/index.html


## Running Tests

```dotnet test .\StealAllTheCats.Tests\StealAllTheCats.Tests.csproj```



# Steal All The Cats Project
## Purpose of this Project

This is a fun project I made to improve .NET skills and learn new design patterns such as the **Result Pattern**

My task is to “steal” 25 cat images from the Cats as a Service API (https://thecatapi.com/) and stash them in the database.

## Endpoints
• ***POST /api/cats/fetch***
 Fetch 25 cat images from CaaS API and save them to the
database. After rerunning the operation, no duplicate cats should be present.

• ***GET /api/cats/{id}*** 
Retrieve a cat by its ID.

• ***GET /api/cats***
 Retrieve cats with paging support (e.g., GET/api/cats?page=1&pageSize=10).

• **GET /api/cats***
Retrieve cats with a specific tag with paging support (e.g., GET/api/cats?tag=playful&page=1&pageSize=10”)


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



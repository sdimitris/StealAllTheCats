# Steal All The Cats Interview Project

##  Prerequisites
Microsoft SQL Server need to be installed on your machine.
* Create a server with name **steallallthecatsdb**

* Run EF Migrations

  ```dotnet ef migrations add YourMigrationName --project .\StealAllTheCats.Infrastructure\ --startup-project .\StealAllTheCats.WebApi\```
* Apply Migrations

  ```dotnet ef database update --project StealAllTheCats.Infrastructure --startup-project StealAllTheCats.WebApi```
## Running the Application

### Building The Application Image

```docker build -t stealallthecats:latest .```

### Creating an Application Container
```docker run -d -p 8080:8080 stealallthecats:latest```

Now you will be able to navigate at http://localhost:8080/swagger/index.html




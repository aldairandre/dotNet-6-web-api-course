# Learning how to create web API with csharp.

## Run docker

```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=sql2019" -p 1433:1433 -name sql1 -h sql1 -d mcr.microsoft.com/mssql/server
```

```
User = SA
Password = Sq1test2022
```

## Run Migrations

```
dotnet-ef migrations add NewNameMigration
```

## Update Database 

```
dotnet-ef databese update
```

## Revert Migrations

```
dotnet-ef migrations remove
```
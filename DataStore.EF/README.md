To create migration run the follwing from this dir
```powershell
dotnet ef --startup-project ..\RestApiTutorial\ migrations add <name>
```

To apply migragtion to the database
```powershell
dotnet ef --startup-project ..\RestApiTutorial\ database update
```

To revert to a specific migrations version
```powershell
dotnet ef --startup-project ..\RestApiTutorial\ database update <migration-name>
```

To revert all migrations
```powershell
dotnet ef --startup-project ..\RestApiTutorial\ database update 0
```

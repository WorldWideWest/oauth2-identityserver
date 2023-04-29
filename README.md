# oAuth2 with IdentityServer and Microsoft Identity (Configuration and Integration)


### Initializing Project from Scratch

PowerShell:
```powershell
./init-soulution.ps1
```

Bash:
```bash
chmod +x ./init-soulution.sh
./init-soulution.sh
```


### Running Migrations

Go into Authentication.Database:
```bash
cd Authentication.Database
```
PowerShell:
```powershell
./migration-script.ps1
```

Bash:
```bash
chmod +x ./migration-script.sh
./migration-script.sh
```

### Running the application

You can run the application using the **dotnet cli** tool or using **VisualStudio**
```bash
cd Authentication.API
dotnet run
```


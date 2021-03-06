# SyncthingWeb
Web application that manages Syncthing folders. Access your Syncthing data everwhere from server, share files and manage users.

## Disclaimer
The project is under (not heavly) development. If you wish some features - please contact me at patryk@pewudev.pl or create pull request.
Use fo your own risk.

If you wanna buy a beer? Contact me at mail.

**Read installation and configuration sections before use!**

## Features

Basic features implemented so far:
* Watching syncthing file changes
* Reading folders content
* Downloading files & folders
* Sharing files
* Users management (with folders permissions) 
* Previewing files

What is under testing? (will be available very soon)
* .NET Core 2.1 update

What is not present?
* Uploading
* Advanced user management
* Sending mails
* More...?

## Screenshots

![Setup](https://raw.githubusercontent.com/PeWuDev/SyncthingWeb/master/Assets/Setup.jpg)
![Dashboard](https://raw.githubusercontent.com/PeWuDev/SyncthingWeb/development/Assets/Dashboard.jpg)
![Files](https://raw.githubusercontent.com/PeWuDev/SyncthingWeb/master/Assets/Files.jpg)
![Sharing](https://raw.githubusercontent.com/PeWuDev/SyncthingWeb/master/Assets/Sharing.jpg)
![Devices](https://raw.githubusercontent.com/PeWuDev/SyncthingWeb/master/Assets/Devices.jpg)
## Requirements
* None!

## Installation
Download newest pre-compiled binaries from https://github.com/pwasiewicz/SyncthingWeb/releases.
Run `SyncthingWeb.exe` to start built-in kestrel server that will host your application.

### From source
* Download sources from github
* Run ```dotnet restore``` to install Asp.net core dependencies
* Run ```bower install``` to install bower dependencies
* Run ```npm install``` to install nodejs depdencies
* Run ```dotnet build``` to build whole project
* Run ```grunt build``` to build bower dependnecies and copy libraries to output path
* Run ```dotnet run``` to start kestrell server:

```ps
C:\sw> dotnet run
Project Syncthing.Integration (.NETStandard,Version=v1.6) was previously compiled. Skipping compilation.
Project SyncthingWeb (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
info: Microsoft.Extensions.DependencyInjection.DataProtectionServices[0]
      User profile is available. Using 'C:\Users\patry\AppData\Local\ASP.NET\DataProtection-Keys' as key repository and Windows DPAPI to encrypt keys at rest.
Hosting environment: Production
Content root path: C:\sw
Now listening on: http://localhost:8385
Application started. Press Ctrl+C to shut down.
```

## Configuration
All basic configuration value is stored in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultMSSQLConnection": "Server=(localdb)\\MSSQLLocalDB;Database=aspnet-SyncthingWeb-d58d8c7f-f8b7-4c3d-94f3-66d1ee2ee957;Trusted_Connection=True;MultipleActiveResultSets=true",
    "DefaultSQLiteConnection": "Filename=SyncthingWebDatabase.db"
  },
  "DatabaseProvider": "sqlite",  
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}

```

You can also specifiy more advanced option via command line arguments.

| Argument     | Description                                  | Default value  | 
| ------------ |:--------------------------------------------:| --------------:|
| --port, -p   | Port number for listening of built-in server | 8385           |

**Attention!**
Remember to allow app to access read permission to syncthing config file and directory with files. Otherwise strange errors may occur (not handled yet :-)) 

### Database providers
Currently, there are two databse providers:
- MSSQL Server
- SQLite (**default**)

## Release notes

### v1.0.1
+ Previewing image files
* Notifications are shown properly

### v1.0.0

* First release

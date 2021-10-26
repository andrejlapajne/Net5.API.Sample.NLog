# NLog configuration within _appsettings.json_ in .NET 5
The purpose of this repository is to provide the simplest example possible of a working configuration of NLog in a **.NET 5.0 webapi project**. 

I wanted to use only the _appsettings.json_ configuration file (as should be with .NET core onwards) to store the NLog configuration, for several reasons (single configuration file, JSON, etc.) and I did not want to duplicate  the connection string to the database in the NLog database target, as it was already specified in an earlier section of the _appsettings.json_ file. 

Trying to achieve that, I came accross only partial documentation and examples... while the result at the end was actually fairly simple because it is already fully supported by NLog. Therefore, I decided to publish this example scenario, gathering all the information I found, as it might be useful to someone hopefully. 

## Sample project setup

I created a new .NET 5.0 web API project using the standard template wizard, out-of-the-box.

Create new project wizard step:

![Create a new project](/images/screenshots/create-new-project.png)

Configure new project wizard step:

![Configure the new project](/images/screenshots/configure-new-project.png)

Additional project information wizard step:

![Additional project information](/images/screenshots/additional-information.png)

## Required configuration to setup NLog

### Add the required NLog packages 

The following NuGet packages are required and must be added to the project:
* NLog: core NLog
* NLog.Extensions.Logging: NLog configuration support
* NLog.Web.AspNetCore: NLog support for .NET core

![NuGet packages](/images/screenshots/NuGet-packages.png)

### Update _Startup.cs_ 

_Startup.cs_  must be slightly adapted to tell NLog to initialize the configuration from _appsettings.json_ instead of default _NLog.config_: 
```C#
...
using NLog.Web;
...
namespace Net5.API.Sample
{
    public class Startup
    {
        ...
        public void ConfigureServices(IServiceCollection services)
        {
            NLog.LogManager.Setup().LoadConfigurationFromAppSettings();

            ...
        }
        ...        
    }
}
``` 

### Add NLog section to _appsettings.json_

The example presented below shows a database and a [SEQ](https://datalust.co/seq) target, with the connection string in the database target reusing the main connection string defined earlier on, in the standard _ConnectionStrings_ section. 

```json
{
    ...
    "ConnectionStrings": {
        "MyDb": "Server=myHost;Initial Catalog=myDb; Trusted_Connection=True;"
    },
    ...
    "NLog": {
        "autoReload": true,
        "throwConfigExceptions": true,
        "extensions": [],
        "targets": {
            "async": true,
            "database": {
                "type": "Database",
                "dbProvider": "Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient",
                "connectionString": "${configsetting:item=ConnectionStrings.MyDb}",
                "commandText": "INSERT INTO [Logs] ([Message], [Level], [Date]) VALUES (@Message, @Level, @Date)",
                "parameters": [
                    {
                        "name": "@Message",
                        "layout": "${message:truncate=1000}"
                    },
                    {
                        "name": "@Level",
                        "layout": "${event-properties:levelId}"
                    },
                    {
                    "name": "@Date",
                    "layout": "${event-properties:date}",
                    "dbType": "DbType.DateTime"
                    }
                ]
            },
            "seq": {
                "type": "Seq",
                "serverUrl": "",
                "apiKey": "",
                "properties": [
                    {
                        "name": "threadId",
                        "value": "${threadid}",
                        "as": "id"
                    },
                    {
                        "name": "name",
                        "value": "${machinename}"
                    }
                ]
            }
        },
        "rules": [
            {
                "logger": "*",
                "minLevel": "Trace",
                "writeTo": "database"
            },
            {
                "logger": "*",
                "minLevel": "Trace",
                "writeTo": "seq"
            }
        ]
    },
    ...
}
```

## References
- [NLog configuration](https://nlog-project.org/config/)
- [NLog configuration file](https://github.com/NLog/NLog/wiki/Configuration-file)
- [NLog logging troubleshooting](https://github.com/NLog/NLog/wiki/Logging-Troubleshooting)
- [NLog new exception handling rules](https://nlog-project.org/2010/09/05/new-exception-handling-rules-in-nlog-2-0.html)
- [NLog configuration with appsettings.json](https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json)
- [NLog _ConfigSetting_ layout renderer](https://github.com/NLog/NLog/wiki/ConfigSetting-Layout-Renderer)
- [StackOverflow - NLog with .NET core and appsettings.json](https://stackoverflow.com/questions/56246416/how-can-i-configure-nlog-in-net-core-with-appsettings-json-instead-of-an-nlog-c)
- [StakOverflow - NLog use connection string name in appsettings.json](https://stackoverflow.com/questions/53977365/nlog-use-connection-string-name-in-appsettings)
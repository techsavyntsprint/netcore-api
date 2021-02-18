# NET Core 5 API

[![Continuous Integration](https://github.com/techsavyntsprint/netcore-api/workflows/CI/badge.svg)](https://github.com/techsavyntsprint/netcore-api/actions)

The solution includes the API to be reused. 

## Build and Test
To build the project, an appsetings.json file, that defines a connection, needs to be added to APICore.API project's root. Also a sample appsettings is provided in order you can duplicat it and adjust the values as per your needs.

## APICore sample_appsettings.json
Is important to note, in the case of Azure, we can use developer tools instead of production environments directly from Azure on our developer machines. To accomplish this you need to use:
1- Install Microsoft Azure Storage Emulator from (https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator).
2- Replace the Azure Connection String with this one "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=[ROOT_PATH];"

```json
{
  "ConnectionStrings": {
    "AlBoteConnection": "Server=[YOUR_SERVER];Database=[YOUR_DATABASE];User Id=[YOUR_USERNAME];Password=[YOUR_PASWORD];",
    "Azure": "DefaultEndpointsProtocol=https;AccountName=[YOUR_ACCOUNT_NAME];AccountKey=[YOUR_ACCOUNT_KEY];BlobEndpoint=[ROOT_PATH];"
  },

  "BearerTokens": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "API_HOST",
    "Audience": "Any",
    "AccessTokenExpirationMinutes": ACCESS_TOKEN_EXPIRATION_TIME,
    "RefreshTokenExpirationMinutes": REFRESH_TOKEN_EXPIRATION_TIME,
    "AllowMultipleLoginsFromTheSameUser": IF_YOU_MAY_ACCEPT_MULTIPLE_LOGINS,
    "AllowSignoutAllUserActiveClients": true
  },

"Blobs": {
    "ImagesRootPath": "[ROOT_PATH]/[CONTAINER_NAME]",
    "ImagesContainer": "[CONTAINER_NAME]"
  },
 "SendGrid": {
    "SendGridKey": "[SENDGRID_KEY]",
	"SendGridUser": "[SENDGRID_USER]",
    "UseSandbox": "true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

# Database
As per requisite you must have installed .Net Core 3 and EF Core Command Line tools (https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet).

We are using the .net migrations system to update the database.

In order to generate migrations for add, delete or update entities execute the following command in the Data project's root CLI:
```
dotnet ef migrations add NameOfTheMigration -s ..\APICore.API
```

Next, in order to reflect these changes in the database execute:
```
dotnet ef database update -s ..\APICore.API
```

#Blob
Need Blob account azure (or use developer storage emulator).
[ROOT_PATH] the root path of the Blob.
[CONTAINER_NAME] name of the image container.


# .NET 9 Core API

[![Continuous Integration](https://github.com/techsavyntsprint/netcore-api/workflows/CI/badge.svg)](https://github.com/techsavyntsprint/netcore-api/actions)

A production-ready ASP.NET Core 9 API solution designed for scalability and reusability across multiple projects.

## Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- **Entity Framework Core 9 CLI Tools** - [Installation Guide](https://learn.microsoft.com/en-us/ef/core/cli/)
- **MySQL Database** (Local or Remote)
- **Azure Storage Account** (Optional - for blob storage features)

## Getting Started

### 1. Configuration Setup

Create an `appsettings.json` file in the `APICore.API` project root. A `sample_appsettings.json` template is provided in the project for reference.

#### Using Azure Storage Emulator (Development Only)

For local development with Azure storage services without a production account:

1. Install [Azure Storage Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
2. Use this connection string in `appsettings.json`:
```
DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=[ROOT_PATH];
```

### 2. Configuration Template

Below is a sample `appsettings.json` configuration template. Replace the placeholder values with your actual configuration:

```json
{
  "ConnectionStrings": {
    "AlBoteConnection": "Server=[YOUR_SERVER];Database=[YOUR_DATABASE];User Id=[YOUR_USERNAME];Password=[YOUR_PASWORD];",
    "Azure": "DefaultEndpointsProtocol=https;AccountName=[YOUR_ACCOUNT_NAME];AccountKey=[YOUR_ACCOUNT_KEY];BlobEndpoint=[ROOT_PATH];"
  },

  "BearerTokens": {
    "Key": "YOUR_SECRET_KEY_MIN_32_CHARACTERS",
    "Issuer": "YOUR_API_HOST",
    "Audience": "Any",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationMinutes": 1440,
    "AllowMultipleLoginsFromTheSameUser": true,
    "AllowSignoutAllUserActiveClients": true
  },

  "Blobs": {
    "ImagesRootPath": "https://[ACCOUNT_NAME].blob.core.windows.net/[CONTAINER_NAME]",
    "ImagesContainer": "[CONTAINER_NAME]"
  },
  "SendGrid": {
    "SendGridKey": "[SENDGRID_API_KEY]",
    "SendGridUser": "[SENDGRID_FROM_EMAIL]",
    "UseSandbox": false
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

## Database Management

This project uses **Entity Framework Core 9** with code-first migrations for database management.

### Creating a Migration

When you add, modify, or delete entities, create a new migration from the `APICore.Data` project directory:

```bash
cd APICore.Data
dotnet ef migrations add YourMigrationName -s ..\APICore.API
```

### Applying Migrations

To update the database with the latest migrations:

```bash
cd APICore.Data
dotnet ef database update -s ..\APICore.API
```

### Removing a Migration

If you need to remove the last migration (before applying it):

```bash
dotnet ef migrations remove -s ..\APICore.API
```

## Project Structure

```
APICore/
├── APICore.API/           # ASP.NET Core main API project
├── APICore.Common/        # Shared DTOs and models
├── APICore.Services/      # Business logic and services
├── APICore.Data/          # EF Core DbContext and migrations
└── APICore.Tests/         # Unit and integration tests
```

## Azure Blob Storage

### Development Setup

For local development, use the **Azure Storage Emulator**:

1. Install [Azure Storage Emulator](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
2. Start the emulator and configure the connection string in `appsettings.json`

### Production Setup

Replace the connection string with your Azure Storage Account credentials:
- **Account Name**: Your Azure Storage Account name
- **Account Key**: Your storage account access key
- **Container Name**: The blob container where images will be stored

## Building and Running

### Build the Solution

```bash
dotnet build
```

### Run the API

```bash
dotnet run --project APICore.API
```

The API will start at `https://localhost:5001` by default.

### Running Tests

```bash
dotnet test
```

## Key Features

- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **MySQL Integration** - Using Pomelo.EntityFrameworkCore.MySql
- ✅ **Azure Blob Storage** - Cloud file management
- ✅ **Email Integration** - SendGrid support for transactional emails
- ✅ **Health Checks** - System and database health monitoring
- ✅ **Swagger/OpenAPI** - Interactive API documentation
- ✅ **Structured Logging** - Serilog integration
- ✅ **AutoMapper** - Object mapping and transformation
- ✅ **Localization** - Multi-language support

## Contributing

Please follow the existing code style and ensure all tests pass before submitting pull requests.

## License

See the [LICENSE](LICENSE) file for more information.


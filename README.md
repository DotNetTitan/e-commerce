# E-commerce API

This project is an E-commerce API built using ASP.NET Core. It provides various functionalities for managing products, categories, and user authentication.

## Table of Contents

- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [API Versioning](#api-versioning)
- [Swagger Documentation](#swagger-documentation)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

To get started with this project, follow these steps:

1. **Clone the repository:**
    ```sh
    git clone https://github.com/your-repo/e-commerce.git
    cd e-commerce
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

3. **Set up the database:**
    Update the connection string in `appsettings.json` and run the migrations:
    ```sh
    dotnet ef database update
    ```

4. **Run the application:**
    ```sh
    dotnet run --project src/Ecommerce.Api
    ```

## Configuration

The application settings are configured based on the environment. The following files are used:

- `appsettings.json`
- `appsettings.{Environment}.json`
- User secrets (optional)

Make sure to update these files with your specific settings.

## API Versioning

The API supports versioning. The default API version is `1.0`. You can specify the version in the URL or use the default version.

```csharp
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
# E-commerce API

This project is an E-commerce API built using ASP.NET Core. It provides various functionalities for managing products, categories, and user authentication.

## Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Configuration](#configuration)
- [API Versioning](#api-versioning)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

To get started with this project, follow these steps:

1. **Clone the repository:**
    ```sh
    git clone https://github.com/DotNetTitan/e-commerce.git
    
    cd e-commerce
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

3. **Set up the database:**

   - **Navigate to the `src` directory:**
    ```sh
    cd src
    ```

   - **Add the initial migration:**
    ```sh
    dotnet ef migrations add InitDatabase --project Ecommerce.Infrastructure -s Ecommerce.Api -c ApplicationDbContext
    ```

   - **Update the database:**
    ```sh
    dotnet ef database update --project Ecommerce.Infrastructure -s Ecommerce.Api -c ApplicationDbContext
    ```

4. **Run the application:**
    ```sh
    dotnet run --project src/Ecommerce.Api
    ```

## Prerequisites

Ensure you have the following installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Configuration

The application settings are configured based on the environment. The following files are used:

- `appsettings.json`
- `appsettings.{Environment}.json`
- User secrets (optional)

Make sure to update these files with your specific settings.

## API Versioning

The API supports versioning. The default API version is `1.0`. You can specify the version in the URL or use the default version.


## Contributing

Contributions are welcome! Please read the contributing guidelines first.

## License

This project is licensed under the MIT License. See the LICENSE file for details.

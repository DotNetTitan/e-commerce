# E-commerce API

![License](https://img.shields.io/badge/License-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)
![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)

Welcome to the **E-commerce API**, a robust and scalable solution built using **ASP.NET Core**. This API provides comprehensive functionalities for managing products, categories, user authentication, orders, reviews, shopping carts, customers, and inventory, making it an ideal backbone for any online retail platform.

## 🚀 Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [Clone the Repository](#clone-the-repository)
  - [Install Dependencies](#install-dependencies)
  - [Set Up the Database](#set-up-the-database)
  - [Run the Application](#run-the-application)
- [Configuration](#configuration)
- [API Versioning](#api-versioning)
- [Authentication](#authentication)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## 🎯 Features

- **Category Management**: Create, update, delete, and list product categories.
- **Product Management**: Add, update, delete, and retrieve product details.
- **Order Management**: Place, cancel, and get order details.
- **Review Management**: Add, update, delete, and list product reviews.
- **Shopping Cart Management**: Add, update, and remove items from the shopping cart.
- **Customer Management**: Edit and view customer details.
- **Inventory Management**: Retrieve and update inventory information.
- **User Authentication**: Register, login, change password, reset password, confirm email, and resend email confirmation with JWT authentication.
- **Rate Limiting**: Protect your API with configurable rate limiting.
- **Exception Handling**: Centralized exception handling for consistent error responses.
- **Swagger Integration**: Comprehensive API documentation with Swagger UI.

## 🛠 Technologies Used

- **.NET 8.0**
- **ASP.NET Core**
- **Entity Framework Core**
- **MediatR**
- **FluentResults**
- **FluentValidation**
- **Swashbuckle (Swagger)**
- **Azure Communication Email**
- **SQL Server**

## 📋 Prerequisites

Before you begin, ensure you have met the following requirements:

- **.NET 8 SDK**: [Download Here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server**: [Download Here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## 🚀 Getting Started

Follow these steps to set up the project on your local machine.

### Clone the Repository

```sh
git clone https://github.com/DotNetTitan/e-commerce.git
cd e-commerce
```

### Install Dependencies

```sh
dotnet restore
```

### Set Up the Database

1. **Navigate to the `src` directory:**

    ```sh
    cd src
    ```

2. **Add the initial migration:**

    ```sh
    dotnet ef migrations add InitDatabase --project Ecommerce.Infrastructure -s Ecommerce.Api -c ApplicationDbContext
    ```

3. **Update the database:**

    ```sh
    dotnet ef database update --project Ecommerce.Infrastructure -s Ecommerce.Api -c ApplicationDbContext
    ```

### Run the Application

```sh
dotnet run --project src/Ecommerce.Api
```

The API will be available at `https://localhost:7045` and `http://localhost:33251`.

## ⚙️ Configuration

The application settings are configured based on the environment. The following files are used:

- `appsettings.json`
- `appsettings.{Environment}.json`
- User secrets (optional)

Ensure to update these files with your specific settings, such as database connection strings, JWT settings, and email configurations.

## 📚 API Versioning

The API supports versioning to manage changes efficiently. The default API version is `1.0`. You can specify the version in the URL or use the default version.

## 🔒 Authentication

The API uses **JWT Bearer Tokens** for authentication. Secure endpoints require a valid JWT token. Follow the authentication endpoints to register and obtain tokens.

## 📄 API Documentation

Interactive API documentation is available via **Swagger UI**. Once the application is running, navigate to:

```
https://localhost:7045/swagger
```

Here, you can explore and test the API endpoints directly from your browser.

## 🤝 Contributing

Contributions are welcome! Please follow these steps to contribute:

1. **Fork the Repository**
2. **Create a Feature Branch**

    ```sh
    git checkout -b feature/YourFeature
    ```

3. **Commit Your Changes**

    ```sh
    git commit -m "Add some feature"
    ```

4. **Push to the Branch**

    ```sh
    git push origin feature/YourFeature
    ```

5. **Open a Pull Request**

Please read the [contributing guidelines](CONTRIBUTING.md) for more details.

## 📝 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.txt) file for details.

## 📧 Contact

For any questions or suggestions, feel free to reach out:

- **GitHub**: [DotNetTitan](https://github.com/DotNetTitan)

---

✨ *Happy Coding!*

# Contributing to E-commerce API

First off, thank you for considering contributing to our E-commerce API project! 🎉 Your participation is highly appreciated and helps us improve the project for everyone.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
  - [Reporting Bugs](#reporting-bugs)
  - [Suggesting Enhancements](#suggesting-enhancements)
  - [Pull Requests](#pull-requests)
- [Style Guides](#style-guides)
  - [.NET Coding Standards](#net-coding-standards)
  - [Commit Messages](#commit-messages)
- [Testing](#testing)
- [License](#license)
- [Contact](#contact)

## Code of Conduct

Please read our [Code of Conduct](CODE_OF_CONDUCT.md) to understand the standards we expect from all contributors.

## How Can I Contribute?

### Reporting Bugs

Bugs are tracked as [GitHub issues](https://github.com/DotNetTitan/e-commerce/issues). Before filing a bug, please ensure that it hasn't already been reported. When reporting a bug, please provide as much detail as possible, including steps to reproduce the issue.

### Suggesting Enhancements

Enhancements and new features are also tracked through [GitHub issues](https://github.com/DotNetTitan/e-commerce/issues). Before suggesting an enhancement, please search existing issues to see if it's already been proposed or implemented.

### Pull Requests

1. **Fork the Repository**

   Click on the [Fork button](https://github.com/DotNetTitan/e-commerce/fork) at the top right of the repository page to fork the project.

2. **Clone Your Fork**

   ```sh
   git clone https://github.com/YourUsername/e-commerce.git
   cd e-commerce
   ```

3. **Create a New Branch**

   It's recommended to create a new branch for each significant change.

   ```sh
   git checkout -b feature/YourFeatureName
   ```

4. **Make Your Changes**

   Ensure your code adheres to the project's coding standards and passes all tests.

5. **Commit Your Changes**

   Follow the [commit message guidelines](#commit-messages).

   ```sh
   git commit -m "Add feature: Your feature description"
   ```

6. **Push to Your Fork**

   ```sh
   git push origin feature/YourFeatureName
   ```

7. **Create a Pull Request**

   Navigate to the original repository and click on the **New Pull Request** button. Provide a clear description of your changes and reference any related issues.

## Style Guides

### .NET Coding Standards

- Follow the [Microsoft .NET Coding Guidelines](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Use **PascalCase** for public members and classes.
- Use **camelCase** for private members and parameters.
- Ensure consistent indentation and spacing.

### Commit Messages

- Use the present tense ("Add feature" not "Added feature").
- Provide a concise description of the changes.
- Reference relevant issues by number (e.g., `Fixes #123`).

Example:

```
Add feature: Implement user registration endpoint

- Added RegisterController with Register action
- Implemented RegisterCommand and RegisterHandler
- Updated integration tests
Fixes #45
```

## Testing

Ensure that all tests pass before submitting a pull request. You can run the tests using the following command:

```sh
dotnet test
```

For new features or bug fixes, please include appropriate unit and integration tests.

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](LICENSE.txt).

## Contact

For any questions or further assistance, feel free to reach out:

- **GitHub**: [DotNetTitan](https://github.com/DotNetTitan)

Thank you for contributing! 🙏

---

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Ecommerce.Api.Tests.E2E
{
    public class AuthenticationControllerE2ETests : IDisposable
    {
        private readonly IWebDriver _driver;

        public AuthenticationControllerE2ETests()
        {
            _driver = new ChromeDriver();
        }

        [Fact]
        public void Register_New_User_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("http://localhost:5000/api/v1/auth/register");

            // Act
            _driver.FindElement(By.Id("UserName")).SendKeys("testuser");
            _driver.FindElement(By.Id("Email")).SendKeys("test@example.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Password123");
            _driver.FindElement(By.Id("RegisterButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("User registered successfully", successMessage);
        }

        [Fact]
        public void Register_User_With_Existing_Email()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/register");

            // Act
            _driver.FindElement(By.Id("UserName")).SendKeys("existinguser");
            _driver.FindElement(By.Id("Email")).SendKeys("existing@example.com");
            _driver.FindElement(By.Id("Password")).SendKeys("Password123");
            _driver.FindElement(By.Id("RegisterButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Email already exists", errorMessage);
        }

        [Fact]
        public async Task Register_User_Too_Many_Requests()
        {
            // Arrange
            await _driver.Navigate().GoToUrlAsync("https://localhost:7045/api/v1/auth/register");

            // Act
            for (int i = 0; i < 601; i++)
            {
                _driver.FindElement(By.Id("UserName")).SendKeys($"testuser");
                _driver.FindElement(By.Id("Email")).SendKeys($"test@example.com");
                _driver.FindElement(By.Id("Password")).SendKeys("Password123");
                _driver.FindElement(By.Id("RegisterButton")).Click();
                await Task.Delay(10); // Small delay to avoid overwhelming the server
            }

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Too many requests", errorMessage);
        }

        [Fact]
        public void Login_User_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/login");

            // Act
            _driver.FindElement(By.Id("UserName")).SendKeys("testuser");
            _driver.FindElement(By.Id("Password")).SendKeys("Password123");
            _driver.FindElement(By.Id("LoginButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("Login successful", successMessage);
        }

        [Fact]
        public void Login_User_With_Invalid_Credentials()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/login");

            // Act
            _driver.FindElement(By.Id("UserName")).SendKeys("testuser");
            _driver.FindElement(By.Id("Password")).SendKeys("WrongPassword");
            _driver.FindElement(By.Id("LoginButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Invalid credentials", errorMessage);
        }

        [Fact]
        public async Task Login_User_Too_Many_Requests()
        {
            // Arrange
            await _driver.Navigate().GoToUrlAsync("https://localhost:7045/api/v1/auth/login");

            // Act
            for (int i = 0; i < 601; i++)
            {
                _driver.FindElement(By.Id("UserName")).SendKeys("testuser");
                _driver.FindElement(By.Id("Password")).SendKeys("WrongPassword");
                _driver.FindElement(By.Id("LoginButton")).Click();
                await Task.Delay(10); // Small delay to avoid overwhelming the server
            }

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Too many requests", errorMessage);
        }

        [Fact]
        public void Confirm_Email_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/confirm-email?token=sampleToken&email=test@example.com");

            // Act
            _driver.FindElement(By.Id("ConfirmButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("Email confirmed successfully", successMessage);
        }

        [Fact]
        public void Confirm_Email_With_Invalid_Token()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/confirm-email?token=invalidToken&email=test@example.com");

            // Act
            _driver.FindElement(By.Id("ConfirmButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Invalid token", errorMessage);
        }

        [Fact]
        public void Forgot_Password_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/forgot-password");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("test@example.com");
            _driver.FindElement(By.Id("ForgotPasswordButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("Password reset link sent successfully", successMessage);
        }

        [Fact]
        public void Forgot_Password_With_Nonexistent_Email()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/forgot-password");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("nonexistent@example.com");
            _driver.FindElement(By.Id("ForgotPasswordButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Email not found", errorMessage);
        }

        [Fact]
        public void Reset_Password_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/reset-password");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("test@example.com");
            _driver.FindElement(By.Id("Token")).SendKeys("sampleToken");
            _driver.FindElement(By.Id("NewPassword")).SendKeys("NewPassword123");
            _driver.FindElement(By.Id("ResetPasswordButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("Password reset successfully", successMessage);
        }

        [Fact]
        public void Reset_Password_With_Invalid_Token()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/reset-password");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("test@example.com");
            _driver.FindElement(By.Id("Token")).SendKeys("invalidToken");
            _driver.FindElement(By.Id("NewPassword")).SendKeys("NewPassword123");
            _driver.FindElement(By.Id("ResetPasswordButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Invalid token", errorMessage);
        }

        [Fact]
        public void Resend_Email_Confirmation_Successfully()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/resend-email-confirmation");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("test@example.com");
            _driver.FindElement(By.Id("ResendEmailConfirmationButton")).Click();

            // Assert
            var successMessage = _driver.FindElement(By.Id("SuccessMessage")).Text;
            Assert.Equal("Email confirmation resent successfully", successMessage);
        }

        [Fact]
        public void Resend_Email_Confirmation_With_Nonexistent_Email()
        {
            // Arrange
            _driver.Navigate().GoToUrl("https://localhost:7045/api/v1/auth/resend-email-confirmation");

            // Act
            _driver.FindElement(By.Id("Email")).SendKeys("nonexistent@example.com");
            _driver.FindElement(By.Id("ResendEmailConfirmationButton")).Click();

            // Assert
            var errorMessage = _driver.FindElement(By.Id("ErrorMessage")).Text;
            Assert.Equal("Email not found", errorMessage);
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
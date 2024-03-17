using Account.Model.DTO;
using Account.Repository.EFC;
using Account.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Test;

public class AccountServiceTests
{
    private readonly DatabaseContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        _dbContext = new DatabaseContext(options);

        // Build configuration from appsettings.json
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        _accountService = new AccountService(_dbContext, _configuration);
    }

    [Fact]
    public async Task Login_ReturnsAccountDto_WhenEmailExists()
    {
        var hashedPass= BCrypt.Net.BCrypt.HashPassword("VerySecurePassword");
        // Arrange
        var loginRequest = new LoginRequestDTO { email = "test@test.com" , passwordUnhashed = "VerySecurePassword"};
        _dbContext.Accounts.Add(new Account.Repository.Entities.Account { Email = "test@test.com", PasswordHashed = hashedPass });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _accountService.Login(loginRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Item1.Email);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenEmailDoesNotExist()
    {
        // Arrange
        var loginRequest = new LoginRequestDTO { email = "nonexistent@test.com" };

        // Act
        var result = await _accountService.Login(loginRequest);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Register_ReturnsAccountDtoAndToken_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerRequest = new RegisterRequestDTO { email = "test@test.com", passwordUnhashed = "password" };

        // Act
        var result = await _accountService.Register(registerRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Item1.Email);
        Assert.NotNull(result.Item2);
    }

    [Fact]
    public async Task GetBalance_ReturnsBalance_WhenAccountExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _dbContext.Accounts.Add(new Account.Repository.Entities.Account { AccId = accountId, Balance = 100, Email = "test@test.com", PasswordHashed = "hashedPassword" });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _accountService.GetBalance(accountId);

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public async Task GetBalance_ThrowsException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _accountService.GetBalance(accountId));
    }

    [Fact]
    public async Task ActivateSubscription_ReturnsAccountDto_WhenAccountExistsAndHasSufficientBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _dbContext.Accounts.Add(new Account.Repository.Entities.Account
        {
            AccId = accountId,
            Balance = 100,
            Email = "test@test.com",
            PasswordHashed = "hashedPassword"
        });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _accountService.ActivateSubscription(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(accountId, result.AccId);
        Assert.Equal(85, result.Balance); // Assuming the monthly subscription price is 15 (it is set from the appsettings.json of the assembly)
    }

    [Fact]
    public async Task ActivateSubscription_ThrowsException_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _accountService.ActivateSubscription(accountId));
    }
}
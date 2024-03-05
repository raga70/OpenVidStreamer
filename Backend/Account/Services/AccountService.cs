using Account.Model.DTO;
using Account.Model.Mappers;
using Account.Repository.EFC;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Account.Services;

public class AccountService(DatabaseContext _accountDbContext, IConfiguration configuration)
{
    private readonly AsyncRetryPolicy _dbRetryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(10));
    
    public async Task<AccountDTO?> Login(LoginRequestDTO request)
    {
        var account = await _accountDbContext.Accounts.FirstOrDefaultAsync(x => x.Email == request.email);
        if (account is null) return null;
        return AccountMapper.AccountToAccountDto(account);
    }
    
    public async Task<Tuple<AccountDTO,string>> Register(RegisterRequestDTO request)
    {
        var account = new Repository.Entities.Account()
        {
        Email = request.email,
        PasswordHashed = BCrypt.Net.BCrypt.HashPassword(request.passwordUnhashed)
        };
        _accountDbContext.Accounts.Add(account);
        
        var authToken = AuthTokenGenerator.GenerateOwnAuthToken(account.AccId.ToString(),configuration);
        var accountDto = AccountMapper.AccountToAccountDto(account);
      
        Task.Run(() => _dbRetryPolicy.ExecuteAsync(async () => await _accountDbContext.SaveChangesAsync()));
        
        return new Tuple<AccountDTO, string>(accountDto, authToken);
    }
    
    public async Task<decimal> GetBalance(Guid accId)
    {
        var account = await _accountDbContext.Accounts.FirstOrDefaultAsync(x => x.AccId == accId);
        if (account is null) throw new Exception("Account not found");
        return account.Balance;
    } 
    
    
    public async Task<AccountDTO> ActivateSubscription(Guid accId)
    {
        var account = await _accountDbContext.Accounts.FirstOrDefaultAsync(x => x.AccId == accId);
        if (account is null) throw new Exception("Account not found");
        account.Balance -= configuration.GetValue<int>("monthlySubscriptionPrice");
        account.SubscriptionValidUntil = DateTime.Now.AddMonths(1);
        await _dbRetryPolicy.ExecuteAsync(async () => await _accountDbContext.SaveChangesAsync());
        return AccountMapper.AccountToAccountDto(account);
    }
    
}
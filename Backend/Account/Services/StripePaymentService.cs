using Account.Model.DTO;
using Account.Repository.EFC;
using System;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Stripe;

namespace Account.Services;
public class StripePaymentService
{

    private readonly string _stripeRedirectUrl;
    
    private readonly DatabaseContext _accountDbContext;
    private readonly IAsyncPolicy<PaymentIntent> _retryPolicy = Policy
        .HandleResult<PaymentIntent>(paymentIntent => paymentIntent.Status == "requires_action" || paymentIntent.Status == "requires_payment_method")
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
        {
            Console.WriteLine($"Payment status: {result.Result.Status}, for transactionID {result.Result.Id}. Retrying in {timeSpan.Seconds} seconds. Attempt {retryCount}.");
        });

    private readonly IConfiguration _configuration;
    public StripePaymentService(DatabaseContext accountDbContext, IConfiguration configuration)
    {
        _accountDbContext = accountDbContext;
        _configuration = configuration;
           _stripeRedirectUrl = Environment.GetEnvironmentVariable("StripeRedirectUrl") ??
            _configuration.GetValue<string>("Stripe:RedirectUrl");
    }

    public async Task<PaymentIntent> ProcessPaymentAsync(IncomingPaymentDTO incomingPayment, string accId)
    {
        var account = _accountDbContext.Accounts.FirstOrDefault(a => a.AccId == Guid.Parse(accId));
        if (account is null)
        {
            throw new Exception("Account not found.");
        }
        var customerService = new CustomerService();
        var customerOptions = new CustomerCreateOptions
        {
            Email = account.Email, 
            PaymentMethod = incomingPayment.StripeToken
        };
        var customer = await customerService.CreateAsync(customerOptions);
        
        
        var paymentIntentService = new PaymentIntentService();
        
        var paymentIntentOptions = new PaymentIntentCreateOptions
        {
            Amount = Convert.ToInt64(incomingPayment.Amount * 100), // Convert to cents
            Currency = "eur",
            PaymentMethod = incomingPayment.StripeToken,
            Description = $"OpenVidStreamer - Payment of monthly subscription for AccountNumber: {accId}",
            Confirm = true,
            UseStripeSdk = true,
            ReturnUrl = _stripeRedirectUrl,
            Customer = customer.Id
        };

        PaymentIntent paymentIntent = await _retryPolicy.ExecuteAsync(async () =>
        {
            return await paymentIntentService.CreateAsync(paymentIntentOptions);
        });

        if (paymentIntent.Status == "succeeded")
        {
            // Update the account balance only if payment succeeds
           // var account = _accountDbContext.Accounts.FirstOrDefault(a => a.AccId == Guid.Parse(accId));
            if (account != null)
            {
                account.Balance += incomingPayment.Amount;
                await _accountDbContext.SaveChangesAsync();
            }
        }

        if (paymentIntent.Status == "requires_action")
            
        {
            // Return necessary information for client to complete the authentication
            return paymentIntent;
        }
        
        
        
        return paymentIntent;
    }
    
    
    public async Task<PaymentIntent> ConfirmPaymentAsync(string paymentIntentId, string accId)
    {
        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);
        
        if (paymentIntent.Status == "succeeded")
        {
            // Update the account balance only if payment succeeds
            var account = _accountDbContext.Accounts.FirstOrDefault(a => a.AccId == Guid.Parse(accId));

            decimal paymentAmountBackInEuros = paymentIntent.Amount / 100;
            if (account != null)
            {
                account.Balance += paymentAmountBackInEuros;
                await _accountDbContext.SaveChangesAsync();
            }
        }
        
        return paymentIntent;
    }
    
}

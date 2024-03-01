using Account.Model.DTO;
using Account.Repository.EFC;

namespace Account.Services;
using Polly;
using Stripe;

public class StripePaymentService(DatabaseContext _accountDbContext)
{
    private readonly IAsyncPolicy<Charge> _retryPolicy  = Policy
        .HandleResult<Charge>(charge => charge.Status == "pending")
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
    {
        Console.WriteLine($"Payment status: {result.Result.Status}, for transactionID {result.Result.Id}. Retrying in {timeSpan.Seconds} seconds. Attempt {retryCount}.");
    });



    public async Task<Charge> ProcessPaymentAsync(IncomingPaymentDTO incomingPayment, string accId) //it`s ok for this to hang the user will wait with us until the transaction is complete
    {
        var chargeOptions = new ChargeCreateOptions
        {
            Amount = Convert.ToInt64(incomingPayment.Amount * 100), // Convert amount to cents
            Currency = "eur",
            Source = incomingPayment.StripeToken, 
            Description = $"OpenVidStreamer - Payment of monthly subscription for AccountNumber: {accId}" 
        };
        
        
        
        
        
        var chargeService = new ChargeService();
        var charge = await _retryPolicy.ExecuteAsync(async () =>
        {
            var charge = await chargeService.CreateAsync(chargeOptions);
            if (charge.Status == "failed")
            {
                throw new Exception("Payment failed.");
            }
            return charge;
        });
        //if we hit this line charge.Status == "succeeded" (_retryPolicy.ExecuteAsync did not throw an exception)
        _accountDbContext.Accounts.FirstOrDefault(x => x.AccId == Guid.Parse(accId)).Balance =incomingPayment.Amount;

        return charge;

    }
}

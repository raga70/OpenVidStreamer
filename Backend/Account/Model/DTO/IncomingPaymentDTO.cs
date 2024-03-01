namespace Account.Model.DTO;

public record IncomingPaymentDTO()
{
    public decimal Amount { get; set; }
    
    public string StripeToken { get; set; }
}
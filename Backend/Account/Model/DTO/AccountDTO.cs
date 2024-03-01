namespace Account.Model.DTO;

public class AccountDTO
{
    public int AccId { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public DateTime SubscriptionValidUntil { get; set; }
}
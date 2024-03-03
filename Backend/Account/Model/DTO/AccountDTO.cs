namespace Account.Model.DTO;

public class AccountDTO
{
    public Guid AccId { get; set; }
    public string Email { get; set; }
    public decimal Balance { get; set; }
    public DateTime SubscriptionValidUntil { get; set; }
}
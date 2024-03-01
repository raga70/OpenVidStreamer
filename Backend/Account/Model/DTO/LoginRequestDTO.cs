namespace Account.Model.DTO;

public record LoginRequestDTO()
{
    public string email { get; set; }
    public string passwordUnhashed { get; set; }
}
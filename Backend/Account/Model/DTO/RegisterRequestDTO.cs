namespace Account.Model.DTO;

public record RegisterRequestDTO()
{
    public string email { get; set; }
    public string passwordUnhashed { get; set; }
}
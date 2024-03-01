namespace Common.MBcontracts;

public class RecommendationVideoResponse
{
    public Guid UserId { get; set; }
    public List<Guid> VideoIds { get; set; }
}
using Microsoft.EntityFrameworkCore;
using RecommendationAlgo.Repository;
using RecommendationAlgo.Repository.EFC;

namespace RecommendationAlgo.Services;

[Obsolete]
public class RecommendationService(RecommendationRepository _repo, DatabaseContext dbContext)
{
    /// <summary>
    /// This Serves as example of how the recommendation algorithm works <br/>
    /// i refactored it into one gigantic query in the repository to merge everything into one operation and do the calculations in the database
    ///<br/>
    /// later i decided to not inject hot unlinked videos into the recommended list
    ///<br/><br/>
    /// for break down look at:<br/>
    ///     <see cref="RecommendationRepository.GetRecommendedVideosBasedOnLikeSimilarity"/><br/>
    ///     <see cref="RecommendationRepository.GetPopularVideos"/><br/>
    ///     <see cref="RecommendationRepository.GetUserPreferredCategories"/>
    /// </summary>
    /// 
    /// <returns>list of videoIds</returns>
    [Obsolete]
    public async Task<List<Guid>> GetRecommendedVideos(Guid accId)
    {
        var favoriteCategories = await _repo.GetUserPreferredCategories(accId);
        var popularVideos = await _repo.GetPopularVideos(100, accId);
        var recomedededVideosBasedOnLikeSimilarity = await _repo.GetRecommendedVideosBasedOnLikeSimilarity(accId, 100);

        // Get the videos that belong to the user's favorite categories
        var favoriteCategoryVideos = await dbContext.VideoStats.Where(vs => favoriteCategories.Contains(vs.Category))
            .Select(vs => vs.VideoId).ToListAsync();

        // Create a dictionary to store the scores of each video
        var videoScores = new Dictionary<Guid, int>();

        // Assign scores to videos based on whether they are in popularVideos, favoriteCategoryVideos, or recomedededVideosBasedOnLikeSimilarity
        foreach (var video in popularVideos.Concat(favoriteCategoryVideos)
                     .Concat(recomedededVideosBasedOnLikeSimilarity))
        {
            var score = 0;

            if (popularVideos.Contains(video))
            {
                score += 1; // Assign a lower score if the video is popular
            }

            if (favoriteCategoryVideos.Contains(video))
            {
                score += 2; // Assign a medium score if the video is in a favorite category
            }

            if (recomedededVideosBasedOnLikeSimilarity.Contains(video))
            {
                score += 3; // Assign a higher score if the video is recommended based on like similarity
            }

            videoScores[video] = score;
        }

        // Sort the videos based on their scores in descending order
        var sortedRecommendedVideos = videoScores.Keys.OrderByDescending(video => videoScores[video]).ToList();

        // Get the most popular videos that are not in the recommended videos list
        var popularVideosNotInRecommended = popularVideos.Except(sortedRecommendedVideos).ToList();

        // Create a random number generator
        var rng = new Random();

        // Inject these popular videos at random positions in the recommended videos list
        foreach (var video in popularVideosNotInRecommended)
        {
            var randomIndex = rng.Next(sortedRecommendedVideos.Count + 1);
            sortedRecommendedVideos.Insert(randomIndex, video);
        }

        return sortedRecommendedVideos;
    }
}
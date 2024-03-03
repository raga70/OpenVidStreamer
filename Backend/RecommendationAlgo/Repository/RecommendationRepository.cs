using Common.MBcontracts;
using Common.Model;
using Microsoft.EntityFrameworkCore;
using RecommendationAlgo.Repository.EFC;
using RecommendationAlgo.Repository.Entities;
using RecommendationAlgo.Repository.Model.DTO;
using RecommendationAlgo.Services;

namespace RecommendationAlgo.Repository;
public class RecommendationRepository(DatabaseContext dbContext)
{
    #region MutationOperations

    public async Task LikeVideo(Guid userId, Guid videoId)
    {
        var watchHistory = await dbContext.WatchHistories.FindAsync(userId, videoId);
        if (watchHistory is null)
        {
            watchHistory = new WatchHistory { UserId = userId, VideoId = videoId, Liked = VideoLikeEnum.Liked };
            dbContext.WatchHistories.Add(watchHistory);
        }
        else
        {
            watchHistory.Liked = VideoLikeEnum.Liked;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DislikeVideo(Guid userId, Guid videoId)
    {
        var watchHistory = await dbContext.WatchHistories.FindAsync(userId, videoId);
        if (watchHistory is null)
        {
            watchHistory = new WatchHistory { UserId = userId, VideoId = videoId, Liked = VideoLikeEnum.Disliked };
            dbContext.WatchHistories.Add(watchHistory);
        }
        else
        {
            watchHistory.Liked = VideoLikeEnum.Disliked;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task AddWatchTime(Guid userId, Guid videoId, decimal watchTime)
    {
        var watchHistory = await dbContext.WatchHistories.FindAsync(userId, videoId);
        if (watchHistory is null)
        {
            watchHistory = new WatchHistory { UserId = userId, VideoId = videoId, WatchedTime = watchTime };
            dbContext.WatchHistories.Add(watchHistory);
        }
        else
        {
            watchHistory.WatchedTime += watchTime;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteVideo(Guid videoId)
    {
        var watchHistories = await dbContext.WatchHistories.Where(w => w.VideoId == videoId).ToListAsync();
        dbContext.WatchHistories.RemoveRange(watchHistories);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserData(Guid userId)
    {
        var watchHistories = await dbContext.WatchHistories.Where(w => w.UserId == userId).ToListAsync();
        dbContext.WatchHistories.RemoveRange(watchHistories);
        await dbContext.SaveChangesAsync();
    }


    public async Task PopulateVideoMetadata(VideoMetadataPopulationRequest videoStats,
        CancellationToken cancellationToken)
    {
        dbContext.VideoStats.Add(new VideoStats()
        {
            VideoId = videoStats.VideoId, VideoLength = videoStats.VideoLength, Category = videoStats.Category
        });
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion MutationOperations


    #region RecomendationAlgo

    public async Task<List<VideoCategory>> GetUserPreferredCategories(Guid userId, int topN = 99999)
    {
        var query = dbContext.WatchHistories.Where(w => w.UserId == userId && w.Liked == VideoLikeEnum.Liked)
            .GroupJoin(dbContext.VideoStats, wh => wh.VideoId, vs => vs.VideoId,
                (wh, vs) => new { WatchHistory = wh, VideoStats = vs.FirstOrDefault() })
            .GroupBy(w => w.WatchHistory.VideoId)
            .Select(g => new { VideoId = g.Key, Category = g.Max(w => w.VideoStats.Category) });

        var preferredCategories = await query.GroupBy(v => v.Category).OrderByDescending(g => g.Count()).Take(topN)
            .Select(g => g.Key).ToListAsync();

        return preferredCategories;
    }

    public async Task<List<Guid>> GetPopularVideos(int topN, Guid? excludeVideosWatchedByAccId = null,
        VideoCategory? category = null)
    {
        // Define a query that calculates the popularity score for each video directly in the database
        var query = dbContext.WatchHistories
            .GroupJoin(dbContext.VideoStats, wh => wh.VideoId, vs => vs.VideoId,
                (wh, vs) => new { WatchHistory = wh, VideoStats = vs.FirstOrDefault() })
            .GroupBy(w => w.WatchHistory.VideoId).Select(g => new
            {
                VideoId = g.Key,
                PopularityScore =
                    (g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) /
                     (decimal)(g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) +
                               g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Disliked))) +
                    (g.Sum(w => w.WatchHistory.WatchedTime) / g.Max(w => w.VideoStats.VideoLength)),
                Category = g.Max(w => w.VideoStats.Category)
            });

        // If a category is provided, filter the videos by the given category
        if (category is not null)
        {
            query = query.Where(v => v.Category == category);
        }

        // If an account ID is provided, exclude videos watched by this account
        if (excludeVideosWatchedByAccId is not null)
        {
            var watchedVideosByAccount = dbContext.WatchHistories.Where(w => w.UserId == excludeVideosWatchedByAccId)
                .Select(w => w.VideoId).ToList();

            query = query.Where(v => !watchedVideosByAccount.Contains(v.VideoId));
        }

        var popularVideos = await query.OrderByDescending(v => v.PopularityScore).Take(topN).Select(v => v.VideoId)
            .ToListAsync();

        return popularVideos;
    }


    /// <summary>
    /// if i liked a video and a different user has liked the same video , it will recommend me a video that he liked , the more such user connections the video should be appear higher in the list
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="topN"></param>
    /// <returns></returns>
    public async Task<List<Guid>> GetRecommendedVideosBasedOnLikeSimilarity(Guid userId, int topN,
        VideoCategory? category = null)
    {
        // Find all the videos that the current user has liked
        var likedVideos = dbContext.WatchHistories.Where(w => w.UserId == userId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.VideoId);

        // Find all the videos that the current user has watched to exclude them from the recommendations
        var watchedVideos = dbContext.WatchHistories.Where(w => w.UserId == userId).Select(w => w.VideoId);

        // Find all the users who have liked the same videos
        var similarUsers = dbContext.WatchHistories
            .Where(w => likedVideos.Contains(w.VideoId) && w.UserId != userId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.UserId);

        // From these users, find all the videos they have liked
        var recommendedVideosQuery = dbContext.WatchHistories
            .Where(w => similarUsers.Contains(w.UserId) && w.Liked == VideoLikeEnum.Liked &&
                        !watchedVideos.Contains(w.VideoId))
            .GroupJoin(dbContext.VideoStats, wh => wh.VideoId, vs => vs.VideoId,
                (wh, vs) => new { WatchHistory = wh, VideoStats = vs.FirstOrDefault() }).Select(g =>
                new { VideoId = g.WatchHistory.VideoId, Count = 1, Category = g.VideoStats.Category });

        // If a category is provided, filter the videos by the given category
        if (category is not null)
        {
            recommendedVideosQuery = recommendedVideosQuery.Where(v => v.Category == category);
        }

        // Group by video id and count the number of times each video appears
        var recommendedVideos = recommendedVideosQuery.GroupBy(w => w.VideoId)
            .Select(g => new { VideoId = g.Key, Count = g.Sum(w => w.Count) });

        // Sort the videos by the count in descending order and take the top N
        var topRecommendedVideos = await recommendedVideos.OrderByDescending(v => v.Count).Take(topN)
            .Select(v => v.VideoId).ToListAsync();

        return topRecommendedVideos;
    }

    /// <summary>
    /// Retrieves a list of video recommendations for a specific user based on a combination of factors.
    /// The method identifies videos that the user has liked and finds other users who have liked the same videos.
    /// It calculates a popularity score for each video in the database based on the number of likes, dislikes, and the total watch time.
    /// The method also identifies the top 5 categories of videos that the user prefers.
    /// It then combines the popularity score, the like similarity score, and a score based on whether the video's category is in the user's top 5 categories.
    /// The videos are then sorted by this final score in descending order and the top N videos are returned.
    /// </summary>
    public async Task<List<Guid>> GetAlgoRecommendedVideos(Guid accId, int topN)
    {
        // Find all the videos that the current user has liked
        var likedVideos = dbContext.WatchHistories.Where(w => w.UserId == accId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.VideoId);

        // Find all the users who have liked the same videos
        var similarUsers = dbContext.WatchHistories
            .Where(w => likedVideos.Contains(w.VideoId) && w.UserId != accId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.UserId);

        // Define a query that calculates the popularity score for each video directly in the database
        var query = dbContext.WatchHistories
            .GroupJoin(dbContext.VideoStats, wh => wh.VideoId, vs => vs.VideoId,
                (wh, vs) => new { WatchHistory = wh, VideoStats = vs.FirstOrDefault() })
            .GroupBy(w => w.WatchHistory.VideoId).Select(g => new
            {
                VideoId = g.Key,
                PopularityScore =
                    (g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) /
                     (decimal)(g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) +
                               g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Disliked))) +
                    (g.Sum(w => w.WatchHistory.WatchedTime) / g.Max(w => w.VideoStats.VideoLength)),
                LikeSimilarityScore = dbContext.WatchHistories
                    .Where(w => similarUsers.Contains(w.UserId) && w.Liked == VideoLikeEnum.Liked &&
                                !likedVideos.Contains(w.VideoId)).Count(),
                Category = g.Max(w => w.VideoStats.Category)
            });

        // Get the top 5 categories of the user
        var topCategories = await GetUserPreferredCategories(accId, 5);

        // Combine the popularity score, the like similarity score, and the category score to get a final score
        var finalQuery = query.Select(v => new
        {
            VideoId = v.VideoId,
            FinalScore = (v.PopularityScore * (decimal)0.5) + (v.LikeSimilarityScore * 2) +
                         (topCategories.Contains(v.Category) ? 1 : 0)
        });

        // Sort the videos by the final score in descending order and take the top N
        var recommendedVideos = await finalQuery.OrderByDescending(v => v.FinalScore).Take(topN).Select(v => v.VideoId)
            .ToListAsync();

        return recommendedVideos;
    }

/// <summary>
/// does the same as <see cref="GetAlgoRecommendedVideos"/> but filters by category
///
/// for stramed line view view  <see cref="RecommendationService.GetRecommendedVideos"/>
/// </summary>
/// <param name="accId"></param>
/// <param name="topN"></param>
/// <param name="category"></param>
/// <returns></returns>
    public async Task<List<Guid>> GetAlgoRecommendedVideosForCategory(Guid accId, int topN, VideoCategory category)
    {
        
        // Find all the videos that the current user has liked
        var likedVideos = dbContext.WatchHistories.Where(w => w.UserId == accId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.VideoId);

        // Find all the users who have liked the same videos
        var similarUsers = dbContext.WatchHistories
            .Where(w => likedVideos.Contains(w.VideoId) && w.UserId != accId && w.Liked == VideoLikeEnum.Liked)
            .Select(w => w.UserId);

        // Define a query that calculates the popularity score for each video directly in the database
        var query = dbContext.WatchHistories
            .GroupJoin(dbContext.VideoStats, wh => wh.VideoId, vs => vs.VideoId,
                (wh, vs) => new { WatchHistory = wh, VideoStats = vs.FirstOrDefault() })
            .GroupBy(w => w.WatchHistory.VideoId).Select(g => new
            {
                VideoId = g.Key,
                PopularityScore =
                    (g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) /
                     (decimal)(g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Liked) +
                               g.Count(w => w.WatchHistory.Liked == VideoLikeEnum.Disliked))) +
                    (g.Sum(w => w.WatchHistory.WatchedTime) / g.Max(w => w.VideoStats.VideoLength)),
                LikeSimilarityScore = dbContext.WatchHistories
                    .Where(w => similarUsers.Contains(w.UserId) && w.Liked == VideoLikeEnum.Liked &&
                                !likedVideos.Contains(w.VideoId)).Count(),
                Category = g.Max(w => w.VideoStats.Category)
            });

        // Filter the videos by the given category
        query = query.Where(v => v.Category == category);

        // Combine the popularity score and the like similarity score to get a final score
        var finalQuery = query.Select(v => new
        {
            VideoId = v.VideoId, FinalScore = (v.PopularityScore * (decimal)0.5) + (v.LikeSimilarityScore * 2)
        });

        // Sort the videos by the final score in descending order and take the top N
        var recommendedVideos = await finalQuery.OrderByDescending(v => v.FinalScore).Take(topN).Select(v => v.VideoId)
            .ToListAsync();

        return recommendedVideos;
    }

    #endregion RecomendationAlgo


    public async Task<int> GetVideoLikeCount(Guid videoId, VideoLikeEnum like)
    {
        return await dbContext.WatchHistories.CountAsync(w => w.VideoId == videoId && w.Liked == like);
    }

    public async Task<decimal> getTotalVideoWatchTime(Guid videoId)
    {
        return await dbContext.WatchHistories.Where(w => w.VideoId == videoId).SumAsync(w => w.WatchedTime);
    }

    public async Task<int> GetVideoWatchCount(Guid videoId)
    {
        return await dbContext.WatchHistories.CountAsync(w => w.VideoId == videoId);
    }


    public async Task<VideoStatistics> GetVideoStatistics(Guid videoId)
    {
        var videoStats = await dbContext.VideoStats.FindAsync(videoId);
        if (videoStats is null)
        {
            return null;
        }

        var statistics = await dbContext.WatchHistories.Where(w => w.VideoId == videoId).GroupBy(w => w.VideoId).Select(
            g => new
            {
                LikeCount = g.Count(w => w.Liked == VideoLikeEnum.Liked),
                DislikeCount = g.Count(w => w.Liked == VideoLikeEnum.Disliked),
                TotalWatchTime = g.Sum(w => w.WatchedTime),
                Views = g.Count()
            }).FirstOrDefaultAsync();

        if (statistics is null)
        {
            return null;
        }

        return new VideoStatistics
        {
            VideoId = videoId,
            LikeCount = statistics.LikeCount,
            DislikeCount = statistics.DislikeCount,
            TotalWatchTime = statistics.TotalWatchTime,
            Views = statistics.Views,
            Category = videoStats.Category
        };
    }
}
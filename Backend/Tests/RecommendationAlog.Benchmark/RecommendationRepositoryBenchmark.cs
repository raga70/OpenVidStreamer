using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore;
using RecommendationAlgo.Repository;
using RecommendationAlgo.Repository.EFC;

public class RecommendationRepositoryBenchmark
{
    private RecommendationRepository _repository;
    private RecommendationRepositoryOLD _repositoryOld;
    private Guid _userId;
    private int _topN = 50; // Adjust based on your needs

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        ConfigureRedis(services);
        ConfigureDatabase(services);

        var serviceProvider = services.BuildServiceProvider();

        // Assuming your repository and old repository have similar constructors
        _repository = new RecommendationRepository(
            serviceProvider.GetService<DatabaseContext>(),
            serviceProvider.GetService<IDistributedCache>());

        _repositoryOld = new RecommendationRepositoryOLD(
            serviceProvider.GetService<DatabaseContext>());

        _userId = new Guid("08dc4207-fec0-4e97-8f80-3ae5052a305a"); 
    }

    private void ConfigureRedis(IServiceCollection services)
    {
        // Adapt this part based on your actual configuration needs
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379"; 
        });
    }

    private void ConfigureDatabase(IServiceCollection services)
    {
        var serverVersion = new MariaDbServerVersion(new Version(10, 4, 24));
        services.AddDbContext<DatabaseContext>(options =>
            options.UseMySql("server=127.0.0.1;uid=admin;pwd=12345;database=StatisticsDB;", serverVersion)); 
    }

    [Benchmark]
    public async Task GetUserPreferredCategories_New()
    {
        await _repository.GetUserPreferredCategories(_userId, _topN);
    }

    [Benchmark]
    public async Task GetUserPreferredCategories_Old()
    {
        await _repositoryOld.GetUserPreferredCategories(_userId, _topN);
    }
    
    [Benchmark]
    public async Task GetPopularVideosInGeneral_New()
    {
        await _repository.GetPopularVideos(_topN);
    }
    
    [Benchmark]
    public async Task GetPopularVideosInGeneral_Old()
    {
        await _repositoryOld.GetPopularVideos(_topN);
    }
    
    [Benchmark]
    public async Task GetPopularVideosWithExcludedWatchedForUser_New()
    {
        await _repository.GetPopularVideos(_topN,_userId);
    }
    
    [Benchmark]
    public async Task GetPopularVideosWithExcludedWatchedForUser_Old()
    {
        await _repositoryOld.GetPopularVideos(_topN,_userId);
    }
    
    [Benchmark]
    public async Task GetAlgoRecommendationsForUser_New()
    {
        await _repository.GetAlgoRecommendedVideos(_userId, _topN);
    }
    
    [Benchmark]
    public async Task GetAlgoRecommendationsForUser_Old()
    {
        await _repositoryOld.GetAlgoRecommendedVideos(_userId, _topN);
    }
    
}

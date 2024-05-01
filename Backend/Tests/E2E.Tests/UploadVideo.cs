using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace E2E.Tests;

public class UploadVideo
{
    IWebDriver _driver = new ChromeDriver();
    LoginTests login;

    public UploadVideo()
    {
        login = new LoginTests(_driver);
        _driver.Navigate().GoToUrl("http://145.220.74.148:3000/");
    }

    [Fact]
    public void Upload()
    {
        var repoRoot = GetRepositoryRoot();
        var videoFilePath = Path.Combine(repoRoot, "tmp", "SampleVideo1mb.mp4");
        var thumbnailFilePath = Path.Combine(repoRoot, "tmp", "sampleThumbnail.jpg");
        login.Login();
        _driver.Navigate().GoToUrl("http://145.220.74.148:3000/upload");
        _driver.FindElement(UploadPage.TitleInput).SendKeys($"Test-{new Guid().ToString()}");
        _driver.FindElement(UploadPage.DescriptionInput).SendKeys("Test Description, Lorem Ipsum Dolor Sit Amet Consectetur Adipiscing Elit Sed Do Eiusmod Tempor Incididunt Ut Labore Et Dolore Magna Aliqua Ut Enim Ad Minim Veniam Quis Nostrud Exercitation Ullamco Laboris Nisi Ut Aliquip Ex Ea Commodo Consequat");
        _driver.FindElement(UploadPage.CategoryInput).SendKeys("Music");
        _driver.FindElement(UploadPage.SelectVideoButton).SendKeys(videoFilePath);
        _driver.FindElement(UploadPage.SelectThumbnail).SendKeys(thumbnailFilePath);
        _driver.FindElement(UploadPage.UploadButton).Click();
        Thread.Sleep(7000);
        var alert = _driver.SwitchTo().Alert();
        var text = alert.Text;
        Assert.Contains("Video uploaded successfully!", text);
        alert.Accept();
        _driver.SwitchTo().DefaultContent();
    }
    
    
    string GetRepositoryRoot()
    {
        // This could be an environment variable or a static path
        return Environment.GetEnvironmentVariable("REPO_ROOT") ?? throw new InvalidOperationException("Repository root not set.");
    }
    
}
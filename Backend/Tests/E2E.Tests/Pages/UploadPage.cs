using OpenQA.Selenium;

namespace E2E.Tests;

public static class UploadPage
{
    public static By TitleInput = By.XPath("""//*[@id="root"]/div[2]/form/div[1]/input""");
    public static By DescriptionInput = By.XPath("""//*[@id="root"]/div[2]/form/div[2]/textarea""");
    public static By CategoryInput = By.XPath("""//*[@id="root"]/div[2]/form/div[3]/select""");
    public static By SelectVideoButton = By.XPath("""//*[@id="root"]/div[2]/form/div[4]/input[1]""");
    public static By SelectThumbnail = By.XPath("""//*[@id="root"]/div[2]/form/div[4]/input[2]""");
    public static By UploadButton = By.XPath("""//*[@id="root"]/div[2]/form/button""");
}
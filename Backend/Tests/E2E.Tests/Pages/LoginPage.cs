using OpenQA.Selenium;

namespace E2E.Tests;

public static class LoginPage
{
    public static  By UsernameInput = By.XPath("""//*[@id="root"]/div[2]/div/div/article/label[1]/input""");
    public static  By PasswordInput = By.XPath("""//*[@id="root"]/div[2]/div/div/article/label[2]/input""");
    public static  By LoginButton = By.XPath("""//*[@id="root"]/div[2]/div/div/article/section/button[2]"""); //after switch
    public static  By RegisterButton = By.XPath("""//*[@id="root"]/div[2]/div/div/article/section/button[1]"""); //after switch
 
    public static By AcceptEula = By.XPath("""//*[@id=":r0:"]/div/div[3]/button/span"""); //only available after registe pressed
  
}
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace E2E.Tests;

public class LoginTests
{
    private IWebDriver _driver; //= new ChromeDriver();
    
    
    public LoginTests(IWebDriver? driver = null)
    {
        if (driver is not null)
            _driver = driver;
        else
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl("http://145.220.74.148:3000/");
    }
    
    [Fact]
    public void Login()
    {
        Thread.Sleep(1000);
        string usernameText = "raga70@abv.bg";
        IWebElement usernameInput = _driver.FindElement(LoginPage.UsernameInput);
        foreach (var c in usernameText)
        {
            usernameInput
                .SendKeys(c.ToString());
            Thread.Sleep(200);
        }
            
            
       // _driver.FindElement(LoginPage.PasswordInput).SendKeys("1234");
       string passwordText = "1234";
       IWebElement passwordInput = _driver.FindElement(LoginPage.PasswordInput);
       foreach (var c in passwordText)
       {
           passwordInput
               .SendKeys(c.ToString());
              Thread.Sleep(200);
       }
       
       
       
        _driver.FindElement(LoginPage.LoginButton).Click();
        Thread.Sleep(5000);
        Assert.NotNull(_driver.FindElement(Common.LogoutButton));
    }
    
    [Fact]
    public void Logout()
    {
        Login();
        _driver.FindElement(Common.LogoutButton).Click();
        Thread.Sleep(5000);
        Assert.NotNull(_driver.FindElement(LoginPage.LoginButton));
    }

    [Fact]
    public void Register()
    {
        Thread.Sleep(1000);
       // _driver.FindElement(LoginPage.UsernameInput).SendKeys($"Tester-{new Guid().ToString()}@abv.bg");
       IWebElement usernameInput = _driver.FindElement(LoginPage.UsernameInput);
       string usernameText = $"Tester-{Guid.NewGuid().ToString()}@abv.bg";
       foreach (var c in usernameText)
       {
              usernameInput
                .SendKeys(c.ToString());
                  Thread.Sleep(200);
       }
       // _driver.FindElement(LoginPage.PasswordInput).SendKeys("1234");
       IWebElement passwordInput = _driver.FindElement(LoginPage.PasswordInput);
       string passwordText = "1234";
       foreach (var c in passwordText)
       {
           passwordInput
               .SendKeys(c.ToString());
              Thread.Sleep(600);
       }
       
       
        _driver.FindElement(LoginPage.RegisterButton).Click();
        Thread.Sleep(1500);
        _driver.FindElement(LoginPage.AcceptEula).Click();
        Thread.Sleep(5000);
        Assert.NotNull(_driver.FindElement(Common.LogoutButton));
    }
}
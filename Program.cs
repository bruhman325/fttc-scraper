
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Newtonsoft.Json;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

startDriver();
void startDriver()
{
    ChromeOptions options = new ChromeOptions();

    IWebDriver driver = new ChromeDriver(options);
    
    driver.Navigate().GoToUrl("https://fttc.instructure.com/login/ldap");

    new Actions(driver).Pause(TimeSpan.FromSeconds(5)).Perform();
    IWebElement clickable = driver.FindElement(By.Name("pseudonym_session[unique_id]"));
    new Actions(driver)
        .MoveToElement(clickable)
        .SendKeys("censored")
        .SendKeys(Keys.Return)
        .SendKeys("censored")
        .SendKeys(Keys.Return)
        .Pause(TimeSpan.FromSeconds(2))
        .Perform();
    driver.Navigate().GoToUrl("https://fttc.instructure.com/courses/564");


    string[] toScrape = File.ReadAllLines("./filesToScrape.txt");
    List<string> scrapedUrls = new List<string>(); 

    foreach (string title in toScrape) {
        if (title.ToLower().Equals("attachment")) goto FuckYou;

        //Console.WriteLine(string.Format("//span[contains(@title, '{0}')]", title));
        IWebElement element = driver.FindElement(By.XPath(string.Format("//span[contains(@title, '{0}')]", title)));

        IWebElement parent = element.FindElement(By.XPath("parent::*"));

        string url = parent.FindElement(By.TagName("a")).GetAttribute("href");


        scrapedUrls.Add(url);
        FuckYou:;
        {; }
    }
    Console.WriteLine(scrapedUrls.Count);

    foreach (string url in scrapedUrls)
    {
        new Actions(driver).Pause(TimeSpan.FromSeconds(2)).Perform();
        driver.Navigate().GoToUrl(url);
        new Actions(driver).Pause(TimeSpan.FromSeconds(2)).Perform();


        string downloadUrl = driver.Url.Split("?")[0];
        downloadUrl = downloadUrl + "/download?download_frd=1";
        
        driver.Navigate().GoToUrl(downloadUrl);
        


    }
    


}

/*
         IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
        Object aa = executor.ExecuteScript("var items = {}; for (index = 0; index < arguments[0].attributes.length; ++index) { items[arguments[0].attributes[index].name] = arguments[0].attributes[index].value }; return items;", element);
        var json = JsonConvert.SerializeObject(aa);
        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        foreach(string key in dictionary.Keys)
        {
            Console.Write(key);
        } 
 */
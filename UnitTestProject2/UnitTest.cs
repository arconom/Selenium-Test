using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    [TestClass]
    class GoogleTests
    {
        public void Execute()
        {
            // Create a new instance of the Firefox driver.
            // Note that it is wrapped in a using clause so that the browser is closed 
            // and the webdriver is disposed (even in the face of exceptions).

            // Also note that the remainder of the code relies on the interface, 
            // not the implementation.

            // Further note that other drivers (InternetExplorerDriver,
            // ChromeDriver, etc.) will require further configuration 
            // before this example will work. See the wiki pages for the
            // individual drivers at http://code.google.com/p/selenium/wiki
            // for further information.

            //i have a lot of browsers installed
            using (IWebDriver driver = new OperaDriver())
            {
                GoogleSearchCheese(driver);
            }
            using (IWebDriver driver = new ChromeDriver())
            {
                GoogleSearchCheese(driver);
            }
            using (IWebDriver driver = new InternetExplorerDriver())
            {
                GoogleSearchCheese(driver);
            }
            using (IWebDriver driver = new FirefoxDriver())
            {
                GoogleSearchCheese(driver);
            }
        }

        /// <summary>
        /// Goes to google and searches for cheese.
        /// </summary>
        /// <param name="driver">The driver, usually one of the WebDrivers implemented for Selenium.</param>
        [TestMethod]
        private static void GoogleSearchCheese(IWebDriver driver)
        {
            //Notice navigation is slightly different than the Java version
            //This is because 'get' is a keyword in C#
            driver.Navigate().GoToUrl("http://www.google.com/");

            // Find the text input element by its name
            IWebElement query = driver.FindElement(By.Name("q"));

            // Enter something to search for
            query.SendKeys("Cheese");

            // Now submit the form. WebDriver will find the form for us from the element
            query.Submit();

            // Google's search is rendered dynamically with JavaScript.
            // Wait for the page to load, timeout after 10 seconds
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.Title.StartsWith("cheese", StringComparison.OrdinalIgnoreCase));

            // Should see: "Cheese - Google Search" (for an English locale)
            Console.WriteLine("Page title is: " + driver.Title);

        }
    }
}
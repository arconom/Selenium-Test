using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;

// Requires reference to WebDriver.Support.dll
using OpenQA.Selenium.Support.UI;
using System;

namespace Tests
{
    [TestClass]
    public static class GoogleTests
    {
        public static void Execute()
        {
            //i have a lot of browsers installed
            try
            {
                using (IWebDriver driver = new OperaDriver())
                {
                    ExecuteTests(driver);
                }
            }
            catch
            {
                Console.WriteLine("Opera exploded");
            }

            try
            {
                using (IWebDriver driver = new ChromeDriver())
                {
                    ExecuteTests(driver);
                }
            }
            catch
            {
                Console.WriteLine("Chrome exploded");
            }

            //IE will fail because it is useless
            try
            {
                using (IWebDriver driver = new InternetExplorerDriver())
                {
                    ExecuteTests(driver);
                }
            }
            catch
            {
                Console.WriteLine("IE exploded, as usual");
            }

            try
            {
                using (IWebDriver driver = new FirefoxDriver())
                {
                    ExecuteTests(driver);
                }
            }
            catch
            {
                Console.WriteLine("Firefox probably claimed all the system's memory and locked it up.  Use a good browser instead.");
            }
        }

        private static void ExecuteTests(IWebDriver driver)
        {
            //not needed for the test spec
            //GoogleSearchCheese(driver);
            CheckLinks(driver);
            ClickAbout(driver);
            CheckToolTip(driver);
            CheckDOMManipulation(driver);
            Console.WriteLine("All tests complete: " + driver.GetType().ToString());
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
            //this fails in IE for some reason
            //looks like the driver for IE is busted
            //a lot of exceptions in the debugger
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

        /// <summary>
        /// Make sure the “Advertising”, “Business”, and “About” links at the bottom of the page exist.
        /// </summary>
        /// <param name="driver">This is the driver to be used to interface with the browser.</param>
        [TestMethod]
        private static void CheckLinks(IWebDriver driver)
        {
            //Notice navigation is slightly different than the Java version
            //This is because 'get' is a keyword in C#
            driver.Navigate().GoToUrl("http://www.google.com/");

            IWebElement advert = driver.FindElement(By.LinkText("Advertising"));
            IWebElement business = driver.FindElement(By.LinkText("Business"));
            IWebElement about = driver.FindElement(By.LinkText("About"));

            Assert.AreEqual(true, advert != null);
            Assert.AreEqual(true, business != null);
            Assert.AreEqual(true, about != null);
            Console.WriteLine("CheckLinks complete: " + driver.GetType().ToString());
        }

        /// <summary>
        /// Make sure Clicking on the “About” link at the bottom brings up the About page.
        /// </summary>
        /// <param name="driver">The driver.</param>
        [TestMethod]
        private static void ClickAbout(IWebDriver driver)
        {
            const string ABOUTTITLE = "About Us | Google";

            driver.Navigate().GoToUrl("http://www.google.com/");
            IWebElement about = driver.FindElement(By.LinkText("About"));

            about.Click();

            //if the title of the page changed to the value in the constant, then the page should have at least started loading markup
            Assert.AreEqual(ABOUTTITLE, driver.Title);
            Console.WriteLine("ClickAbout complete: " + driver.GetType().ToString());
        }

        /// <summary>
        /// Make sure Hovering over the mic icon (at the right end of the search box) will cause a “Search by voice” tool tip to pop up.
        ///    NOTICE:  I changed this test because I didn't have a mic icon.
        ///    Instead, I am testing for the tooltip of the goole apps link at the top right of the screen.
        /// </summary>
        /// <param name="driver">The driver.</param>
        [TestMethod]
        private static void CheckToolTip(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("http://www.google.com/");

            const string GOOGLEAPPSLINKXPATH = "//*[@id=\"gbwa\"]/div[1]/a";
            IWebElement account = driver.FindElement(By.XPath(GOOGLEAPPSLINKXPATH));

            string titleText = account.GetAttribute("title");
            string altText = account.GetAttribute("alt");

            //Let's make sure title or alt is set because either will work
            Assert.AreEqual(true, String.IsNullOrEmpty(titleText) ? String.IsNullOrEmpty(altText) ? false : true : true);
            Console.WriteLine("CheckToolTip complete: " + driver.GetType().ToString());
        }

        /// <summary>
        ///  Bonus: Make sure Entering text will immediately take the user to the results page and begin to populate the auto-complete suggestions in a pop up menu.
        /// </summary>
        /// <param name="driver">The driver.</param>
        [TestMethod]
        private static void CheckDOMManipulation(IWebDriver driver)
        {
            const string SUGGESTIONCONTAINERDIVXPATH = "//*[@id=\"sbtc\"]/div[2]/div[2]/div[1]";
            const string FIRSTSUGGESTIONXPATH = "//*[@id=\"sbtc\"]/div[2]/div[2]/div[1]/div/ul/li[1]";
            const string HIDDEN = "display: none;";

            driver.Navigate().GoToUrl("http://www.google.com/");

            IWebElement query = driver.FindElement(By.Name("q"));
            IWebElement suggestionContainer = driver.FindElement(By.XPath(SUGGESTIONCONTAINERDIVXPATH));

            // Enter something to search for
            query.SendKeys("C");

            // Wait for the suggestion list to be written to the DOM, timeout after 3 seconds
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementExists(By.XPath(FIRSTSUGGESTIONXPATH)));

            //we also need to make sure that the container of the suggestions is visible.
            Assert.AreNotEqual(HIDDEN, suggestionContainer.GetAttribute("style"));
            Console.WriteLine("CheckDOMManipulation complete: " + driver.GetType().ToString());
        }
    }
}
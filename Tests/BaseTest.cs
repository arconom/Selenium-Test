/*
* For information about the Screenplay pattern,
* http://thucydides.info/docs/serenity-staging/#_serenity_and_the_screenplay_pattern
*
* and some good practices
* https://www.blazemeter.com/blog/top-15-ui-test-automation-best-practices-you-should-follow
*/
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Selenium.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        [ClassCleanup]
        public static void ClassCleanup()
        {
        }
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
        }
        protected IWebDriver driver;
        [TestInitialize]
        //@Before
        public virtual void Initialize()
        {
            this.driver = WebDriverFactory
                     .getWebDriver(false);
        }
        [TestCleanup]
        //@After
        public virtual void Cleanup()
        {
            this.driver?.Close();
            this.driver?.Quit();
            this.driver?.Dispose();
        }
    }
}

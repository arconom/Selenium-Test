import Utility;
import TemplateJSP;
import org.junit.AfterClass;
import static org.junit.Assert.assertEquals; 
import org.junit.BeforeClass; 
import org.junit.Test; 
import org.openqa.selenium.chrome.ChromeDriver;

 public class JSPTestClassName extends TemplateJSP{

	// use these if you have a lot of tests on one page and need to reuse parts of the URL
	// string host = "www.google.com";
	// string path = "/path/to/page.jsp";
	// string queryString = "?a=1&b=2";
	// string port = ":80";
	// string protocol = "https://";

	WebDriver driver;


	
    /***************************************************************************
     * This section is here for construction / destruction of this class/tests *
     **************************************************************************/
        
    @BeforeClass
    public static void setUpClass() {
		driver = new ChromeDriver();
		//this navigates to the page to be tested
        driver.get("https://www.google.com");
		
    }

    @AfterClass
    public static void tearDownClass() {
        driver.quit();
    }

    /***************************************************************************
     *          This section is to test page specific requirements             *
     **************************************************************************/

    @Test
	/*
	This test checks the header to see if it contains a string	
	*/
    private static void testHeader() throws Exception {
		
		
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
        @Test
        private static void CheckLinks(IWebDriver driver)
        {
            IWebElement advert = driver.FindElement(By.LinkText("Advertising"));
            IWebElement business = driver.FindElement(By.LinkText("Business"));
            IWebElement about = driver.FindElement(By.LinkText("About"));

			// assert your conditions here. failed assertions raise exceptions
            Assert.AreEqual(true, advert != null);
            Assert.AreEqual(true, business != null);
            Assert.AreEqual(true, about != null);
            Console.WriteLine("CheckLinks complete: " + driver.GetType().ToString());
        }
	
}

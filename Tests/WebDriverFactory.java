
/**
This class handles building WebDrivers with the correct configuration.

*/
public class WebDriverFactory{

	/**
	Get a webdriver with appropriate config.
	param headless boolean - set to true to run headless.
	
	todo - add a parameter to specify the browser
	*/
public static WebDriver getWebDriver(boolean headless) {
        WebDriver driver = null;

        if (headless) {
            ChromeOptions options = setupWebDriverHeadless();
            driver = new ChromeDriver(options);
        } else {
            ChromeOptions options = setupWebDriver();
            driver = new ChromeDriver(options);
        }
        return driver;
    }

/**
this needs to point to the location of the webdriver file on your machine
*/
private static String getNonDynamicLocation(){
            return "(chromedriverpath)\\chromedriver2.37.exe";
    }

private static void setupWebDriverProperty() {
        //Turn off the proxy before you do anything, otherwise a lot of tests will error out
        System.setProperty("java.net.useSystemProxies", "false");
        if (System.getProperty("webdriver.chrome.driver") == null) {
            System.setProperty("webdriver.chrome.driver", getNonDynamicLocation());
        } 
    }
	
	private static ChromeOptions setupWebDriver() {
        ChromeOptions options = new ChromeOptions();
        return options;
    }

	
private static ChromeOptions setupWebDriverHeadless() {
        ChromeOptions options = setupWebDriver();
        options.addArguments("headless");
        options.addArguments("disable-gpu");
        options.setExperimentalOption("useAutomationExtension", false);
        //options.addArguments("window-size=1200x600");
        return options;
    }
}
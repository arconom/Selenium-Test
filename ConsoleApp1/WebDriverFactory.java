package common;

import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.ie.InternetExplorerDriver;
import org.openqa.selenium.ie.InternetExplorerOptions;

/**
 * This class handles building WebDrivers with the correct configuration.
 *
 */
public class WebDriverFactory {

	/**
	 * Get a webdriver with appropriate config. param headless boolean - set to
	 * true to run headless.
	 *
	 * todo - add a parameter to specify the browser
	 */
	public static WebDriver getChromeWebDriver(boolean headless) {
		WebDriverFactory.setupWebDriverProperty();
		WebDriver driver = null;

		if (headless) {
			ChromeOptions options = WebDriverFactory.setupWebDriverHeadless();
			driver = new ChromeDriver(options);
		} else {
			ChromeOptions options = WebDriverFactory.setupWebDriver();
			driver = new ChromeDriver(options);
		}
		return driver;
	}

	public static WebDriver getIEWebDriver() {
		WebDriverFactory.setupWebDriverProperty();
		// todo look into accepting ssl certs
		InternetExplorerOptions options = new InternetExplorerOptions();
		options.setCapability("AcceptSslCerts", true);
		options.setCapability("unexpectedAlertBehaviour", "accept");
		WebDriver driver = new InternetExplorerDriver(options);
		return driver;
	}

	private static ChromeOptions setupWebDriver() {
		ChromeOptions options = new ChromeOptions();
		options.addArguments("--ignore-certificate-errors");
		options.addArguments("start-maximized");
		options.setCapability("acceptSslCerts", true);
		options.setCapability("unexpectedAlertBehaviour", "accept");
		return options;
	}

	private static ChromeOptions setupWebDriverHeadless() {
		ChromeOptions options = WebDriverFactory.setupWebDriver();
		options.addArguments("headless");
		options.addArguments("disable-gpu");
		options.addArguments("--ignore-certificate-errors");
		options.setCapability("acceptSslCerts", true);
		options.setCapability("unexpectedAlertBehaviour", "accept");
		options.setExperimentalOption("useAutomationExtension", false);
		// options.addArguments("window-size=1200x600");
		return options;
	}

	private static void setupWebDriverProperty() {
		// Turn off the proxy before you do anything, otherwise a lot of tests
		// will error out

		System.setProperty("java.net.useSystemProxies", "false");

		if (System.getProperty("webdriver.chrome.driver") == null) {
			System.setProperty("webdriver.chrome.driver", "C:\\Users\\matthew.dotson1\\Downloads\\chromedriver.exe");
			System.setProperty("webdriver.chrome.logfile", "C:/Users/matthew.dotson1/Documents/seleniumLog.txt");
			System.setProperty("webdriver.chrome.loglevel", "TRACE");
			System.setProperty("webdriver.chrome.verboseLogging", "true");
		}

		if (System.getProperty("webdriver.ie.driver") == null) {
			System.setProperty("webdriver.ie.driver",
					"C:\\Users\\matthew.dotson1\\Downloads\\IEDriverServer_x64_3.14.0\\IEDriverServer.exe");
			System.setProperty("webdriver.ie.driver.loglevel", "TRACE");
			System.setProperty("webdriver.ie.driver.logfile", "C:/Users/matthew.dotson1/Documents/seleniumLog.txt");
		}

	}
}

import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.openqa.selenium.WebDriver;
import Steps.LoginSteps;
import Steps.LogoutSteps;
import common.Constants;
import common.WebDriverFactory;

/*
 * For information about the Screenplay pattern,
 * http://thucydides.info/docs/serenity-staging/#_serenity_and_the_screenplay_pattern
 *
 * and some good practices
 * https://www.blazemeter.com/blog/top-15-ui-test-automation-best-practices-you-should-follow
 */

public abstract class BaseTest {
	@AfterClass
	public static void cleanUpAfterClass() {
	}

	@BeforeClass
	public static void setUpBeforeClass() throws Exception {
	}

	protected WebDriver driver;

	LoginSteps loginSteps = new LoginSteps();
	LogoutSteps logoutSteps = new LogoutSteps();

	@Before
	public void setUp() throws Exception {
		this.driver = WebDriverFactory
				// .getChromeWebDriver(false);
				.getIEWebDriver();
		this.loginSteps.login_as(this.driver, Constants.admin);
	}

	@After
	public void tearDown() throws Exception {
		this.logoutSteps.logout(this.driver);
		this.driver.close();
	}

}

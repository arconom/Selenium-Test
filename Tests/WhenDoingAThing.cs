import java.time.Instant;
import org.junit.Before;
import org.junit.Test;
import Steps.ClassManagementViewSteps;
import common.Constants;

public class WhenDoingAThing extends BaseTest {

	Steps steps = new Steps();

	@Override
	@Before
	public void setUp() throws Exception {
		super.setUp();
		this.steps.navigate(this.driver);
	}

	@Test
	public void shouldSeeThisResponse() {
		this.steps.create_new(this.driver, "test data");
		this.steps.should_show_message(this.driver, Constants.saveSuccessMessage);
	}
}

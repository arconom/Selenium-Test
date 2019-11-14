import org.junit.Assert;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import PageObjects.ClassManagementViewPage;
import common.Constants;

public class Steps {

	Page page = new Page();

	public void create_new(WebDriver driver, String data) {
		this.page.click_create(driver);
		this.fill_form(driver, data);
		this.page.save(driver);
	}

	private void fill_form(WebDriver driver, String data) {
		this.page.setData(driver, data);
	}

	public void navigate(WebDriver driver) {
		this.page.navigate(driver);
	}

	public void save_button_should_be_disabled(WebDriver driver) {
		Assert.assertTrue(!this.page.saveButton.isEnabled());
	}

	public void save_button_should_be_enabled(WebDriver driver) {
		Assert.assertTrue(this.page.saveButton.isEnabled());
	}
}

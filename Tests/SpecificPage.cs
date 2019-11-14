import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import common.Constants;

public class SpecificPage extends Page {

	public void click_create(WebDriver driver) {
		this.waitFor(driver, "#btn_create");
		this.clickElement(driver, "#btn_create");
	}

	public void navigate(WebDriver driver) {
		this.selectView(driver, "Class Management");
	}

	// for long wait times
	public void waitUntilLoaded(WebDriver driver) {
		(new WebDriverWait(driver, 10)).until(ExpectedConditions.presenceOfElementLocated(By.id("btn_save")));
	}

}

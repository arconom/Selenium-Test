import java.util.List;
import org.junit.Assert;
import org.openqa.selenium.Alert;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;
import common.Constants;

public abstract class Page {

	public boolean breadcrumbContains(WebDriver driver, String text) {
		List<WebElement> spans = driver.findElement(By.id(Constants.breadCrumbId)).findElements(By.tagName("span"));

		for (int i = 0; i < spans.size(); i++) {
			if (spans.get(i).getText().contains(text)) {
				return true;
			}
		}
		return false;
	}

	public void checkAlert(WebDriver driver) throws Exception {
		try {
			WebDriverWait wait = new WebDriverWait(driver, 2);
			wait.until(ExpectedConditions.alertIsPresent());
			Alert alert = driver.switchTo().alert();
			alert.accept();
		} catch (Exception e) {
			// exception handling
			throw (new Exception("alert failed"));
		}
	}


	protected void clickElement(WebDriver driver, String selector) {
		new WebDriverWait(driver, 2).until(ExpectedConditions.elementToBeClickable(By.cssSelector(selector)));
		driver.findElement(By.cssSelector(selector)).click();
	}

	protected void clickFrameElement(WebDriver driver, String frameId, String selector) {
		new WebDriverWait(driver, 2).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		this.clickElement(driver, selector);
		driver.switchTo().defaultContent();
	}



	protected WebElement getRowContainingText(WebDriver driver, String tableSelector, String text) {

		new WebDriverWait(driver, 2).until(ExpectedConditions.presenceOfElementLocated(By.cssSelector(tableSelector)));

		List<WebElement> rows = driver.findElements(By.cssSelector(tableSelector + " tr"));

		for (int i = 0; i < rows.size(); i++) {
			if (rows.get(i).getText().toLowerCase().contains(text.toLowerCase())) {
				return rows.get(i);
			}
		}

		return null;
	}

	protected String getSelectText(WebDriver driver, String selector) {
		return new Select(driver.findElement(By.cssSelector(selector))).getFirstSelectedOption().getText();
	}

	protected String getText(WebDriver driver, String selector) {
		return driver.findElement(By.cssSelector(selector)).getText();
	}

	protected String getTextFromFrameElement(WebDriver driver, String frameId, String selector) {
		new WebDriverWait(driver, 5).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		String returnMe = driver.findElement(By.cssSelector(selector)).getText();
		driver.switchTo().defaultContent();
		return returnMe;
	}


	protected String getValueFromFrameElement(WebDriver driver, String frameId, String selector) {
		driver.switchTo().frame(frameId);
		String returnMe = driver.findElement(By.cssSelector(selector)).getAttribute("value");
		driver.switchTo().defaultContent();
		return returnMe;
	}



	protected void setBooleanSelect(WebDriver driver, String selector, boolean value) {
		new Select(driver.findElement(By.cssSelector(selector))).selectByVisibleText(value ? "Yes" : "No");
	}

	protected void setCheckbox(WebDriver driver, String selector, boolean value) {

		WebElement chk = driver.findElement(By.cssSelector(selector));

		if ((value && !chk.isSelected()) || (!value && chk.isSelected())) {
			chk.click();
		}
	}

	protected void setCheckboxInFrameElement(WebDriver driver, String frameId, String selector, boolean value) {
		new WebDriverWait(driver, 2).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		this.setCheckbox(driver, selector, value);
		driver.switchTo().defaultContent();
	}

	protected void setSelect(WebDriver driver, String selector, int index) {
		new Select(driver.findElement(By.cssSelector(selector))).selectByIndex(index);
	}

	protected void setSelect(WebDriver driver, String selector, String value) {
		new Select(driver.findElement(By.cssSelector(selector))).selectByVisibleText(value);
	}

	protected void setSelectInFrameElement(WebDriver driver, String frameId, String selector, String value) {
		new WebDriverWait(driver, 2).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		this.setSelect(driver, selector, value);
		driver.switchTo().defaultContent();
	}


	protected void setText(WebDriver driver, String selector, String value) {
		WebElement e = driver.findElement(By.cssSelector(selector));
		e.clear();
		e.sendKeys(value);
	}

	protected void setTextInFrameElement(WebDriver driver, String frameId, String selector, String text) {
		new WebDriverWait(driver, 5).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		this.setText(driver, selector, text);
		driver.switchTo().defaultContent();
	}




	public WebElement waitFor(WebDriver driver, String selector) {
		return (new WebDriverWait(driver, 10))
				.until(ExpectedConditions.presenceOfElementLocated(By.cssSelector(selector)));
	}

	public void waitForFrameToHide(WebDriver driver, String selector) {
		(new WebDriverWait(driver, 10))
				.until(ExpectedConditions.invisibilityOfElementLocated(By.cssSelector(selector)));
	}

	protected void waitUntilFrameLoaded(WebDriver driver, String frameId, String selector) {
		new WebDriverWait(driver, 2).until(ExpectedConditions.frameToBeAvailableAndSwitchToIt(By.id(frameId)));
		(new WebDriverWait(driver, 10)).until(ExpectedConditions.presenceOfElementLocated(By.cssSelector(selector)));
		driver.switchTo().defaultContent();
	}

}


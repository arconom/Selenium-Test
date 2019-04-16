package PageObjects;

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

	public void clickBreadCrumb(WebDriver driver, String text) {
		driver.findElement(By.id(Constants.breadCrumbId)).findElement(By.linkText(text)).click();
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

	protected void clickPopupElement(WebDriver driver, String selector) {
		this.clickFrameElement(driver, Constants.popupId, selector);
	}

	protected void clickTabContentElement(WebDriver driver, String selector) {
		this.clickFrameElement(driver, Constants.tabFrameId, selector);
	}

	protected String getMessage(WebDriver driver, String frameContext) {
		String selector = "#messages";

		if (frameContext.equals("")) {
			return this.getText(driver, selector);
		} else {
			return this.getTextFromFrameElement(driver, frameContext, selector);
		}
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

	protected String getTextFromPopupElement(WebDriver driver, String selector) {
		return this.getTextFromFrameElement(driver, Constants.popupId, selector);
	}

	protected String getTextFromTabContentElement(WebDriver driver, String selector) {
		return this.getTextFromFrameElement(driver, Constants.tabFrameId, selector);
	}

	protected String getValueFromFrameElement(WebDriver driver, String frameId, String selector) {
		driver.switchTo().frame(frameId);
		String returnMe = driver.findElement(By.cssSelector(selector)).getAttribute("value");
		driver.switchTo().defaultContent();
		return returnMe;
	}

	public void gotoTab(WebDriver driver, int index) {
		driver.findElements(By.cssSelector(Constants.detailTabListSelector + " li")).get(index).click();
	}

	public void gotoTab(WebDriver driver, String text) {
		List<WebElement> tabs = driver.findElements(By.cssSelector(Constants.detailTabListSelector + " li"));

		for (int i = 0; i < tabs.size(); i++) {
			if (tabs.get(i).getText().equals(text)) {
				tabs.get(i).click();
				break;
			}
		}
	}

	public void popupSave(WebDriver driver) {
		this.clickPopupElement(driver, "#btn_save");
	}

	public void selectDetailTab(WebDriver driver, String tab) {
		this.selectTab(driver, ".detail_tab", tab);
	}

	public void selectHorizontalMenuTab(WebDriver driver, String tab) {
		this.selectTab(driver, ".horizontal_menu_tabs", tab);
	}

	public void selectTab(WebDriver driver, String tab) {
		this.selectTab(driver, ".menu_tabs", tab);
	}

	private void selectTab(WebDriver driver, String tabContainerSelector, String tab) {
		WebElement tabs = (new WebDriverWait(driver, 10))
				.until(ExpectedConditions.presenceOfElementLocated(By.cssSelector(tabContainerSelector)));
		tabs.findElement(By.xpath("//span[.='" + tab + "']")).findElement(By.xpath("..")).click();
	}

	public void selectView(WebDriver driver, String view) {
		driver.switchTo().defaultContent();
		
		new Select((new WebDriverWait(driver, 10)).until(ExpectedConditions.presenceOfElementLocated(By.id("viewId"))))
				.selectByVisibleText(view);
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

	protected void setSelectInPopupElement(WebDriver driver, String selector, String text) {
		this.setSelectInFrameElement(driver, Constants.popupId, selector, text);
	}

	protected void setSelectInTabContentElement(WebDriver driver, String selector, String text) {
		this.setSelectInFrameElement(driver, Constants.tabFrameId, selector, text);
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

	protected void setTextInPopupElement(WebDriver driver, String selector, String text) {
		this.setTextInFrameElement(driver, Constants.popupId, selector, text);
	}

	protected void setTextInTabContentElement(WebDriver driver, String selector, String text) {
		this.setTextInFrameElement(driver, Constants.tabFrameId, selector, text);
	}

	public void shouldShowMessage(WebDriver driver, String message, String frameContext) {
		Assert.assertEquals(true, this.getMessage(driver, frameContext).contains(message));
	}

	public void tabFrameRemove(WebDriver driver) {
		this.clickFrameElement(driver, Constants.tabFrameId, "#btn_remove");
	}

	public void tabFrameSave(WebDriver driver) {
		this.clickFrameElement(driver, Constants.tabFrameId, "#btn_save");
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

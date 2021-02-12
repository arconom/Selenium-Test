using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
namespace Selenium.Pages
{
    /// <summary>
    /// Represents a page viewed by Selenium.  Contains all functions required to get find and manipulate DOM elements.
    /// </summary>
    public abstract class Page
    {
        /// <summary>
       /// Initializes a new instance of the <see cref="Page" /> class.
        /// </summary>
        /// <param name="driver">The Selenium web driver viewing the page.</param>
        /// <param name="url">The URL of the page.</param>
        public Page(IWebDriver driver, string url)
        {
            this.Driver = driver;
            this.Url = url;
        }
        /// <summary>
        /// Represents the locator method used to find DOM elements.
        /// </summary>
        public enum LocatorType
        {
            CssSelector,
            XPath
        }
        private IWebDriver driver;
        private string url;
        /// <summary>
        /// The selenium web driver viewing the page.
        /// </summary>
        protected IWebDriver Driver { get => driver; set => driver = value; }
        /// <summary>
        /// The URL of the page.
        /// </summary>
        public string Url { get => url; set => url = value; }
        /// <summary>
        /// Navigates the Selenium web driver to the page and agrees to a DoD warnings if necessary.
        /// </summary>
        public virtual void Navigate()
        {
            Driver.Navigate().GoToUrl(GetBaseURL() + Url);
            string popupSelector = "#frmAgree button";
            try
            {
                WaitFor(popupSelector);
                ClickElement(popupSelector);
            }
            catch (Exception)
            {
                //probably don't want to throw anything
            }
        }
        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <returns>The base URL.</returns>
        public string GetBaseURL()
        {
            return Constants.getBaseURI();
            // just get the "base" bit of the URL
            //return new Uri(Driver.Url).Authority;
        }
        /// <summary>
        /// Accepts an alert.
        /// </summary>
        /// <exception cref="Exception">Thrown if there is a problem accepting an alert.</exception>
        protected void CheckAlert()
        {
            try
            {
                IAlert alert;
                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(2));
                try
                {
                    alert = wait.Until(AlertIsPresent());
                }
                catch (WebDriverTimeoutException)
                {
                    alert = null;
                }
                alert.Accept();
            }
            catch (Exception)
            {
                // exception handling
                throw (new Exception("alert failed"));
            }
        }
        /// <summary>
        /// Finds the amount of elements that match the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The amount of elements matching the locator.</returns>
        protected int ElementCount(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return driver.FindElements(locatorBy).Count;
        }
        /// <summary>
        /// Checks if an element that matches the provided locator exists.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>Returns true if the element exists, false otherwise.</returns>
        protected Boolean ElementExists(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return driver.FindElements(locatorBy).Count != 0;
        }
        /// <summary>
        /// Gets the first element to match the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first element matching the locator.</returns>
        protected IWebElement GetElement(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            return WaitFor(locator, locatorType: locatorType);
        }
        /// <summary>
        /// Gets the first child element to match the provided locator in a specified element.
        /// </summary>
        /// <param name="scope">The element to search for the child element.</param>
        /// <param name="locator">The locator of the child element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first child element matching the locator.</returns>
        protected IWebElement GetElement(IWebElement scope, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return scope.FindElement(locatorBy);
        }
        /// <summary>
        /// Gets the all child elements to match the provided locator in a specified element.
        /// </summary>
        /// <param name="scope">The element to search for the child elements.</param>
        /// <param name="locator">The locator of the child elements.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>A collection of all child elements matching the locator.</returns>
        protected ICollection<IWebElement> GetElements(IWebElement scope, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return scope.FindElements(locatorBy);
        }
        /// <summary>
       /// Clicks the first clickable element (i.e. button, link) matching the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void ClickElement(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            WaitForInteractable(locator, locatorType: locatorType).Click();
        }
        /// <summary>
        /// Hovers the mouse over the first element matching the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void MouseOverElement(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            Actions action = new Actions(Driver);
            action.MoveToElement(WaitFor(locator, locatorType: locatorType)).Perform();
        }
        /// <summary>
        /// Click the first element matching the provided locator in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void ClickFrameElement(String frameId, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            this.ClickElement(locator, locatorType);
            Driver.SwitchTo().DefaultContent();
        }
        /// <summary>
        /// Gets the first row containing the provided text in the first table matching the provided locator.
        /// </summary>
        /// <param name="tableLocator">The locator of the table.</param>
        /// <param name="text">The text to search for.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first row containing the provided text in the first table matching the provided locator.</returns>
        protected IWebElement GetRowContainingText(String tableLocator, String text, LocatorType locatorType = LocatorType.CssSelector)
        {
            WaitFor(tableLocator, locatorType: locatorType);
            By locatorBy = getLocatorBy(tableLocator, locatorType);
            IList<IWebElement> rows = Driver.FindElements(locatorBy);
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Text.ToLower().Contains(text.ToLower()))
                {
                    return rows[i];
                }
            }
            return null;
        }
        /// <summary>
        /// Gets the text of the text of the selected child option element of the first select element mathing the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The text of the selected child option element of the first select element mathing the provided locator</returns>
        protected String GetSelectText(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            return new SelectElement(WaitFor(locator, locatorType: locatorType))
                .AllSelectedOptions[0].Text;
        }
        /// <summary>
        /// Get the value or, if there is no value attribute, the text contained in the first element matching the provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The value or, if there is no value attribute, the text contained in the first element  matching the provided locator.</returns>
        protected String GetText(String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            IWebElement element = WaitFor(locator, locatorType: locatorType);
            String value = element.GetAttribute("value");
            return value != null ? value : element.Text;
        }
        /// <summary>
        /// Get the  text contained in the first element matching the provided locator in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The  text contained in the first element matching the provided locator in the frame located by the provided frame id.</returns>
        protected String GetTextFromFrameElement(String frameId, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            By locatorBy = getLocatorBy(locator, locatorType);
            String returnMe = Driver.FindElement(locatorBy).Text;
            Driver.SwitchTo().DefaultContent();
            return returnMe;
        }
        /// <summary>
        /// Get the value of the first element matching the provided locator in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The value of the first element matching the provided locator in the frame located by the provided frame id.</returns>
        protected String GetValueFromFrameElement(String frameId, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            By locatorBy = getLocatorBy(locator, locatorType);
            String returnMe = Driver.FindElement(locatorBy).GetAttribute("value");
            Driver.SwitchTo().DefaultContent();
            return returnMe;
        }
        /// <summary>
        /// Sets the value of the first select element matching to provided locator to "Yes" if value is true or "No" if value is false.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="value">Select element set to "Yes" if value is true or "No" if value is false.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetBooleanSelect(String locator, Boolean value, LocatorType locatorType = LocatorType.CssSelector)
        {
            new SelectElement(WaitFor(locator, locatorType: locatorType)).SelectByText(value ? "Yes" : "No");
        }
        /// <summary>
        /// Sets the check of the first checkbox input element matching to provided locator to checked if value is true or unchecked if value is false.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="value">Checkbox input element set to checked if value is true or unchecked if value is false.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetCheckbox(String locator, Boolean value, LocatorType locatorType = LocatorType.CssSelector)
        {
            IWebElement chk = WaitFor(locator, locatorType: locatorType);
            if ((value && !chk.Selected) || (!value && chk.Selected))
            {
                chk.Click();
            }
        }
        /// <summary>
        /// Sets the check of the first checkbox input element matching to provided locator to checked if value is true or unchecked if value is false in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="value">Checkbox input element set to checked if value is true or unchecked if value is false.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetCheckboxInFrameElement(String frameId, String locator, Boolean value, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            this.SetCheckbox(locator, value, locatorType);
            Driver.SwitchTo().DefaultContent();
        }
        /// <summary>
        /// Sets the value of the first select element matching to provided locator to the child option element who's index matches the provided index.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="index">The index of the child option element to select.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetSelect(String locator, int index, LocatorType locatorType = LocatorType.CssSelector)
        {
            new SelectElement(GetElement(locator, locatorType)).SelectByIndex(index);
        }
        /// <summary>
        /// Sets the value of the first select element matching to provided locator to the child option element who's text matches value.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="value">The text of the child option element to select.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetSelect(String locator, String value, LocatorType locatorType = LocatorType.CssSelector)
        {
            new SelectElement(GetElement(locator, locatorType)).SelectByText(value);
        }
        /// <summary>
        /// Sets the value of the first select element matching to provided locator to the child option element who's text matches value in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="value">The text of the child option element to select.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetSelectInFrameElement(String frameId, String locator, String value, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            this.SetSelect(locator, value, locatorType);
            Driver.SwitchTo().DefaultContent();
        }
        /// <summary>
        /// Sends the key strokes of text to the first element matching to provided locator.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="text">The keystrokes to send.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetText(String locator, String text, LocatorType locatorType = LocatorType.CssSelector)
        {
            IWebElement e = WaitFor(locator, locatorType: locatorType);
            e.Clear();
            e.SendKeys(text);
        }
        /// <summary>
        /// Sends the key strokes of text to the first element matching to provided locator in the frame located by the provided frame id.
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="text">The keystrokes to send.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void SetTextInFrameElement(String frameId, String locator, String text, LocatorType locatorType = LocatorType.CssSelector)
        {
            new WebDriverWait(Driver, TimeSpan.FromSeconds(5))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            this.SetText(locator, text, locatorType);
            Driver.SwitchTo().DefaultContent();
        }
        /// <summary>
        /// Waits for and returns the first element matching the provided locator or times out if no element is found in the specified time.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="seconds">The amount of time to wait before timeing out.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first element matching the provided locator.</returns>
        protected IWebElement WaitFor(String locator, int seconds = 10, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locatorBy));
        }
        /// <summary>
        /// Waits for the first element matching the provided locator to become visible or times out if no element is found or the element does not become visible in the specified time.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="seconds">The amount of time to wait before timeing out.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first element matching the provided locator.</returns>
        protected IWebElement WaitForVisible(String locator, int seconds = 10, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locatorBy));
        }
        /// <summary>
        /// Waits for the first element matching the provided locator to become interactable or times out if no element is found or the element does not become interacable in the specified time.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="seconds">The amount of time to wait before timeing out.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The first element matching the provided locator.</returns>
        protected IWebElement WaitForInteractable(String locator, int seconds = 10, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locatorBy));
        }
        /// <summary>
        /// Waits for the first element matching the provided locator to become invisible or no longer exist times out if the element does not become invisible and still exists in the specified time.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="seconds">The amount of time to wait before timeing out.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void WaitForInvisible(String locator, int seconds = 10, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            new WebDriverWait(driver, TimeSpan.FromSeconds(seconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locatorBy));
        }
        /// <summary>
        /// Waits for the first element matching the provided locator to no longer be displayed or times out if the element is still displayed after the specified time.
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="seconds">The amount of time to wait before timeing out.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void WaitForFrameToHide(String locator, int seconds = 10, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            (new WebDriverWait(Driver, TimeSpan.FromSeconds(10)))
                    .Until(drv => !drv.FindElement(locatorBy).Displayed);
        }
        /// <summary>
        /// Wait for a frame to load
        /// </summary>
        /// <param name="frameId">The id of the frame containing the element.</param>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        protected void WaitUntilFrameLoaded(String frameId, String locator, LocatorType locatorType = LocatorType.CssSelector)
        {
            By locatorBy = getLocatorBy(locator, locatorType);
            new WebDriverWait(Driver, TimeSpan.FromSeconds(2))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(By.Id(frameId)));
            (new WebDriverWait(Driver, TimeSpan.FromSeconds(10)))
                .Until(WaitUntilFrameLoadedAndSwitchToIt(locatorBy));
            Driver.SwitchTo().DefaultContent();
        }
        /// <summary>
        /// Wait for a frame with the specified id to load and switch the Selenium driver's context to it
        /// </summary>
        /// <param name="byToFindFrame">The id of the frame</param>
        /// <returns></returns>
        private Func<IWebDriver, bool> WaitUntilFrameLoadedAndSwitchToIt(By byToFindFrame)
        {
            return (driver) =>
            {
                try
                {
                    driver.SwitchTo().Frame(driver.FindElement(byToFindFrame));
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            };
        }
        /// <summary>
        /// An expectation for checking whether an element is visible and clickable.
        /// </summary>
        /// <param name="locator">The locator used to find the element.</param>
        /// <returns>The <see cref="IWebElement"/> once it is located, visible and clickable.</returns>
        protected Func<IWebDriver, IWebElement> ElementIsClickable(By locator)
        {
            return driver =>
            {
                var element = driver.FindElement(locator);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }
        /// <summary>
        /// Gets a function that switches to and returns an <see cref="IAlert"/> is there is an alert, or null otherwise
        /// </summary>
        /// <returns>A function that switches to and returns an <see cref="IAlert"/> is there is an alert, or null otherwise</returns>
        protected Func<IWebDriver, IAlert> AlertIsPresent()
        {
            return (driver) =>
            {
                try
                {
                    return driver.SwitchTo().Alert();
                }
                catch (NoAlertPresentException)
                {
                    return null;
                }
            };
        }
        /// <summary>
        /// Get the By for the locator of the specified type
        /// </summary>
        /// <param name="locator">The locator of the element.</param>
        /// <param name="locatorType">The type of the locator.</param>
        /// <returns>The By for the locator of the specified type</returns>
        private static By getLocatorBy(string locator, LocatorType locatorType)
        {
            By locatorBy;
            switch (locatorType)
            {
                case LocatorType.XPath:
                    locatorBy = By.XPath(locator);
                    break;
                case LocatorType.CssSelector:
                default:
                    locatorBy = By.CssSelector(locator);
                    break;
            }
            return locatorBy;
        }
    }
}

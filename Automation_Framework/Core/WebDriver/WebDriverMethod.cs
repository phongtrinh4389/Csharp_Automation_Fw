using NLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Threading;
using Automation_Framework.Core.Common;
using System.Drawing;
using System.IO;

namespace Automation_Framework.Core.WebDriver
{
    public class WebDriverMethod : WebDriverFactory
    {

        public WebDriverMethod()
        {
        }

        public int GetWaitTimeoutSeconds()
        {
            return int.Parse(ConfigurationManager.AppSettings["WEBDRIVERTIMEOUT"]);
        }

        public void OpenUrl(string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);

                string info = string.Format("Go to url [{0}]", url);
                HtmlReporter.Pass(info);
                NLogger.Info(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("Can't navigate to the url [{0}]", url);

                HtmlReporter.Fail(message, ex);
                throw new ErrorHandler(message, ex);
            }
        }


        public void WaitForPageTitleEqual(string title)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                wait.Until(ExpectedConditions.TitleIs(title));

                string info = string.Format("Wait for page title equal [{0}]", title);
                HtmlReporter.Pass(info);
                NLogger.Info(info);
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Page title does not appear as expected. Title information: {0}", title);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool IsPageTitleEqual(string title)
        {
            try
            {
                WaitForPageTitleEqual(title);

                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public IWebElement WaitForElementToBeVisible(string elementName, By byObject)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(byObject));

                string info = string.Format("Wait for element [{0}] to be visible", elementName);
                HtmlReporter.Pass(info);
                NLogger.Info(info);

                return element;
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Element [{0}] is not visible as expected.", elementName);

                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool IsElementVisible(string elementName, By byObject)
        {
            try
            {
                WaitForElementToBeVisible(elementName, byObject);
                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public IWebElement WaitForElementToBeExist(string elementName, By byObject)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                IWebElement element = wait.Until(ExpectedConditions.ElementExists(byObject));

                string info = string.Format("Wait for element [{0}] to be exist", elementName);
                HtmlReporter.Pass(info);
                NLogger.Info(info);

                return element;


            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Element [{0}] is not exist as expected.", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public ReadOnlyCollection<IWebElement> WaitForVisibilityOfAllElementsLocatedBy(string elementName, By byObject)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                ReadOnlyCollection<IWebElement> elements = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(byObject));

                string info = string.Format("Wait for elements [{0}] to be visible", elementName);
                HtmlReporter.Pass(info);
                NLogger.Info(info);

                return elements;

            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Elements [{0}] are not visible as expected.", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public List<string> WaitForVisibilityOfAllElementTextsLocatedBy(string elementName, By byObject, List<string> itemList)
        {
            try
            {

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                List<string> list = wait.Until(CustomExpectedConditions.VisibilityOfAllElementTextsLocatedBy(byObject, itemList));

                string info = string.Format("Wait for text list to be present in element. String list: [{0}]; Element information: {1}",
                    string.Join(", ", itemList.ToArray()), elementName);
                HtmlReporter.Pass(info);
                NLogger.Info(info);

                return list;
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Text list is not present in element as expected. String list: [{0}]; Element information: {1}",
                    string.Join(", ", itemList.ToArray()), elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool AreAllElementTextsLocatedBy(string elementName, By byObject, List<string> itemList)
        {
            try
            {
                WaitForVisibilityOfAllElementTextsLocatedBy(elementName, byObject, itemList);
                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public bool WaitForTextToBePresentInElementLocated(string elementName, By byObject, string text)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                bool status = wait.Until(ExpectedConditions.TextToBePresentInElementLocated(byObject, text));

                string info = string.Format("Wait for text [{0}] to be present in element [{1}].", text, elementName);
                HtmlReporter.Pass(info);
                NLogger.Info(info);

                return status;
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Text [{0}] is not present in element [{1}] as expected.", text, elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool IsTextPresentInElementLocated(string elementName, By byObject, string text)
        {
            try
            {
                return WaitForTextToBePresentInElementLocated(elementName, byObject, text);
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public void WaitForElementToBeInvisible(string elementName, By byObject)
        {
            try
            {
                NLogger.Info(string.Format("Wait for element [{0}] to be invisible.", elementName));
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(byObject));

                string info = string.Format("Wait for element [{0}] to be invisible.", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Element [{0}] is still visible.", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool IsElementInvisible(string elementName, By byObject)
        {
            try
            {
                WaitForElementToBeInvisible(elementName, byObject);
                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public IWebElement WaitForElementToBeClickable(string elementName, By byObject)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                IWebElement element = wait.Until(ExpectedConditions.ElementToBeClickable(byObject));

                string info = string.Format("Wait for element [{0}] to be clickable.", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return element;
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("Element [{0}] is not clickable as expected.", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool IsElementClickable(string elementName, By byObject)
        {
            try
            {
                WaitForElementToBeClickable(elementName, byObject);
                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public string WaitForVisibilityOfURL(string url)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                string actual_url = wait.Until(CustomExpectedConditions.urlContainedBy(url));

                string info = string.Format("Wait for URL [{0}] displays", url);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return actual_url;
            }
            catch (WebDriverTimeoutException ex)
            {
                var message = string.Format("The URL [{0}] is not presented as expected", url);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public bool isURLContainedBy(string elementName, string url)
        {
            try
            {
                WaitForVisibilityOfURL(url);
                return true;
            }
            catch (ErrorHandler)
            {
                return false;
            }
        }

        public void Clear(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                element.Clear();

                string info = string.Format("Clear element content [{0}]", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to clear element content [{0}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }
        public void EnterText(string elementName, By byObject, string text)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                element.Clear();
                element.SendKeys(text);

                string info = string.Format("Enter text [{0}] to element [{1}].", text, elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to send text [{0}] to element [{1}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void UploadFile(string elementName, By byObject, string filename)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                element.SendKeys(filename);

                string info = string.Format("Uploading file. File name: [{0}]; Element Name [{1}]", filename, elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to send text [{0}] to element [{1}].", filename, elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }
        public void FillTextAndEnter(string elementName, By byObject, string text)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                element.Clear();
                element.SendKeys(text);
                element.SendKeys(Keys.Enter);

                string info = string.Format("Enter text to element. Text: [{0}]; Element Name [{1}]", text, elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to send text [{0}] to element [{1}] then Enter.", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }
        public void VerifyText(string elementName, By byObject, string expected)
        {
            string actual = null;
            try
            {

                actual = GetText(elementName, byObject);
                Assert.AreEqual(expected, actual);

                string info = string.Format("Text of element [{0}] as expectation [{1}]", elementName, expected);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (AssertFailedException ex)
            {
                IWebElement element = WaitForElementToBeVisible(elementName, byObject);
                string message = string.Format("Verify text for element [{0}] failed. Expected [{1}] but actual [{2}]", elementName, expected, actual);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);

            }
        }

        public void VerifyValue(string elementName, By byObject, string expected)
        {
            string actual = null;
            try
            {

                actual = GetValue(elementName, byObject);
                Assert.AreEqual(expected, actual);

                string info = string.Format("Value of element [{0}] as expectation [{1}]", elementName, expected);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (AssertFailedException ex)
            {
                IWebElement element = WaitForElementToBeVisible(elementName, byObject);
                string message = string.Format("Verify value for element [{0}] failed. Expected [{1}] but actual [{2}]", elementName, expected, actual);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);

            }
        }

        public void VerifyInteger(string elementName, By byObject, int expectedInt)
        {
            int actualInteger = 0;
            try
            {

                actualInteger = int.Parse(GetText(elementName, byObject));
                Assert.AreEqual(expectedInt, actualInteger);

                string info = string.Format("Interger of element [{0}] as expectation [{1}]", elementName, expectedInt);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (AssertFailedException ex)
            {
                IWebElement element = WaitForElementToBeVisible(elementName, byObject);
                string message = string.Format("Verify Interger for element [{0}] failed. Expected [{1}] but actual [{2}]", elementName, expectedInt, actualInteger);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);

            }
        }


        public void VerifyDouble(string elementName, By byObject, double expectedDouble)
        {
            double actualDouble = 0;
            try
            {

                actualDouble = double.Parse(GetText(elementName, byObject));
                Assert.AreEqual(expectedDouble, actualDouble);

                string info = string.Format("Double of element [{0}] as expectation [{1}]", elementName, expectedDouble);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (AssertFailedException ex)
            {
                IWebElement element = WaitForElementToBeVisible(elementName, byObject);
                string message = string.Format("Verify Double for element [{0}] failed. Expected [{1}] but actual [{2}]", elementName, expectedDouble, actualDouble);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);

            }
        }

        public void Click(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                WaitForFullPageLoadedA();
                element = WaitForElementToBeClickable(elementName, byObject);
                element.Click();

                string info = string.Format("Click on element. Element information: {0}", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to click on element [{0}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }
        public void ClickButtonAndWaitToLoad(string elementName, By byObject, By obicon)
        {

            try
            {
                IWebElement element = WaitForElementToBeClickable(elementName, byObject);
                element.Click();
                WaitForElementToBeInvisible("", obicon);
                WaitForFullPageLoadedA();

                string info = string.Format("Click on element [{0}] and wait for loading", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to click on [{0}] and wait for loading", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void ClickAndWait(string elementName, By byObject)
        {

            try
            {
                Click(elementName, byObject);
                WaitForFullPageLoadedA();

                string info = string.Format("Click on element [{0}] and wait for loading", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to click on [{0}] and wait for loading", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void SelectComboItemByText(string elementName, By byObject, string itemText)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                SelectElement select = new SelectElement(element);
                select.SelectByText(itemText);

                string info = string.Format("Select [{0}] from element [{1}].", elementName, itemText);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to SelectComboItemByText [{0}] from element [{1}].", elementName, itemText);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void SelectValueInDropBoxByElement(string elementName, By byObject, By item)
        {
            try
            {
                IWebElement comboBox = WaitForElementToBeClickable(elementName, byObject);
                comboBox.Click();

                IWebElement selectedItem = WaitForElementToBeClickable("", item);
                selectedItem.Click();

                string info = string.Format("SelectValueInDropBoxByElement from element [{0}].", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to SelectValueInDropBoxByElement from element [{0}].", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void SwitchToFrame(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                driver.SwitchTo().Frame(element);

                string info = string.Format(string.Format("Switch to frame. Frame information: {0}", elementName));
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when switching to frame [{0}]", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void SwitchToDefaultContent(string elementName)
        {
            driver.SwitchTo().DefaultContent();
        }

        public string GetText(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);

                string info = string.Format(string.Format("Get element text. Element information: {0}", elementName));
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return element.Text;
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to get the text of element [{0}]", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void HoverMouseOn(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeClickable(elementName, byObject);
                Actions action = new Actions(driver);
                action.MoveToElement(element).Perform();

                string info = string.Format(string.Format("Hover mouse on element. Element information: {0}", elementName));
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to hover mouse on element [{0}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void SwitchToNewWindow()
        {
            try
            {
                string currentWindowHandle = driver.CurrentWindowHandle;
                var windowHandles = driver.WindowHandles;
                foreach (string windowHandle in windowHandles)
                {
                    if (windowHandle != currentWindowHandle)
                    {
                        driver.Close();
                        driver.SwitchTo().Window(windowHandle);
                    }
                }

                string info = string.Format(string.Format("Switch to new window"));
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to switch to new window");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public List<string> GetTextFromElements(string elementName, By byObject)
        {
            ReadOnlyCollection<IWebElement> listElements = null;
            try
            {
                List<string> listElementText = new List<string>();
                listElements = WaitForVisibilityOfAllElementsLocatedBy(elementName, byObject);
                foreach (IWebElement element in listElements)
                {
                    string text = element.Text;
                    listElementText.Add(text);
                }

                string info = string.Format("Get text from list of elemets [{0}]", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return listElementText;
            }
            catch (WebDriverException ex)
            {
                foreach (IWebElement element in listElements)
                {
                    ScreenShotCapturing.HighlightElement(driver, element);
                }
                string message = string.Format("An error happens when trying to get text for list of elements [{0}].", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public string GetHref(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);

                string info = string.Format("Get the href of elemet [{0}]", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return element.GetAttribute("href");
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to get the href of element [{0}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public string GetValue(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);

                string info = string.Format("Get the value of elemet [{0}]", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return element.GetAttribute("value");
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to get the value of element [{0}].", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void HoverMouseAndClick(string elementName, By byObject)
        {
            IWebElement element = null;
            try
            {
                element = WaitForElementToBeVisible(elementName, byObject);
                HoverMouseOn(elementName, byObject);
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                executor.ExecuteScript("arguments[0].click();", element);

                string info = string.Format("Hover mouse and click on element [{0}].", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverException ex)
            {
                string message = string.Format("An error happens when trying to hover mouse and click element on [{0}]", elementName);
                ScreenShotCapturing.HighlightElement(driver, element);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public ReadOnlyCollection<IWebElement> GetListOfWebElements(string elementName, By byObject)
        {
            ReadOnlyCollection<IWebElement> listElements = null;
            try
            {

                listElements = WaitForVisibilityOfAllElementsLocatedBy(elementName, byObject);

                string info = string.Format("Get list of elements [{0}].", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

                return listElements;
            }
            catch (WebDriverException ex)
            {
                foreach (IWebElement element in listElements)
                {
                    ScreenShotCapturing.HighlightElement(driver, element);
                }
                string message = string.Format("An error happens when trying to get list of elements [{0}].", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }

        }

        public void SelectByValue(string elementName, By byObject, string value)
        {
            ReadOnlyCollection<IWebElement> listElements = null;
            try
            {

                listElements = WaitForVisibilityOfAllElementsLocatedBy(elementName, byObject);

                foreach (IWebElement e in listElements)
                {
                    string text = e.GetAttribute("value");
                    if (text.Equals(value))
                    {
                        e.Click();
                        break;
                    }
                }

                string info = string.Format("Get list of elements [{0}].", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (WebDriverException ex)
            {
                foreach (IWebElement element in listElements)
                {
                    ScreenShotCapturing.HighlightElement(driver, element);
                }
                string message = string.Format("An error happens when trying to get list of elements [{0}].", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }

        }

        public void ClickAll(string elementName, By byObject)
        {
            IReadOnlyCollection<IWebElement> listElements = null;

            try
            {

                listElements = WaitForVisibilityOfAllElementsLocatedBy(elementName, byObject);

                foreach (IWebElement e in listElements)
                {
                    e.Click();
                }

                string info = string.Format("Click on all elements [{0}].", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);

            }
            catch (WebDriverException ex)
            {
                foreach (IWebElement element in listElements)
                {
                    ScreenShotCapturing.HighlightElement(driver, element);
                }
                string message = string.Format("An error happens when trying to click all on list of elements [{0}].", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }

        }
        public void WaitForFullPageLoadedA()
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(GetWaitTimeoutSeconds()));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//body[@class='loading']")));

                string info = string.Format("Wait for page to be loaded completedly");
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (WebDriverTimeoutException ex)
            {
                string message = string.Format("Page is still loading");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void WaitForFullPageLoaded()
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                for (int i = 0; i < 60; i++)
                {
                    Thread.Sleep(1000);
                    if (js.ExecuteScript("return document.readyState").ToString().Equals("complete"))
                    {
                        break;
                    }
                }

                string info = string.Format("Wait for page to be loaded completedly");
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("WaitForFullPageLoaded: Error occurs");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }

        }

        public void ScrolltillElement(string elementName, By byObject)
        {

            try
            {
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver;

                IWebElement element = WaitForElementToBeVisible(elementName, byObject);
                je.ExecuteScript("arguments[0].scrollIntoView(true);", element);

                string info = string.Format("Scroll page to element [{0}]", elementName);
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("Can't scroll page to element [{0}]", elementName);
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void ScrollDownPage(int pixels)
        {

            try
            {
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                je.ExecuteScript("window.scrollBy(0," + pixels + ")", "");

                string info = string.Format("Scroll page down");
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("Can't scroll page down.");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }

        }

        public void ScrollUpPage(string elementName, int pixels)
        {
            try
            {
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                je.ExecuteScript("window.scrollBy(0,-" + pixels + ")", "");

                string info = string.Format("Scroll page up");
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("Can't scroll page up.");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public void ScrollPage()
        {

            try
            {
                IJavaScriptExecutor je = (IJavaScriptExecutor)driver;
                je.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)", "");

                string info = string.Format("ScrollPage");
                NLogger.Info(info);
                HtmlReporter.Pass(info);
            }
            catch (Exception ex)
            {
                string message = string.Format("Can't ScrollPage.");
                string screenshot = ScreenShotCapturing.TakeScreenShoot(driver, message);
                HtmlReporter.Fail(message, ex, screenshot);
                throw new ErrorHandler(message, ex);
            }
        }

        public string CaptureScreenshot()
        {
            try
            {
                var screenshotDirectory = Utils.ScreenshotPath;
                var fileName = string.Format(@"Screenshot_{0}.png", DateTime.Now.ToString(Constant.TIME_STAMP_2));
                System.IO.Directory.CreateDirectory(screenshotDirectory);
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshot = ss.AsBase64EncodedString;
                string fileLocation = string.Format(@"{0}\{1}", Utils.ScreenshotPath, fileName);
                ss.SaveAsFile(fileLocation, ScreenshotImageFormat.Png);
                NLogger.Debug(string.Format("Screenshot captured. File location: {0}", fileLocation));

                return fileLocation;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string CaptureScreenshot(string fileDir, string filename)
        {
            try
            {
                Utils.CreateDirectory(fileDir);
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                string screenshot = ss.AsBase64EncodedString;
                string fileLocation = fileDir + Utils.separator + filename + ".png";
                ss.SaveAsFile(fileLocation, ScreenshotImageFormat.Png);
                NLogger.Info(string.Format("Screenshot captured. File location: {0}", fileLocation));

                return fileLocation;
            }
            catch (Exception)
            {

                return "";
            }
        }

        public string CaptureScreenshot(string fileDir, string filename, By byObject)
        {
            try
            {
                ScrolltillElement(filename, byObject);
                IWebElement element = WaitForElementToBeVisible(filename, byObject);

                string fileLocation = fileDir + Utils.separator + filename + ".png";
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                var img = Image.FromStream(new MemoryStream(ss.AsByteArray)) as Bitmap;
                var e = img.Clone(new Rectangle(element.Location, element.Size), img.PixelFormat);
                e.Save(fileLocation, System.Drawing.Imaging.ImageFormat.Png);

                NLogger.Info(string.Format("Screenshot captured. File location: {0}", fileLocation));

                return fileLocation;
            }
            catch (Exception)
            {

                return "";
            }
        }

        public void CompareScreenshot(string filename, By byObject = null)
        {
            bool DAILY_REPORT_ENABLED = Convert.ToBoolean(ConfigurationManager.AppSettings["DAILY_REPORT_ENABLED"]);
            bool UI_TEST_ENABLED = Convert.ToBoolean(ConfigurationManager.AppSettings["UI_TEST_ENABLED"]);
            int threshold = Convert.ToInt32(ConfigurationManager.AppSettings["IMAGE_COMPARE_THRESHOLD"]);

            string strScreenshotFileName = filename + ".png";
            string strBaseLineImage = Utils.BaselinePath + Utils.separator + strScreenshotFileName;
            string strActualImage = Utils.ActualScreenshotPath + Utils.separator + strScreenshotFileName;
            string strDiffImage = Utils.DiffScreenshotPath + Utils.separator + strScreenshotFileName;
            try
            {

                if (!(DAILY_REPORT_ENABLED && UI_TEST_ENABLED))
                {
                    throw new Exception("Enable both option DAILY_REPORT_ENABLED and UI_TEST_ENABLED to active the UI testing mode");
                }

                WaitForFullPageLoaded();
                if (!File.Exists(strBaseLineImage))
                {
                    if (byObject == null)
                    {
                        CaptureScreenshot(Utils.BaselinePath, filename);
                    }
                    else
                    {
                        CaptureScreenshot(Utils.BaselinePath, filename, byObject);
                    }

                    NLogger.Info("The baseline screenshot of [" + filename + "] not exist, creating it");
                    HtmlReporter.Pass("The baseline screenshot of [" + filename + "] not exist, creating it");
                }
                else
                {
                    if (byObject == null)
                    {
                        CaptureScreenshot(Utils.ActualScreenshotPath, filename);
                    }
                    else
                    {
                        CaptureScreenshot(Utils.ActualScreenshotPath, filename, byObject);
                    }

                    Bitmap diffImage = ImageCompare.DiffImage(strActualImage, strBaseLineImage, threshold);

                    if (diffImage == null)
                    {
                        NLogger.Info("The actual screenshot of [" + filename + "] matches with the baseline");
                        HtmlReporter.Pass("The actual screenshot of [" + filename + "] matches with the baseline", strActualImage);
                    }
                    else
                    {
                        diffImage.Save(strDiffImage);
                        //NLogger.Error("The actual screenshot of [" + filename + "] not matches with the baseline");
                        HtmlReporter.Fail("The actual screenshot of [" + filename + "] not matches with the baseline", strDiffImage);
                        throw new Exception("The actual screenshot of [" + filename + "] not matches with the baseline");
                    }

                }
            }
            catch (Exception ex)
            {
                string message = string.Format("There is an error when comparing the screenshot of page [{0}]", filename);
                //HtmlReporter.Fail(message, ex);
                throw new ErrorHandler(message, ex);
            }
        }
    }
}

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Framework.Core.WebDriver
{
    public sealed class CustomExpectedConditions
    {
        public static Func<IWebDriver, List<string>> VisibilityOfAllElementTextsLocatedBy(By locator, List<string> expectedItems)
        {
            return (driver) =>
            {
                try
                {
                    List<string> elementTexts = new List<string>(
                    driver.FindElements(locator).Select(iw => iw.Text));
                    bool isSubset = !expectedItems.Except(elementTexts).Any();
                    if (isSubset)
                    {
                        return elementTexts;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }

        public static Func<IWebDriver, string> urlContainedBy(string url)
        {
            return (driver) =>
            {
                try
                {
                    string currentURL = driver.Url.ToString();
                    bool isURLVisible = currentURL.Contains(url);
                    if (isURLVisible)
                    {
                        return currentURL;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            };
        }
    }
}

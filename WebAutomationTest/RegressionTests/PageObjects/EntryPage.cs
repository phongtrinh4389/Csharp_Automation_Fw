using Automation_Framework.Core.WebDriver;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAutomationTest.RegressionTests.Common;

namespace WebAutomationTest.RegressionTests.PageObjects
{
    public class EntryPage
    {
        private WebDriverMethod driver;
        private Dictionary<string, string> data;

        private By btnSignIn = By.Id("btn1");
        private By btnSkipSignIn = By.Id("btn2");
        private By txtEmail = By.Id("email");

        public EntryPage(WebDriverMethod driver, Dictionary<string, string> data)
        {
            this.driver = driver;
            this.data = data;
        }

        public void GoToEntryPage()
        {
            driver.OpenUrl(Constant.URL);
        }

        public void VerifyEntryPageTitle()
        {
            driver.WaitForPageTitleEqual(data["EntryPageTitle"]);
        }

        public LoginPage ClickOnSignIn()
        {
            driver.Click("Sign In button", btnSignIn);
            return new LoginPage(driver, data);
        }
    }
}

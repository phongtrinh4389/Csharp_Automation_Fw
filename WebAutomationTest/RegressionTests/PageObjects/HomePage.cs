using Automation_Framework.Core.WebDriver;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAutomationTest.RegressionTests.PageObjects
{
    public class HomePage
    {
        private WebDriverMethod driver;
        private Dictionary<string, string> data;

        private By txtFirstName = By.XPath("//input[@ng-model='FirstName']");
        private By txtLastName = By.XPath("//input[@ng-model='LastName']");
        private By btnSubmit = By.Id("submitbtn");

        public HomePage(WebDriverMethod driver, Dictionary<string, string> data)
        {
            this.driver = driver;
            this.data = data;
        }

        public void VerifyHomeTitle()
        {
            driver.WaitForPageTitleEqual(data["HomePageTitle"]);
            Console.WriteLine(driver.IsPageTitleEqual(data["HomePageTitle"].ToString()));
        }
    }
}

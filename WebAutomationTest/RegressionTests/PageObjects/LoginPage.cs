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
    public class LoginPage
    {
        private WebDriverMethod driver;
        private Dictionary<string, string> data;

        private By txtEmail = By.XPath("//input[@ng-model='Email']");
        private By txtPassword = By.XPath("//input[@ng-model='Password']");
        private By btnEnter = By.Id("enterbtn");

        public LoginPage(WebDriverMethod driver, Dictionary<string, string> data)
        {
            this.driver = driver;
            this.data = data;
        }

        public void InputUsername()
        {
            Console.WriteLine("Username: " + data["Username"]);
            driver.EnterText("Username", txtEmail, data["Username"]);
        }

        public void InputPassword()
        {
            Console.WriteLine("Password: " + data["Password"]);
            driver.EnterText("Password", txtPassword, data["Password"]);
        }

        public HomePage ClickEnter()
        {
            driver.Click("Enter button", btnEnter);
            return new HomePage(driver, data);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Automation_Framework.Core.Common;
using Automation_Framework.Core.WebDriver;
using System.Collections;
using WebAutomationTest.RegressionTests.PageObjects;

namespace WebAutomationTest.RegressionTests.TestCases
{
    [TestFixture("ie")]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("regression")]
    [Author("phongtrinh")]
    [Description("this is the first class test")]
    public class LoginSuccessTest : WebTestSetup
    {

        public LoginSuccessTest(string browser) : base(browser)
        {

        }
        public static IEnumerable<TestCaseData> TestData1
        {
            get
            {
                string[] sheets = { "LoginSuccessTest" };
                Dictionary<string, string> filter = new Dictionary<string, string>() { { "Username", "username21" } };
                return ExcelReader.GetTestData("Data.xlsx", sheets);
            }
        }


        [Test, TestCaseSource("TestData1")]
        public void LoginSuccess1(Dictionary<string, string> data)
        {
            EntryPage entryPage = new EntryPage(driver, data);
            LoginPage loginPage = new LoginPage(driver, data);
            HomePage homePage = new HomePage(driver, data);


            entryPage.GoToEntryPage();
            HtmlReporter.Info("+++++++++++ On Entry page ++++++++++");
            //entryPage.VerifyEntryPageTitle();
            loginPage = entryPage.ClickOnSignIn();
            HtmlReporter.Info("+++++++++++ On Login page ++++++++++");
            loginPage.InputUsername();
            loginPage.InputPassword();
            homePage = loginPage.ClickEnter();

            //homePage.VerifyHomeTitle();
        }


    }
}

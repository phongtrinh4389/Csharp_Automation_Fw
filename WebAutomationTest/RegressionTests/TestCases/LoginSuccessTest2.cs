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
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    class LoginSuccessTest2 : WebTestSetup
    {
        public static IEnumerable<TestCaseData> TestData2
        {
            get
            {
                string[] sheets = { "LoginSuccessTest2" };
                return ExcelReader.GetTestData("Data.xlsx", sheets);
            }
        }

        [Test, TestCaseSource("TestData2")]
        public void LoginSuccess2(Dictionary<string, string> data)
        {
            EntryPage entryPage = new EntryPage(driver, data);
            LoginPage loginPage = new LoginPage(driver, data);
            HomePage homePage = new HomePage(driver, data);

            entryPage.GoToEntryPage();
            entryPage.VerifyEntryPageTitle();
            loginPage = entryPage.ClickOnSignIn();

            loginPage.InputUsername();
            loginPage.InputPassword();
            homePage = loginPage.ClickEnter();


            //homePage.VerifyHomeTitle();
        }


    }
}

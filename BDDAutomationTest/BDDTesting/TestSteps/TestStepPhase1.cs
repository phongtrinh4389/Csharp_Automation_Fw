using System;
using TechTalk.SpecFlow;
using Automation_Framework.Core.WebDriver;
using BDDAutomationTest.BDDTesting.Common;
using Automation_Framework.Core.Common;
using System.Configuration;

namespace BDDAutomationTest.BDDTesting.TestSteps
{
    [Binding]
    public class TestStepPhase1
    {
        WebDriverMethod driver = (WebDriverMethod)FeatureContext.Current["driver"];
        [Given(@"I am in the Entry page")]
        public void GivenIAmInTheEntryPage()
        {
            driver.OpenUrl(ProjectConstant.URL);
        }

        [Then(@"The entry page title is ""(.*)""")]
        public void ThenTheEntryPageTitleIs(string title)
        {
            driver.IsPageTitleEqual(title);

        }


    }
}

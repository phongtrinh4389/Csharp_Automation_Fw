using System;
using TechTalk.SpecFlow;
using Automation_Framework.Core.Common;

namespace BDDAutomationTest.BDDTesting.TestSteps
{
    [Binding]
    public class TheThirdTestSteps
    {
        private int result;
        private int start;
        private int eat;
        [Given(@"there are (.*) cucumbers")]
        public void GivenThereAreCucumbers(int start)
        {
            HtmlReporter.Pass("I have " + start);
            this.start = start;
        }

        [When(@"I eat (.*) cucumbers")]
        public void WhenIEatCucumbers(int eat)
        {
            HtmlReporter.Pass("I eat " + eat);
            this.eat = eat;
        }

        [Then(@"I should have (.*) cucumbers")]
        public void ThenIShouldHaveCucumbers(int left)
        {
            result = start - eat;
            if (result.Equals(left))
            {
                HtmlReporter.Pass("I have " + result);
            }
            else
            {
                HtmlReporter.Fail(string.Format("I have [{0}] but [{1}]", result, left));
            }
        }
    }
}

﻿using System;
using TechTalk.SpecFlow;

namespace BDDAutomationTest.BDDTesting.TestSteps
{
    [Binding]
    public class TestSteps
    {
        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
        }

        [Given(@"I have also entered (.*) into the calculator")]
        public void GivenIHaveAlsoEnteredIntoTheCalculator(int p0)
        {
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
        }
    }
}

using System;
using NUnit.Framework;
using System.Collections.Generic;
using Automation_Framework.Core.Common;
using Automation_Framework.Core.WebDriver;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports;
using System.Configuration;
using OpenQA.Selenium;

namespace WebAutomationTest.RegressionTests.TestCases
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {

            string actualImage = @"C:\Users\Public\Pictures\Sample Pictures\Desert.jpg";
            string expectImage = @"C:\Users\Public\Pictures\Sample Pictures\Desert2.jpg";
            var diff = ImageCompare.DiffImage(actualImage, expectImage, 40);
            if (diff != null)
            {
                Console.WriteLine("diff");
            }
            else
            {
                Console.WriteLine("equal");
            }
        }

    }
}

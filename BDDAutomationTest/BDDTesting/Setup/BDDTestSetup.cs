using Automation_Framework.Core.Common;
using Automation_Framework.Core.WebDriver;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

[Binding]
public class BDDTestSetup
{

    protected static WebDriverMethod driver;

    [BeforeTestRun]
    public static void BeforeTestRun()
    {

        driver = new WebDriverMethod();
        driver.Init();

        Utils.CreateReportFolder();
        HtmlReporter.CreateExtentReport();
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        HtmlReporter.Flush();
        driver.DestroyDriver();

    }

    [BeforeFeature]
    public static void BeforeEachFeature()
    {
        var feature = FeatureContext.Current.FeatureInfo;
        HtmlReporter.CreateTest(feature.Title).AssignCategory(feature.Tags);

        FeatureContext.Current["driver"] = driver;
    }

    [AfterFeature]
    public static void AfterEachFeature()
    {

    }

    [BeforeScenario]
    public void BeforeEachScenario()
    {
        var scenario = ScenarioContext.Current.ScenarioInfo;
        HtmlReporter.CreateNode("Scenario", scenario.Title);

    }

    [AfterScenario]
    public void AfterEachScenario()
    {
    }

    [BeforeStep]
    public void BeforeEachStep()
    {
        var step = ScenarioStepContext.Current.StepInfo.StepInstance;
        HtmlReporter.CreateNode(step.Keyword, step.Text);
    }

    [AfterStep]
    public void AfterEachStep()
    {
    }
}

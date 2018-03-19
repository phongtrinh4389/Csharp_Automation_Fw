using Automation_Framework.Core.WebDriver;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automation_Framework.Core.Common
{
    public class HtmlReporter
    {
        private static Dictionary<string, ExtentTest> extentTestMap = new Dictionary<string, ExtentTest>();
        public static ExtentReports _reporter;

        private readonly static string PARENT = "Parent_";
        private readonly static string NODE = "Node_";

        private readonly static string SCENARIO = "Scenario_";

        /// <summary>
        /// To initialize the html reporter. 
        /// This method should be located before a test running or a suite or a class
        /// </summary>
        public static void CreateExtentReport()
        {

            try
            {

                ExtentHtmlReporter htmlReporter = null;

                if (_reporter == null)
                {
                    string reportName = ConfigurationManager.AppSettings["HTML_REPORT_NAME"];
                    htmlReporter = CreateExtentHtmlReporter(reportName);
                    _reporter = new ExtentReports();
                    _reporter.AttachReporter(htmlReporter);

                    _reporter.AddSystemInfo("OS", Environment.OSVersion.ToString());
                    _reporter.AddSystemInfo("Domain", Environment.UserDomainName);
                    _reporter.AddSystemInfo("User", Environment.UserName);

                }

            }
            catch (Exception ex)
            {
                string message = "There is an error when creating html reporter";
                throw new ErrorHandler(message, ex);
            }

        }

        /// <summary>
        /// To add the system information to the Html reporter
        /// </summary>
        /// <param name="key">The attribute name</param>
        /// <param name="value">The attribute value</param>
        public static void AddSystemInfo(string key, string value)
        {
            _reporter.AddSystemInfo(key, value);
        }

        /// <summary>
        /// To write the tests, nodes to the html reporter
        /// This method is mandatory and should be located after a test running
        /// </summary>
        public static void Flush()
        {
            _reporter.Flush();
        }

        private static ExtentHtmlReporter CreateExtentHtmlReporter(string filename)
        {

            try
            {
                string reportPath = Utils.DailyReportPath + Utils.separator + filename;

                var htmlReporter = new ExtentHtmlReporter(reportPath);
                htmlReporter.AppendExisting = true;
                htmlReporter.LoadConfig(Utils.GetPathFromConfig("PATH_EXTENT_CONFIG"));

                return htmlReporter;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To create the html report for a test case
        /// This method is mandatory and should be located before each class or each test method
        /// </summary>
        /// <param name="strTestName">The test case name</param>
        /// <returns>The html report session for the specific test case</returns>
        /// 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string strTestName)
        {
            try
            {

                int threadID = Thread.CurrentThread.ManagedThreadId;
                ExtentTest test = _reporter.CreateTest(strTestName);

                if (extentTestMap.ContainsKey(PARENT + threadID))
                {
                    extentTestMap[PARENT + threadID] = test;
                }
                else
                {
                    extentTestMap.Add(PARENT + threadID, test);
                }

                return test;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// To create the html report for a test case
        /// This method is mandatory and should be located before each class or each test method
        /// </summary>
        /// <param name="strTestName">The test case name</param>
        /// <param name="strTestDescription">The test case description</param>
        /// <returns>The html report session for the specific test case</returns>
        /// 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string strTestName, string strTestDescription)
        {
            try
            {
                int threadID = Thread.CurrentThread.ManagedThreadId;
                ExtentTest test = _reporter.CreateTest(strTestName, strTestDescription);
                if (extentTestMap.ContainsKey(PARENT + threadID))
                {
                    extentTestMap[PARENT + threadID] = test;
                }
                else
                {
                    extentTestMap.Add(PARENT + threadID, test);
                }

                return test;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To create the html report for a test node
        /// This method is mandatory and should be located before each test method
        /// </summary>
        /// <param name="strTestName">The test case name</param>
        /// <param name="strNodeName">The test case node</param>
        /// <param name="strNodeDescription">The test case description</param>
        /// <returns>The html report session for the specific test node</returns>
        /// 
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateNode(string strTestName, string strNodeName, string strNodeDescription)
        {
            ExtentTest test = GetParent();
            int threadID = Thread.CurrentThread.ManagedThreadId;

            if (test == null)
            {
                test = CreateTest(strTestName, strNodeDescription);
            }
            ExtentTest node = test.CreateNode(strNodeName, description: strNodeDescription);

            if (extentTestMap.ContainsKey(NODE + threadID))
            {
                extentTestMap[NODE + threadID] = node;
            }
            else
            {
                extentTestMap.Add(NODE + threadID, node);
            }
            return node;
        }

        /// <summary>
        /// To create the html report for a test node of BDD testing
        /// This method is mandatory and should be located before each test scenario
        /// </summary>
        /// <param name="gherkinKeyword">[Scenario, Given, When, Then, And]</param>
        /// <param name="strDescription">The scenario's description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateNode(string gherkinKeyword, string strDescription)
        {
            ExtentTest test = GetParent();
            int threadID = Thread.CurrentThread.ManagedThreadId;
            if (test == null)
            {
                throw new Exception("ExtentTest for Feature hasn't been initialized!");
            }
            // If keyword is a scenario
            if (gherkinKeyword.Equals("Scenario"))
            {
                ExtentTest scenario = test.CreateNode<Scenario>(strDescription);

                if (extentTestMap.ContainsKey(SCENARIO + threadID))
                {
                    extentTestMap[SCENARIO + threadID] = scenario;
                }
                else
                {
                    extentTestMap.Add(SCENARIO + threadID, scenario);
                }

                return scenario;
            }
            // If keyword is a step
            else
            {
                if (!extentTestMap.ContainsKey(SCENARIO + threadID))
                {
                    throw new Exception("ExtentTest for Scenario hasn't been initialized!");
                }
                else
                {
                    ExtentTest scenario = extentTestMap[SCENARIO + threadID];
                    ExtentTest step = null;
                    if (gherkinKeyword.Contains("Given"))
                    {
                        step = scenario.CreateNode<Given>(strDescription);
                    }
                    else if (gherkinKeyword.Contains("When"))
                    {
                        step = scenario.CreateNode<When>(strDescription);
                    }
                    else if (gherkinKeyword.Contains("And"))
                    {
                        step = scenario.CreateNode<And>(strDescription);
                    }
                    else if (gherkinKeyword.Contains("Then"))
                    {
                        step = scenario.CreateNode<Then>(strDescription);
                    }
                    else
                    {
                        throw new Exception(string.Format("Gherkin keyword [{0}] is not defined", gherkinKeyword));
                    }

                    if (extentTestMap.ContainsKey(NODE + threadID))
                    {
                        extentTestMap[NODE + threadID] = step;
                    }
                    else
                    {
                        extentTestMap.Add(NODE + threadID, step);
                    }
                    return step;
                }


            }

        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetParent()
        {

            int threadID = Thread.CurrentThread.ManagedThreadId;
            if (extentTestMap.ContainsKey(PARENT + threadID))
            {
                return extentTestMap[PARENT + threadID];
            }
            else
            {
                return null;
            }

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetNode()
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            if (extentTestMap.ContainsKey(NODE + threadID))
            {
                return extentTestMap[NODE + threadID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// To get the test report session
        /// </summary>
        /// <returns>ExtentTest</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetTest()
        {
            int threadID = Thread.CurrentThread.ManagedThreadId;
            if (extentTestMap.ContainsKey(NODE + threadID))
            {
                return GetNode();
            }
            else
            {
                return GetParent();
            }
        }

        public static void Pass(string message)
        {
            GetTest().Pass(message);
        }

        public static void Pass(string message, string screenshotPath)
        {
            GetTest().Pass(message).AddScreenCaptureFromPath("file:\\\\" + screenshotPath);
        }

        public static void Fail(string message)
        {
            GetTest().Fail(message);
        }

        public static void Fail(string message, string screenshotPath)
        {
            GetTest().Fail(message).AddScreenCaptureFromPath("file:\\\\" + screenshotPath);
        }

        public static void Fail(string message, Exception ex, string screenshotPath)
        {
            GetTest().Fail(message).Fail(ex).AddScreenCaptureFromPath("file:\\\\" + screenshotPath);
        }

        public static void Fail(string message, Exception ex)
        {
            GetTest().Fail(message).Fail(ex);
        }

        public static void Skip(string message)
        {
            GetTest().Skip(message);
        }

        public static void Skip(string message, string screenshotPath)
        {
            GetTest().Skip(message).AddScreenCaptureFromPath("file:\\\\" + screenshotPath);
        }

        public static void Skip(string message, Exception ex, string screenshotPath)
        {
            GetTest().Skip(message).Skip(ex).AddScreenCaptureFromPath("file:\\\\" + screenshotPath);
        }

        public static void Info(string message)
        {
            IMarkup m = MarkupHelper.CreateLabel(message, ExtentColor.Green);
            GetTest().Info(m);
        }
    }
}

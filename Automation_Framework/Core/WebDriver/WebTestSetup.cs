using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Automation_Framework.Core.Common;
using NLog;
using System.Threading;
using System.Reflection;
using System.Collections;

namespace Automation_Framework.Core.WebDriver
{

    public class WebTestSetup
    {
        protected WebDriverMethod driver;
        protected string browser;

        public WebTestSetup(string browser = null)
        {
            this.browser = browser;
        }

        public WebTestSetup()
        {

        }

        /// <summary>
        /// This method will be executed before each test fixture
        /// </summary>
        [OneTimeSetUp]
        public void BeforeClass()
        {
            try
            {
                string[] category = GetCategory();
                string[] author = GetAuthor();
                string description = GetDescription();
                string strClassName = this.GetType().Name;

                // Create Report folder
                Utils.CreateReportFolder();

                // Start Driver
                driver = new WebDriverMethod();
                driver.Init(this.browser);

                // Init Html reporter
                NLogger.StartTestClass(strClassName, description);
                HtmlReporter.CreateExtentReport();
                HtmlReporter.CreateTest(strClassName, description).AssignAuthor(author).AssignCategory(category);

            }
            catch (Exception ex)
            {
                string message = "An error occurs at BeforeClass";
                throw new ErrorHandler(message, ex);
            }
        }

        /// <summary>
        /// This method will be executed after each test fixture
        /// </summary>
        [OneTimeTearDown]
        public void AfterClass()
        {

            try
            {
                driver.DestroyDriver();

                HtmlReporter.Flush();
            }
            catch (Exception ex)
            {
                string message = "An error occurs at AfterClass";
                throw new ErrorHandler(message, ex);
            }
        }

        /// <summary>
        /// This method will be executed before each test method
        /// </summary>
        [SetUp]
        public void BeforeMethod()
        {
            try
            {
                string strTestName = GetMethodName();
                string strDescription = GetDescription();

                NLogger.StartTestMethod(strTestName, strDescription);

                HtmlReporter.CreateNode(GetClassName(), strTestName + " - " + strDescription, "dsfdsfdf");
                //HtmlReporter.CreateTest(strTestName, strDescription);
            }
            catch (Exception ex)
            {
                string message = "An error occurs at BeforeMethod";
                throw new ErrorHandler(message, ex);
            }

        }

        /// <summary>
        /// This method is executed after each method
        /// </summary>
        [TearDown]
        public void AfterMethod()
        {

        }

        /// <summary>
        /// Get the test's category
        /// </summary>
        /// <returns></returns>
        public string[] GetCategory()
        {
            try
            {
                IList list = TestContext.CurrentContext.Test.Properties["Category"];
                string[] category = new string[list.Count];
                list.CopyTo(category, 0);
                return category;
            }
            catch (Exception)
            {
                return null;

            }
        }

        /// <summary>
        /// Get the test's description
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            try
            {
                IList list = TestContext.CurrentContext.Test.Properties["Description"];
                if (list.Count > 0)
                {
                    string[] description = new string[list.Count];
                    list.CopyTo(description, 0);
                    return string.Join(";", description);
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {
                return "";

            }
        }

        /// <summary>
        /// Get the test method's name
        /// </summary>
        /// <returns></returns>
        public string GetMethodName()
        {
            try
            {
                return TestContext.CurrentContext.Test.MethodName;

            }
            catch (Exception)
            {
                return "";

            }
        }

        /// <summary>
        /// To get the class name
        /// </summary>
        /// <returns></returns>
        public string GetClassName()
        {
            try
            {
                return TestContext.CurrentContext.Test.ClassName;

            }
            catch (Exception)
            {
                return "";

            }
        }

        /// <summary>
        /// To get the test's full name
        /// </summary>
        /// <returns></returns>
        public string GetFullName()
        {
            try
            {
                return TestContext.CurrentContext.Test.FullName;

            }
            catch (Exception)
            {
                return "";

            }
        }

        /// <summary>
        /// To get the author
        /// </summary>
        /// <returns></returns>
        public string[] GetAuthor()
        {
            try
            {
                IList list = TestContext.CurrentContext.Test.Properties["Author"];
                string[] author = new string[list.Count];
                list.CopyTo(author, 0);
                return author;
            }
            catch (Exception)
            {
                return null;

            }
        }

    }
}

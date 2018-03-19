using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using NLog;
using NLog.Config;
using System.Configuration;
using NT_Selenium_Automation_Framework.Utils;
using KCL_AutomationTest.Modules;
using System.Data;

namespace KCL_AutomationTest
{
    [TestClass]
    public class UnitTest1
    {
        public IWebDriver driver;
        public static Logger logger = LogManager.GetCurrentClassLogger();
        private TestContext testContextInstance;
        string browser = null;
        string path = null;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
        
        [TestInitialize]
        public void SetUp()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigurationManager.AppSettings["PATHLOGCONFIG"]);
            browser = ConfigurationManager.AppSettings["BROWSER"];
            path = ConfigurationManager.AppSettings["PATHDRIVER"];
            driver = null;
            driver = new DriverFactory(browser, path).GetDriver();
            string url = ConfigurationManager.AppSettings["URL"];
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();           
        }
        [TestMethod]       
        [DataSource("System.Data.OleDb", "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='F:\\Automated Testing\\Project\\KCL_AutomationPOC_Project\\KCL_AutomationTest\\TestData\\Data.xlsx';Persist Security Info=False;Extended Properties='Excel 12.0;HDR=Yes'", "Login$", DataAccessMethod.Sequential)]
        public void TestMethod1()
        {
            string email = TestContext.DataRow["Email"].ToString();
            string password = TestContext.DataRow["password"].ToString();
            BusinessFunctions.doLogin(driver, email, password);

            //***** Do Payment *****
            string cardNumber = TestContext.DataRow["CardNumber"].ToString();
            string expireMonth = TestContext.DataRow["ExpireMonth"].ToString();
            string expireYear = TestContext.DataRow["ExpireYear"].ToString();
            string securityCode = TestContext.DataRow["SecurityCode"].ToString();
            string cardHolderName = TestContext.DataRow["CardHolderName"].ToString();
            BusinessFunctions.doPayNow(driver, cardNumber, expireMonth, expireYear, securityCode, cardHolderName);


        }
        
        [TestCleanup]
        public void TearDown()
        {
            Console.WriteLine("to do teardown");
            //driver.Quit();
            //driver.Dispose();
        }
    }
}

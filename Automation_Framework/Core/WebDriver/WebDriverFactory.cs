using Automation_Framework.Core.Common;
using BrowserStack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Automation_Framework.Core.WebDriver
{
    public class WebDriverFactory
    {
        public IWebDriver driver;
        private Local browserStackLocal = null;

        /// <summary>
        ///   To initialize the browser
        /// </summary>
        /// <param name="browser">
        ///  Select the browser from [chrome, firefox, ie, edge, safari]
        ///  Otherwise, it's default as app.config
        /// </param>
        public void Init(string browser = null)
        {
            try
            {
                bool BROWSER_MODE = ConfigurationManager.AppSettings.Get("BROWSERSTACK").ToString().Equals("true");

                if (BROWSER_MODE)
                {
                    driver = RemoteDriver(browser);
                }
                else
                {
                    driver = LocalDriver(browser);
                }

                SetDimension();
            }
            catch (Exception ex)
            {

                string message = "There is an error when creating driver";
                throw new ErrorHandler(message, ex);
            }
        }
        /// <summary>
        /// To setup browser for Local Testing
        /// </summary>
        /// <param name="browser">[chrome, firefox, ie, edge]</param>
        /// <returns>IWebDriver</returns>
        private IWebDriver LocalDriver(string browser)
        {
            IWebDriver driver;
            string path = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["PATHDRIVER"];

            if (browser == null)
            {
                browser = ConfigurationManager.AppSettings["BROWSER"];
            }

            switch (browser)
            {
                case "chrome":

                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("test-type");
                    options.AddArgument("--start-maximized");
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService(path, "chromedriver.exe");
                    service.HideCommandPromptWindow = false;
                    driver = new ChromeDriver(service, options);
                    break;

                case "ie":

                    InternetExplorerOptions IEoptions = new InternetExplorerOptions();
                    IEoptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    IEoptions.IgnoreZoomLevel = true;
                    IEoptions.EnableNativeEvents = false;
                    IEoptions.EnablePersistentHover = true;
                    driver = new InternetExplorerDriver(path, IEoptions);
                    break;

                case "edge":

                    driver = new EdgeDriver(path);
                    break;

                case "firefox":

                    FirefoxDriverService FFservice = FirefoxDriverService.CreateDefaultService(path, "geckodriver.exe");
                    driver = new FirefoxDriver(FFservice);
                    break;
                default:
                    driver = null;
                    break;
            }

            return driver;

        }
        /// <summary>
        /// To setup environment for testing with BrowserStack
        /// </summary>
        /// <param name="environment">[chrome, firefox, ie, safari, edge, mobile]</param>
        /// <returns>IWebDriver</returns>
        private IWebDriver RemoteDriver(string environment)
        {

            string username = ConfigurationManager.AppSettings.Get("bs_user");
            string accesskey = ConfigurationManager.AppSettings.Get("bs_key");
            string server = ConfigurationManager.AppSettings.Get("bs_server");

            // Set default envrionment if it's null
            if (environment == null)
            {
                environment = ConfigurationManager.AppSettings["BROWSER"];
            }
            NameValueCollection caps = ConfigurationManager.GetSection("bs_environments/" + environment) as NameValueCollection;

            DesiredCapabilities capability = new DesiredCapabilities();
            foreach (string key in caps.AllKeys)
            {
                capability.SetCapability(key, caps[key]);
            }

            capability.SetCapability("browserstack.user", username);
            capability.SetCapability("browserstack.key", accesskey);

            // Set debug mode
            if (ConfigurationManager.AppSettings.Get("browserstack.debug").ToString() == "true")
            {
                capability.SetCapability("browserstack.debug", true);
            }

            // Set Local mode
            if (ConfigurationManager.AppSettings.Get("browserstack.local").ToString() == "true")
            {
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("key", accesskey)
                };
                browserStackLocal.start(bsLocalArgs);
            }

            return new RemoteWebDriver(new Uri("http://" + server + "/wd/hub/"), capability);
        }

        /// <summary>
        /// Set the Browser Size, this method is usually used when testing responsive
        /// The parameters are optionaly, if you forgot it, the framework will get from App.config
        /// </summary>
        /// <param name="width">The browser's width</param>
        /// <param name="height">The browser's height</param>
        public void SetDimension(string width = null, string height = null)
        {
            string strWidth = width == null ? ConfigurationManager.AppSettings.Get("WIDTH") : width;
            string strHeight = height == null ? ConfigurationManager.AppSettings.Get("HEIGHT") : height;
            if (!strWidth.Equals("") && !strHeight.Equals(""))
            {
                driver.Manage().Window.Size = new Size(Convert.ToInt32(strWidth), Convert.ToInt32(strHeight));
            }
            else
            {
                driver.Manage().Window.Maximize();
            }

        }

        /// <summary>
        /// To get the driver session
        /// </summary>
        /// <returns>IWebDriver</returns>
        public IWebDriver GetDriver()
        {
            return driver;
        }

        /// <summary>
        /// Get the Browser Type
        /// </summary>
        /// <returns>Browser Type</returns>
        public string GetBrowserType()
        {
            try
            {
                ICapabilities caps = ((RemoteWebDriver)driver).Capabilities;
                return caps.BrowserName;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// To close the web driver
        /// </summary>
        public void DestroyDriver()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                    driver.Dispose();
                }
                if (browserStackLocal != null)
                {
                    browserStackLocal.stop();
                }

            }
            catch (Exception ex)
            {
                string message = "There is an error when closing driver";
                throw new ErrorHandler(message, ex);
            }
        }
    }
}

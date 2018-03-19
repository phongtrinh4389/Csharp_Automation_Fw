using Automation_Framework.Core.Common;
using NLog;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Framework.Core.WebDriver
{
    static class ScreenShotCapturing
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static bool IsCaptureEnable()
        {
            var screenshot = ConfigurationManager.AppSettings["ISTAKESCREENSHOT"];
            return Convert.ToBoolean(screenshot);
        }

        public static string TakeScreenShoot(IWebDriver driver, string errorText)
        {
            if (!IsCaptureEnable())
            {
                return "";
            }
            AddErrorText(driver, errorText);
            return Capture(driver);
        }

        public static string Capture(IWebDriver driver)
        {
            var screenshotDirectory = Utils.ScreenshotPath;
            var fileName = string.Format(@"Screenshot_{0}.png", DateTime.Now.ToString("yyyyMMdd_HHmmssfff"));
            System.IO.Directory.CreateDirectory(screenshotDirectory);
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshot = ss.AsBase64EncodedString;
            string fileLocation = string.Format(@"{0}\{1}", screenshotDirectory, fileName);
            ss.SaveAsFile(fileLocation, ScreenshotImageFormat.Png);
            logger.Debug(string.Format("Screenshot captured. File location: {0}", fileLocation));

            return fileLocation;
        }

        public static void HighlightElement(IWebDriver driver, IWebElement element)
        {
            if (element != null)
            {
                IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
                String args = "arguments[0].setAttribute('style', arguments[1]);";
                executor.ExecuteScript(args, element, "border: 8px solid red;display:block;z-index:1;");
            }
            else
            {
                return;
            }
        }

        public static void AddErrorText(IWebDriver driver, string errorText)
        {
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;
            errorText = errorText.Replace("'", "");
            executor.ExecuteScript("var para = document.createElement('H1');" +
                string.Format("var text = document.createTextNode('{0}');", errorText) +
                "para.appendChild(text);" +
                "para.style.color = 'red';" +
                "para.setAttribute('style','border: 5px solid red; position: absolute; top:0; left:0; background-color: white; display:block; z-index:1;');" +
                "document.body.appendChild(para);");
        }
    }
}

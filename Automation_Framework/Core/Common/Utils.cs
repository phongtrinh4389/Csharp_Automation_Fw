using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Automation_Framework.Core.Common
{
    public class Utils
    {
        private static string _strReportPath = "";
        private static string _strBaselinePath = "";
        private static string _strDailyReportPath = "";
        private static string _strScreenshotPath = "";
        private static string _strActualScreenshotPath = "";
        private static string _strDiffScreenshotPath = "";
        public static char separator = Path.DirectorySeparatorChar;
        private static bool DAILY_REPORT_ENABLED = Convert.ToBoolean(ConfigurationManager.AppSettings["DAILY_REPORT_ENABLED"]);
        private static bool UI_TEST_ENABLED = Convert.ToBoolean(ConfigurationManager.AppSettings["UI_TEST_ENABLED"]);
        private static string SQL_SERVER_CONNECTION_STRING = ConfigurationManager.AppSettings["SQL_SERVER_CONNECTION_STRING"];
        public static string ReportPath
        {
            get
            {
                if (_strReportPath.Equals(""))
                {
                    _strReportPath = GetPathFromConfig("REPORT_PATH");
                }

                if (DAILY_REPORT_ENABLED)
                {
                    if (!Directory.Exists(_strReportPath))
                    {
                        Directory.CreateDirectory(_strReportPath);
                    }

                    return _strReportPath;
                }
                else
                {
                    return AppDomain.CurrentDomain.BaseDirectory;
                }

            }
        }

        public static string BaselinePath
        {
            get
            {
                if (_strBaselinePath.Equals(""))
                {
                    _strBaselinePath = GetPathFromConfig("BASELINE_PATH");
                }

                if (DAILY_REPORT_ENABLED && UI_TEST_ENABLED)
                {
                    if (!Directory.Exists(_strBaselinePath))
                    {
                        Directory.CreateDirectory(_strBaselinePath);
                    }

                    return _strBaselinePath;
                }
                else
                {
                    return "";
                }

            }
        }

        public static string DailyReportPath
        {
            get
            {
                if (_strDailyReportPath.Equals(""))
                {
                    _strDailyReportPath = ReportPath + separator + "TestResults_" + GetCurrentDateTime();
                }

                if (DAILY_REPORT_ENABLED)
                {
                    if (!Directory.Exists(_strDailyReportPath))
                    {
                        Directory.CreateDirectory(_strDailyReportPath);
                    }

                    return _strDailyReportPath;
                }
                else
                {
                    return AppDomain.CurrentDomain.BaseDirectory;
                }
            }
        }

        public static string ActualScreenshotPath
        {
            get
            {
                _strActualScreenshotPath = DailyReportPath + separator + "Actual";

                if (DAILY_REPORT_ENABLED && UI_TEST_ENABLED)
                {
                    if (!Directory.Exists(_strActualScreenshotPath))
                    {
                        Directory.CreateDirectory(_strActualScreenshotPath);
                    }

                }
                return _strActualScreenshotPath;

            }
        }

        public static string DiffScreenshotPath
        {
            get
            {
                _strDiffScreenshotPath = DailyReportPath + separator + "Diff";

                if (DAILY_REPORT_ENABLED && UI_TEST_ENABLED)
                {
                    if (!Directory.Exists(_strDiffScreenshotPath))
                    {
                        Directory.CreateDirectory(_strDiffScreenshotPath);
                    }

                }
                return _strDiffScreenshotPath;

            }
        }

        public static string ScreenshotPath
        {
            get
            {
                if (_strScreenshotPath.Equals(""))
                {
                    if (DAILY_REPORT_ENABLED)
                    {
                        _strScreenshotPath = DailyReportPath + separator + "Screenshots";
                    }
                    else
                    {
                        _strScreenshotPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["SCREENSHOTPATH"];
                    }

                }

                if (!Directory.Exists(_strScreenshotPath))
                {
                    Directory.CreateDirectory(_strScreenshotPath);
                }

                return _strScreenshotPath;

            }
        }

        public static string GetPathFromConfig(string pathKey)
        {
            return AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings[pathKey];

        }

        public static void CreateReportFolder()
        {
            Console.WriteLine(ReportPath);
            Console.WriteLine(ScreenshotPath);
            Console.WriteLine(DailyReportPath);
            Console.WriteLine(ActualScreenshotPath);
            Console.WriteLine(DiffScreenshotPath);
            Console.WriteLine(BaselinePath);

        }

        public string CreateEmailByCurrentTime()
        {
            DateTime time = DateTime.Now;
            string format = "MMM_ddd_d_HH_mm_yyyy";
            return string.Concat(time.ToString(format), "@gmail.com");
        }

        public static string GetCurrentDateTime()
        {
            DateTime time = DateTime.Now;
            return time.ToString(Constant.TIME_STAMP_1);
        }

        public static void CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {

            }
        }

        public static SqlConnection GetSqlServerConnection()
        {
            SqlConnection connection = new SqlConnection(SQL_SERVER_CONNECTION_STRING);
            try
            {
                connection.Open();
                Console.WriteLine("SQL Server database connected.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Server database can't be connected." + ex.Message);
            }
            return connection;
        }

        public static DataTable ReadSqlData(string query)
        {
            SqlDataReader reader;
            DataTable dataTable;
            using (SqlConnection connection = GetSqlServerConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                dataTable = new DataTable();
                dataTable.Load(reader);
            }
            return dataTable;
        }
    }
}

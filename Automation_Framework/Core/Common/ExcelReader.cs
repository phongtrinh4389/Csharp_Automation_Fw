using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Excel;
using System.Collections;
using System.Threading;

namespace Automation_Framework.Core.Common
{
    public class ExcelReader
    {
        protected string ExcelFile { get; set; }

        protected List<string> Sheets { get; set; }

        public IExcelDataReader GetExcelReader(string excelFile)
        {
            var stream = GetExcelFileStreamFromFileSystem(excelFile);

            IExcelDataReader excelReader;
            var extensao = excelFile.Trim().Substring(excelFile.LastIndexOf("."));
            switch (extensao.ToUpper())
            {
                case ".XLS":
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
                case ".XLSX":
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    break;
                default:
                    throw new ArgumentException("Unknown file extension.");
            }
            return excelReader;
        }

        public Stream GetExcelFileStreamFromFileSystem(string excelFilePath)
        {
            try
            {
                ExcelFile = excelFilePath;
                var fileStream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);
                return fileStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ExcelReader AddExcel(String excelFilePath)
        {
            ExcelFile = excelFilePath;
            return this;
        }
        public ExcelReader AddSheet(string sheetName)
        {
            if (Sheets == null)
            {
                Sheets = new List<string>();
            }
            var excelReader = GetExcelReader(ExcelFile);
            var result = excelReader.AsDataSet();
            var sheetTable = result.Tables[sheetName];
            if (sheetTable != null)
            {
                Sheets.Add(sheetName);
            }
            else
            {
                throw new Exception(string.Format("The Sheet [{0}] is not existing in the file [{1}]", sheetName, ExcelFile));
            }
            return this;
        }

        public List<TestCaseData> GetTestCases(Func<string, DataRow, int, TestCaseData> testCaseDataCreator)
        {
            var testDataList = new List<TestCaseData>();
            var excelReader = GetExcelReader(ExcelFile);
            excelReader.IsFirstRowAsColumnNames = true;
            var result = excelReader.AsDataSet();
            foreach (var sheet in Sheets)
            {
                var sheetTable = result.Tables[sheet];
                var i = 0;
                foreach (DataRow dr in sheetTable.Rows)
                {
                    testDataList.Add(testCaseDataCreator(sheet, dr, i));
                    i = i + 1;
                }
            }

            excelReader.Close();
            return testDataList;

        }

        public static IEnumerable<TestCaseData> GetTestCaseData(string filename, string[] sheets)
        {
            ExcelReader reader = new ExcelReader();
            reader.AddExcel(filename);
            foreach (string sheet in sheets)
            {
                reader.AddSheet(sheet);
            }
            var testcases = reader.GetTestCases(delegate (string sheet, DataRow row, int rowNum)
            {
                var testName = sheet + "_" + rowNum;

                Dictionary<string, string> testDataArgs = new Dictionary<string, string>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    testDataArgs.Add(column.ColumnName, row[column].ToString());
                }
                TestCaseData testData = new TestCaseData(testDataArgs).SetName(testName);
                return testData;
            }
            );

            foreach (TestCaseData testCaseData in testcases)
            {
                yield return testCaseData;
            }
        }

        public static IEnumerable<TestCaseData> GetTestData(string filename, string[] sheets)
        {
            IEnumerable<TestCaseData> testCases = null;
            try
            {
                string excelFile = Utils.GetPathFromConfig("PATHDATA") + filename;
                //string sheetname = sheetName;
                //testCases = ExcelReader.GetTestCaseData(excelFile, sheetname);
                testCases = ExcelReader.GetTestCaseData(excelFile, sheets);

            }
            catch (Exception ex)
            {
                string message = "There is an error when getting TestData from excel file";
                throw new ErrorHandler(message, ex);
            }

            foreach (TestCaseData testCaseData in testCases)
            {
                Dictionary<string, string> data = (Dictionary<string, string>)testCaseData.Arguments[0];
                string description = string.Join(";", data.Select(x => "{" + x.Key + "=" + x.Value + "}").ToArray());
                testCaseData.SetDescription(description);
                yield return testCaseData;

            }
        }

    }
}

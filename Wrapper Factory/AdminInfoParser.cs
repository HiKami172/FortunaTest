using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ExcelDataReader;
using System.Text;

namespace FortunaTest.WrapperFactory
{
    public class AdminInfoParser
    {
        static IParser parser;

        public static List<string> GetAdminInfo(string projectDirPath)
        {
            string extension = "";
            string adminInfoPath = "";
            foreach (string fileName in Directory.GetFiles(projectDirPath))
            {
                if (fileName.Contains("adminInfo"))
                {
                    adminInfoPath = fileName;

                    extension = Path.GetExtension(adminInfoPath);
                    break;
                }
            }

            switch (extension)
            {
                case ".csv":
                    parser = new CSVParser();
                    break;
                case ".xml":
                    parser = new XMLParser();
                    break;
                case ".xlsx":
                    parser = new XLSXParser();
                    break;
                default:
                    throw new Exception("Wrong adminInfo file extension!");
            }

            return parser.Parse(adminInfoPath);
        }
    }

    public interface IParser
    {
        public List<string> Parse(string filePath);
    }

    public class XLSXParser : IParser
    {
        public List<string> Parse(string filePath)
        {
            List<string> data = new List<string>();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                var dataSet = reader.AsDataSet();
                var dataTable = dataSet.Tables[0];

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        var innerData = dataTable.Rows[i][j];
                        data.Add(innerData.ToString());
                    }
                }
            }
            return data;
        }
    }

    public class XMLParser : IParser
    {
        public List<string> Parse(string filePath)
        {
            List<string> data = new List<string>();

            XmlDocument file = new XmlDocument();
            file.Load(filePath);
            XmlElement root = file.DocumentElement;
            foreach (XmlNode node in root)
            {
                if (node.Name == "username")
                    data.Add(node.InnerText);
                if (node.Name == "password")
                    data.Add(node.InnerText);
            }
            return data;
        }
    }

    public class CSVParser : IParser
    {
        public List<string> Parse(string filePath)
        {
            List<string> data = new List<string>();

            string text = File.ReadAllText(filePath);
            string[] usernameXpassword = text.Split(',');
            data.AddRange(usernameXpassword);
            return data;
        }
    }
}

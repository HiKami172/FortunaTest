using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Excel;

namespace FortunaTest.common
{
    public class AdminInfoParser
    {
        static IParser parser;

        public static (string, string) GetAdminInfo(string projectDirPath)
        {
            string extension = "";
            string adminInfoPath = "";
            foreach (string fileName in Directory.GetFiles(projectDirPath))
                if (fileName.Contains("adminInfo"))
                {
                    adminInfoPath =  fileName;

                    extension = fileName.Split('.')[1];
                    break;
                }
            if (extension == "csv")
                parser = new CSVParser();
            else if (extension == "xml")
                parser = new XMLParser();
            else if (extension == "xlsx")
                parser = new XLSXParser();
            else
                throw new Exception("Wrong adminInfo file extension!");

            return parser.Parse(adminInfoPath);
        }
    } 
    public interface IParser
    {
        public (string, string) Parse(string filePath);
    }

    public class XLSXParser : IParser
    {
        public (string, string) Parse(string filePath)
        {
            List<string> data = new List<string>();
            foreach (var worksheet in Workbook.Worksheets(filePath))
                foreach (var row in worksheet.Rows)
                    foreach (var cell in row.Cells)
                        if (cell != null)
                            data.Add(cell.Text);
            return (data[0], data[1]);
        }
    }
    public class XMLParser : IParser
    {
        public (string, string) Parse(string filePath)
        {
            string username = "";
            string password = "";

            XmlDocument file = new XmlDocument();
            file.Load(filePath);
            XmlElement root = file.DocumentElement;
            foreach(XmlNode node in root)
            {
                if (node.Name == "username")
                    username = node.InnerText;
                if (node.Name == "password")
                    password = node.InnerText;
            }
            return (username, password);
        }
    }
    public class CSVParser : IParser
    {
        public (string, string) Parse(string filePath)
        {
            string text = File.ReadAllText(filePath);
            string[] usernameXpassword = text.Split(',');
            return (usernameXpassword[0], usernameXpassword[1]);
        }
    }
}

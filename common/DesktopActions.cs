using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace FortunaTest.common
{
    public class DesktopHelper
    {
        public static void ShowDesktop(WindowsDriver<WindowsElement> desktopSession)
        {
            IWebElement showDesktopBtn = desktopSession.FindElement(By.Name("Show desktop"));
            showDesktopBtn.Click();
        }

        public static void ExecuteAsAdmin(string fileFullPath)
        {
            Process execution = new Process();
            ProcessStartInfo options = new ProcessStartInfo()
            {
                FileName = fileFullPath,
                UseShellExecute = true,
                Verb = "runas"
            };

            execution.StartInfo = options;
            execution.Start();
            execution.WaitForExit();
        }
    }
}

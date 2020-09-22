using FortunaTest.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;

namespace FortunaTest
{
    [TestClass]
    public class UnitTest
    {
        private const string APPDRIVERURL = "http://127.0.0.1:4723";
        private static RemoteWebDriver AppSession;
        private static WindowsDriver<WindowsElement> DesktopSession;
        private static TestCases Cases;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var deskCap = new AppiumOptions();
            deskCap.AddAdditionalCapability("app", "Root");
            DesktopSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), deskCap);
            Assert.IsNotNull(DesktopSession);
            Process.Start(@"C:\Program Files (x86)\Agfa\Security\ag_prog_fortuna_v130\com_win\fortuna.bat");

            DesktopSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            var AppWindow = DesktopSession.FindElementByName("Fortuna");
            var AppTopLevelWindowHandle = AppWindow.GetAttribute("NativeWindowHandle");
            AppTopLevelWindowHandle = ConvertToHex(AppTopLevelWindowHandle); 
            var cap = new AppiumOptions();
            cap.AddAdditionalCapability("appTopLevelWindow", AppTopLevelWindowHandle);
            AppSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), cap);
            Assert.IsNotNull(AppSession);
            Cases = new TestCases(AppSession);

            TimeSpan TimeToWait = TimeSpan.FromSeconds(3);
            AppSession.Manage().Timeouts().ImplicitWait = TimeToWait;
        }

        [ClassCleanup]
        public static void TestsCleanup()
        {
            AppSession.Dispose();
            AppSession = null;
        }

        [TestMethod]
        public void FileCreation()
        {
            Cases.FileCreation();
        }

        private static string ConvertToHex(string handler)
        {
            return (int.Parse(handler)).ToString("x");
        }
    }
}

using FortunaTest.Test;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;

namespace FortunaTest
{
    [TestClass]
    public class UnitTest
    {
        private const string APPDRIVERURL = "http://127.0.0.1:4723";
        private static RemoteWebDriver appSession;
        private static WindowsDriver<WindowsElement> desktopSession;
        private static TestCases cases;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var deskCap = new AppiumOptions();
            deskCap.AddAdditionalCapability("app", "Root");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), deskCap);
            Assert.IsNotNull(desktopSession);

            TimeSpan TimeToStart = TimeSpan.FromSeconds(10);
            desktopSession.Manage().Timeouts().ImplicitWait = TimeToStart;

            IWebElement appShortcut = desktopSession.FindElementByName("Fortuna X3");
            Actions actions = new Actions(desktopSession);
            actions
                .KeyDown(Keys.Command)
                .SendKeys("d")
                .KeyUp(Keys.Command)
                .DoubleClick(appShortcut)
                .Perform();

            var appWindow = desktopSession.FindElementByName("Fortuna");
            var appTopLevelWindowHandle = appWindow.GetAttribute("NativeWindowHandle");
            appTopLevelWindowHandle = ConvertToHex(appTopLevelWindowHandle); 
            var cap = new AppiumOptions();
            cap.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHandle);
            appSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), cap);
            Assert.IsNotNull(appSession);

            cases = new TestCases(appSession);

            TimeSpan TimeToWait = TimeSpan.FromSeconds(3);
            appSession.Manage().Timeouts().ImplicitWait = TimeToWait;
        }

        [ClassCleanup]
        public static void TestsCleanup()
        {
            appSession.Dispose();
            appSession = null;
        }

        [TestMethod]
        public void FileCreation()
        {
            cases.FileCreation();
        }

        private static string ConvertToHex(string handler)
        {
            return (int.Parse(handler)).ToString("x");
        }
    }
}

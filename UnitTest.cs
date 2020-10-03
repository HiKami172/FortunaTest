using FortunaTest.Test;
using FortunaTest.common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.IO;

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
            SpecialFunctions.CompileScripts();

            if (!SpecialFunctions.IsInstalled())
                ExecuteFromProject("\\Scripts\\install.exe");

            var deskCap = new AppiumOptions();
            deskCap.AddAdditionalCapability("app", "Root");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), deskCap);
            Assert.IsNotNull(desktopSession);

            TimeSpan TimeToStart = TimeSpan.FromSeconds(20);
            desktopSession.Manage().Timeouts().ImplicitWait = TimeToStart;
        }

        [ClassCleanup]
        public static void TestsCleanup()
        {
            appSession.Dispose();
            appSession = null;
        }

        [TestMethod]
        [Priority(1)]
        public void Installation()
        {
            Assert.IsTrue(SpecialFunctions.IsInstalled());
        }

        [TestMethod]
        [Priority(2)]
        public void Launch()
        {
            OpenFortuna();
            Assert.IsNotNull(appSession);
        }

        [TestMethod]
        [Priority(3)]
        public void FileCreation()
        {
            cases.FileCreation();
        }

        [TestMethod]
        [Priority(4)]
        public void Uninstallation()
        {
            ExecuteFromProject("\\Scripts\\unistall.exe");
            Assert.IsFalse(SpecialFunctions.IsInstalled());
        }

        private static void ExecuteFromProject(string fileLocalPath)
        {
            string projectFullPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            DesktopHelper.ExecuteAsAdmin(@projectFullPath + @fileLocalPath);
        }

        private static void OpenFortuna()
        {
            TimeSpan TimeToWait = TimeSpan.FromSeconds(3);

            IWebElement appShortcut = desktopSession.FindElementByName("Fortuna X3");
            Actions actions = new Actions(desktopSession);
            DesktopHelper.ShowDesktop();
            actions
                .DoubleClick(appShortcut)
                .Perform();

            var appWindow = desktopSession.FindElementByName("Fortuna");
            var appTopLevelWindowHandle = appWindow.GetAttribute("NativeWindowHandle");
            appTopLevelWindowHandle = SpecialFunctions.ConvertToHex(appTopLevelWindowHandle);
            var cap = new AppiumOptions();
            cap.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHandle);
            appSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), cap);
            Assert.IsNotNull(appSession);

            cases = new TestCases(appSession);

            appSession.Manage().Timeouts().ImplicitWait = TimeToWait;
        }
    }
}

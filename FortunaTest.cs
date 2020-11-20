using FortunaTest.Tests;
using FortunaTest.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace FortunaTest
{
    [TestFixture]
    public class FortunaTest
    {
        private const string APPDRIVERURL = "http://127.0.0.1:4723";
        private const string APPLICATIONNAME = "Fortuna X3";

        private static RemoteWebDriver appSession;
        private static WindowsDriver<WindowsElement> desktopSession;

        private static AccountTests accountTests;
        private static FileMenuBarTests functionalTests;


        [SetUp]
        public static void Setup()
        {
            SpecialFunctions.CompileScripts();

            if (!SpecialFunctions.IsInstalled())
                ExecuteFromProject("\\Scripts\\install.exe");

            var deskCap = new AppiumOptions();
            deskCap.AddAdditionalCapability("app", "Root");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), deskCap);
            Assert.IsNotNull(desktopSession);

            TimeSpan timeToStart = TimeSpan.FromSeconds(20);
            desktopSession.Manage().Timeouts().ImplicitWait = timeToStart;
        }

        [Test, Order(1)]
        public void Install()
        {
            Assert.IsTrue(SpecialFunctions.IsInstalled());
        }

        [Test, Order(2)]
        public void Launch()
        {
            OpenFortuna();
            Assert.IsNotNull(appSession);
        }

        [Test, Order(3)]
        public void LoginAdminAccount()
        {
            accountTests.LoginAdminAccount();
        }

        [Test, Order(4)]
        public void AddNewUser()
        {
            accountTests.AddNewUser();
        }

        [Test, Order(5)]
        public void CreateFile()
        {
            functionalTests.CreateFile();
        }

        [Test, Order(6)]
        public void LoginUserAccount()
        {
            accountTests.LoginAccount(accountTests.username, accountTests.password);
        }

        [Test, Order(7)]
        public void CheckUserCapabilities()
        {
            accountTests.CheckUserCapabilities();
        }

        [Test, Order(8)]
        public void LogoutAccount()
        {
            accountTests.LogoutAccount();
        }

        [Test, Order(9)]
        public void LoginWithWrongCredentials()
        {
            accountTests.LoginWithWrongCredentials();
        }

        [Test, Order(10)]
        public void RemoveUser()
        {
            accountTests.RemoveUser();
        }

        [Test, Order(11)]
        public void CloseFortuna()
        {
            appSession.Close();
            try
            {
                desktopSession.FindElementByName("Fortuna");
                Assert.Fail();
            }
            catch (WebDriverException) { }
        }

        [Test, Order(12)]
        public void Uninstall()
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
            TimeSpan timeToWait = TimeSpan.FromSeconds(3);

            IWebElement appShortcut = desktopSession.FindElementByName(APPLICATIONNAME);
            Actions actions = new Actions(desktopSession);
            DesktopHelper.ShowDesktop(desktopSession);
            actions.DoubleClick(appShortcut)
                   .Perform();

            var appWindow = desktopSession.FindElementByName("Fortuna");
            var appTopLevelWindowHandle = appWindow.GetAttribute("NativeWindowHandle");
            appTopLevelWindowHandle = SpecialFunctions.ConvertToHex(appTopLevelWindowHandle);
            var cap = new AppiumOptions();
            cap.AddAdditionalCapability("appTopLevelWindow", appTopLevelWindowHandle);
            appSession = new WindowsDriver<WindowsElement>(new Uri(APPDRIVERURL), cap);
            Assert.IsNotNull(appSession);

            accountTests = new AccountTests(appSession);
            functionalTests = new FileMenuBarTests(appSession);

            appSession.Manage().Timeouts().ImplicitWait = timeToWait;
        }
    }
}

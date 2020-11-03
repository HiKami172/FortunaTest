using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using FortunaTest.WrapperFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace FortunaTest.Dialogs
{
    class LoginDialog
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);

        private string AdminUsername { get; }
        private string AdminPassword { get; }

        private readonly Actions enterBtn;
        private readonly Actions clickLoginBtn;  

        private RemoteWebDriver AppSession { get; set; }

        public LoginDialog(RemoteWebDriver appSession)
        {
            AppSession = appSession;

            enterBtn = new Actions(AppSession).SendKeys(Keys.Enter);

            IWebElement applicationPane = AppSession.FindElementByName("Application");

            clickLoginBtn = new Actions(AppSession)
                .MoveToElement(applicationPane, 20, 510)
                .Click();

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirPath = Directory.GetParent(workingDirectory).Parent.Parent.FullName + @"\FilesForParse";
            List<string> usernameXpassword = AdminInfoParser.GetAdminInfo(projectDirPath);
            AdminUsername = usernameXpassword[0];
            AdminPassword = usernameXpassword[1];
        }

        public void ClickLoginBtn()
        {
            clickLoginBtn.Perform();
        }

        public void LoginAccount(string username, string password)
        {
            ClickLoginBtn();

            IWebElement loginDialogue = AppSession.FindElementByName("Fortuna Login");
            ReadOnlyCollection<IWebElement> inputFields = loginDialogue.FindElements(By.XPath(".//Edit"));

            inputFields[1].SendKeys(username);
            Thread.Sleep(timeToSleep);

            inputFields[0].SendKeys(password);
            Thread.Sleep(timeToSleep);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);
        }

        public void LoginAdminAccount()
        {
            LoginAccount(AdminUsername, AdminPassword);
        }

        public void CheckForLoginDeniedWarning()
        {
            IWebElement loginDeniedWarning = AppSession.FindElementByName("Login denied for this Fortuna session");
            Assert.IsNotNull(loginDeniedWarning);
            Thread.Sleep(timeToSleep);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);
        }
    }
}

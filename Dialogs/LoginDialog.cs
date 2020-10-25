using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using FortunaTest.common;

namespace FortunaTest.Dialogs
{
    class LoginDialog
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);

        private string adminUsername;
        private string adminPassword;

        private Actions enterBtn;
        private Actions clickLoginBtn;  

        private RemoteWebDriver appSession { get; set; }

        public LoginDialog(RemoteWebDriver appSession)
        {
            this.appSession = appSession;

            enterBtn = new Actions(appSession).SendKeys(Keys.Enter);

            IWebElement applicationPane = appSession.FindElementByName("Application");

            clickLoginBtn = new Actions(appSession)
                .MoveToElement(applicationPane, 20, 510)
                .Click();

            string projectDirPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            (string, string) usernameXpassword = AdminInfoParser.GetAdminInfo(projectDirPath);
            adminUsername = usernameXpassword.Item1;
            adminPassword = usernameXpassword.Item2;
        }

        public void ClickLoginBtn()
        {
            clickLoginBtn.Perform();
        }

        public void LoginAccount(string username, string password)
        {
            ClickLoginBtn();

            IWebElement loginDialogue = appSession.FindElementByName("Fortuna Login");
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
            LoginAccount(adminUsername, adminPassword);
        }

        public void CheckForLoginDeniedWarning()
        {
            IWebElement loginDeniedWarning = appSession.FindElementByName("Login denied for this Fortuna session");
            Assert.IsNotNull(loginDeniedWarning);
            Thread.Sleep(timeToSleep);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);
        }
    }
}

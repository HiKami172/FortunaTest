using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using FortunaTest.Dialogs;

namespace FortunaTest.Test
{
    class AccountTests
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private LoginDialog loginDialog;
        private readonly Actions enterBtn;

        public readonly string username = "username";
        public readonly string password = "password";

        private RemoteWebDriver appSession { get; set; }

        public AccountTests(RemoteWebDriver appSession)
        {
            this.appSession = appSession;
            loginDialog = new LoginDialog(appSession);

            enterBtn = new Actions(appSession).SendKeys(Keys.Enter);
        }

        public void LoginAccount(string username, string password)
        {
            loginDialog.LoginAccount(username, password);
        }

        public void LoginAdminAccount()
        {
            loginDialog.LoginAdminAccount();

            IWebElement adminDialogue = appSession.FindElementByName("Fortuna Administrator");
            Assert.IsNotNull(adminDialogue);
        }

        public void AddNewUser()
        {
            IWebElement adminDialogue = appSession.FindElementByName("Fortuna Administrator");
            ReadOnlyCollection<IWebElement> adminDialogueOptions = adminDialogue.FindElements(By.XPath(".//*"));
            IWebElement addUserBtn = adminDialogueOptions[4];

            addUserBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement adminAddUserDialogue = appSession.FindElementByName("Fortuna Administrator: Add User");
            ReadOnlyCollection<IWebElement> adminAddUserDialogueFields = adminAddUserDialogue.FindElements(By.XPath(".//Edit"));
            ReadOnlyCollection<IWebElement> adminAddUserDialogueBtns = adminAddUserDialogue.FindElements(By.XPath(".//Button"));
            IWebElement newUsernameField = adminAddUserDialogueFields[1];
            IWebElement newPasswordField = adminAddUserDialogueFields[0];
            IWebElement submitBtn = adminAddUserDialogueBtns[^1];
            IWebElement guillocheGeneratorBtn = adminAddUserDialogueBtns[33];

            newUsernameField.SendKeys(username);
            Thread.Sleep(timeToSleep);

            newPasswordField.SendKeys(password);
            Thread.Sleep(timeToSleep);

            guillocheGeneratorBtn.Click();
            Thread.Sleep(timeToSleep);

            submitBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement userAddedInfo = appSession.FindElementByName("User " + username + " added");
            Assert.IsNotNull(userAddedInfo);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminAddUserDialogue = new Actions(appSession)
                .MoveToElement(adminAddUserDialogue, 285, 5)
                .Click();
            closeAdminAddUserDialogue.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(appSession)
                .MoveToElement(adminDialogue, 225, 5)
                .Click();
            closeAdminDialogue.Perform();
            Thread.Sleep(timeToSleep);
        }

        public void RemoveUser()
        {
            LoginAdminAccount();

            IWebElement adminDialogue = appSession.FindElementByName("Fortuna Administrator");
            ReadOnlyCollection<IWebElement> adminDialogueOptions = adminDialogue.FindElements(By.XPath(".//*"));
            IWebElement removeUserBtn = adminDialogueOptions[3];

            removeUserBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement adminRemoveUserDialogue = appSession.FindElementByName("Fortuna Administrator: Remove User");
            IWebElement usernameField = adminRemoveUserDialogue.FindElement(By.XPath(".//Edit"));
            ReadOnlyCollection<IWebElement> adminRemoveUserDialogueBtns = adminRemoveUserDialogue.FindElements(By.XPath(".//Button"));
            IWebElement submitBtn = adminRemoveUserDialogueBtns[1];

            usernameField.SendKeys(username);
            Thread.Sleep(timeToSleep);

            submitBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement userRemovedInfo = appSession.FindElementByName("User " + username + " removed");
            Assert.IsNotNull(userRemovedInfo);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminRemoveUserDialogue = new Actions(appSession)
                .MoveToElement(adminRemoveUserDialogue, 280, 5)
                .Click();
            closeAdminRemoveUserDialogue.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(appSession)
                .MoveToElement(adminDialogue, 225, 5)
                .Click();
            closeAdminDialogue.Perform();
            Thread.Sleep(timeToSleep);

            LoginAccount(username, password);

            IWebElement loginDeniedWarning = appSession.FindElementByName("Login denied for this Fortuna session");
            Assert.IsNotNull(loginDeniedWarning);

            enterBtn.Perform();
        }

        public void LoginWithWrongCredentials()
        {
            string wrongPassword = ".";
            string wrongUsername = ".";
            if (username == ".")
                wrongUsername = ",";
            if (password == ".")
                wrongPassword = ",";

            loginDialog.LoginAccount(username, wrongPassword);
            loginDialog.CheckForLoginDeniedWarning();

            loginDialog.LoginAccount(wrongUsername, password);
            loginDialog.CheckForLoginDeniedWarning();

            loginDialog.LoginAccount(wrongUsername, wrongPassword);
            loginDialog.CheckForLoginDeniedWarning();
        }

        public void CheckUserCapabilities()
        {
            IWebElement applicationPane = appSession.FindElementByName("Application");

            Actions variableLineWidthBtn = new Actions(appSession)
                .MoveToElement(applicationPane, 20, 535)
                .Click();
            variableLineWidthBtn.Perform();
            Thread.Sleep(timeToSleep);

            IWebElement variableLineWidthDialogue = appSession.FindElementByName("Variable Line Width");
            Assert.IsNotNull(variableLineWidthDialogue);
        }

        public void LogoutAccount()
        {
            IWebElement applicationPane = appSession.FindElementByName("Application");

            loginDialog.ClickLoginBtn();

            Actions variableLineWidthBtn = new Actions(appSession)
                .MoveToElement(applicationPane, 20, 535)
                .Click();
            variableLineWidthBtn.Perform();
            Thread.Sleep(timeToSleep);

            try
            {
                appSession.FindElementByName("Variable Line Width");
                Assert.Fail();
            }
            catch { }
        }
    }
}

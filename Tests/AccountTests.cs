using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using FortunaTest.Dialogs;

namespace FortunaTest.Tests
{
    class AccountTests
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private readonly LoginDialog loginDialog;
        private readonly Actions enterBtn;

        public readonly string username = "username";
        public readonly string password = "password";

        private RemoteWebDriver AppSession { get; set; }

        public AccountTests(RemoteWebDriver appSession)
        {
            AppSession = appSession;
            loginDialog = new LoginDialog(AppSession);

            enterBtn = new Actions(AppSession).SendKeys(Keys.Enter);
        }

        public void LoginAccount(string username, string password)
        {
            loginDialog.LoginAccount(username, password);
        }

        public void LoginAdminAccount()
        {
            loginDialog.LoginAdminAccount();

            IWebElement adminDialogue = AppSession.FindElementByName("Fortuna Administrator");
            Assert.IsNotNull(adminDialogue);
        }

        public void AddNewUser()
        {
            IWebElement adminDialogue = AppSession.FindElementByName("Fortuna Administrator");
            ReadOnlyCollection<IWebElement> adminDialogueOptions = adminDialogue.FindElements(By.XPath(".//*"));
            IWebElement addUserBtn = adminDialogueOptions[4];

            addUserBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement adminAddUserDialogue = AppSession.FindElementByName("Fortuna Administrator: Add User");
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

            IWebElement userAddedInfo = AppSession.FindElementByName("User " + username + " added");
            Assert.IsNotNull(userAddedInfo);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminAddUserDialogue = new Actions(AppSession)
                .MoveToElement(adminAddUserDialogue, 285, 5)
                .Click();
            closeAdminAddUserDialogue.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(AppSession)
                .MoveToElement(adminDialogue, 225, 5)
                .Click();
            closeAdminDialogue.Perform();
            Thread.Sleep(timeToSleep);
        }

        public void RemoveUser()
        {
            LoginAdminAccount();

            IWebElement adminDialogue = AppSession.FindElementByName("Fortuna Administrator");
            ReadOnlyCollection<IWebElement> adminDialogueOptions = adminDialogue.FindElements(By.XPath(".//*"));
            IWebElement removeUserBtn = adminDialogueOptions[3];

            removeUserBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement adminRemoveUserDialogue = AppSession.FindElementByName("Fortuna Administrator: Remove User");
            IWebElement usernameField = adminRemoveUserDialogue.FindElement(By.XPath(".//Edit"));
            ReadOnlyCollection<IWebElement> adminRemoveUserDialogueBtns = adminRemoveUserDialogue.FindElements(By.XPath(".//Button"));
            IWebElement submitBtn = adminRemoveUserDialogueBtns[1];

            usernameField.SendKeys(username);
            Thread.Sleep(timeToSleep);

            submitBtn.Click();
            Thread.Sleep(timeToSleep);

            IWebElement userRemovedInfo = AppSession.FindElementByName("User " + username + " removed");
            Assert.IsNotNull(userRemovedInfo);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminRemoveUserDialogue = new Actions(AppSession)
                .MoveToElement(adminRemoveUserDialogue, 280, 5)
                .Click();
            closeAdminRemoveUserDialogue.Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(AppSession)
                .MoveToElement(adminDialogue, 225, 5)
                .Click();
            closeAdminDialogue.Perform();
            Thread.Sleep(timeToSleep);

            LoginAccount(username, password);

            IWebElement loginDeniedWarning = AppSession.FindElementByName("Login denied for this Fortuna session");
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
            IWebElement applicationPane = AppSession.FindElementByName("Application");

            Actions variableLineWidthBtn = new Actions(AppSession)
                .MoveToElement(applicationPane, 20, 535)
                .Click();
            variableLineWidthBtn.Perform();
            Thread.Sleep(timeToSleep);

            IWebElement variableLineWidthDialogue = AppSession.FindElementByName("Variable Line Width");
            Assert.IsNotNull(variableLineWidthDialogue);
        }

        public void LogoutAccount()
        {
            IWebElement applicationPane = AppSession.FindElementByName("Application");

            loginDialog.ClickLoginBtn();

            Actions variableLineWidthBtn = new Actions(AppSession)
                .MoveToElement(applicationPane, 20, 535)
                .Click();
            variableLineWidthBtn.Perform();
            Thread.Sleep(timeToSleep);

            try
            {
                AppSession.FindElementByName("Variable Line Width");
                Assert.Fail();
            }
            catch (WebDriverException) { }
        }
    }
}

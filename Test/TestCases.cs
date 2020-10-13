using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;

namespace FortunaTest.Test
{
    class TestCases
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private readonly Actions enterBtn;
        private readonly string adminUsername;
        private readonly string adminPassword;
        private readonly string username = "username";
        private readonly string password = "password"; // != "1"

        private RemoteWebDriver appSession { get; set; }

        public TestCases(RemoteWebDriver appSession)
        {
            this.appSession = appSession;

            enterBtn = new Actions(appSession);
            enterBtn
                .SendKeys(Keys.Enter);
            string adminInfoFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\")) + "adminInfo.txt";
            string adminInfo = File.ReadAllText(adminInfoFilePath);
            string[] splittedInfo = adminInfo.Split('\n');
            adminUsername = splittedInfo[0];
            adminPassword = splittedInfo[1];
        }

        public void CreateFile()
        {
            IWebElement AppMenuBar = appSession.FindElementByName("Application");
            ReadOnlyCollection<IWebElement> AppMenyBarOptions = AppMenuBar.FindElements(By.XPath(".//*"));
            AppMenyBarOptions[1].Click();
            Thread.Sleep(timeToSleep);

            //"New file" btn click
            Actions PointerActions = new Actions(appSession);
            PointerActions
                .MoveByOffset(0, 20)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            IWebElement SetUpDialogue = appSession.FindElementByName("Document Set-up");
            ReadOnlyCollection<IWebElement> SetUpDialogueBtns = SetUpDialogue.FindElements(By.XPath(".//Button"));
            SetUpDialogueBtns[1].Click();
            Thread.Sleep(timeToSleep);

            IWebElement NewFileWorkspace = appSession.FindElementByName("untitled");
            Assert.IsNotNull(NewFileWorkspace);
        }

        public void AdminAccLogin()
        {
            AccountLogin(adminUsername, adminPassword);
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

            Actions closeAdminAddUserDialogue = new Actions(appSession);
            closeAdminAddUserDialogue
                .MoveToElement(adminAddUserDialogue, 285, 5)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(appSession);
            closeAdminDialogue
                .MoveToElement(adminDialogue, 225, 5)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);
        }

        public void AccountLogout()
        {
            IWebElement applicationPane = appSession.FindElementByName("Application");

            Actions clickLoginBtn = new Actions(appSession);
            clickLoginBtn
                .MoveToElement(applicationPane, 20, 510)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            Actions variableLineWidthBtn = new Actions(appSession);
            variableLineWidthBtn
                .MoveToElement(applicationPane, 20, 535)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            try
            {
                appSession.FindElementByName("Variable Line Width");
                Assert.Fail();
            }
            catch { }
        }

        public void LoginWithWrongPassword()
        {
            string wrongPassword = "1";
            AccountLogin(username, wrongPassword);

            IWebElement loginDeniedWarning = appSession.FindElementByName("Login denied for this Fortuna session");
            Assert.IsNotNull(loginDeniedWarning);
            Thread.Sleep(timeToSleep);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);
        }

        public void RemoveUser()
        {
            AdminAccLogin();

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

            Actions closeAdminRemoveUserDialogue = new Actions(appSession);
            closeAdminRemoveUserDialogue
                .MoveToElement(adminRemoveUserDialogue, 280, 5)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            Actions closeAdminDialogue = new Actions(appSession);
            closeAdminDialogue
                .MoveToElement(adminDialogue, 225, 5)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            AccountLogin(username, password);

            IWebElement loginDeniedWarning = appSession.FindElementByName("Login denied for this Fortuna session");
            Assert.IsNotNull(loginDeniedWarning);

            enterBtn.Perform();
        }

        public void CheckUserCapabilities()
        {
            IWebElement applicationPane = appSession.FindElementByName("Application");

            Actions variableLineWidthBtn = new Actions(appSession);
            variableLineWidthBtn
                .MoveToElement(applicationPane, 20, 535)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            IWebElement variableLineWidthDialogue = appSession.FindElementByName("Variable Line Width");
            Assert.IsNotNull(variableLineWidthDialogue);
        }

        public void AccountLogin(string username, string password)
        {
            IWebElement applicationPane = appSession.FindElementByName("Application");

            Actions clickLoginBtn = new Actions(appSession);
            clickLoginBtn
                .MoveToElement(applicationPane, 20, 510)
                .Click()
                .Perform();
            Thread.Sleep(timeToSleep);

            IWebElement loginDialogue = appSession.FindElementByName("Fortuna Login");
            ReadOnlyCollection<IWebElement> inputFields = loginDialogue.FindElements(By.XPath(".//Edit"));

            inputFields[1].SendKeys(username);
            Thread.Sleep(timeToSleep);

            inputFields[0].SendKeys(password);
            Thread.Sleep(timeToSleep);

            enterBtn.Perform();
            Thread.Sleep(timeToSleep);
        }
    }
}

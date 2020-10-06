using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace FortunaTest.Test
{
    class TestCases
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private RemoteWebDriver appSession { get; set; }

        public TestCases(RemoteWebDriver appSession)
        {
            this.appSession = appSession;
        }

        public void FileCreation()
        {
            IWebElement AppMenuBar = appSession.FindElementByName("Application");
            ReadOnlyCollection<IWebElement> AppMenyBarOptions = AppMenuBar.FindElements(By.XPath(".//*"));
            AppMenyBarOptions[1].Click();
            Thread.Sleep(timeToSleep);

            //"New file" btn click
            Actions PointerActions = new Actions(appSession);
            PointerActions.MoveByOffset(0, 20).Click();
            PointerActions.Perform();
            Thread.Sleep(timeToSleep);

            IWebElement SetUpDialogue = appSession.FindElementByName("Document Set-up");
            ReadOnlyCollection<IWebElement> SetUpDialogueOptions = SetUpDialogue.FindElements(By.XPath(".//*"));
            SetUpDialogueOptions[3].Click();
            Thread.Sleep(timeToSleep);

            IWebElement NewFileWorkspace = appSession.FindElementByName("untitled");
            Assert.IsNotNull(NewFileWorkspace);
            appSession.Close();
        }
    }
}

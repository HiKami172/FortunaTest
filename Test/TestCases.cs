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
        private TimeSpan TimeToSleep = TimeSpan.FromSeconds(2);
        private RemoteWebDriver AppSession { get; set; }

        public TestCases(RemoteWebDriver appSession)
        {
            AppSession = appSession;
        }
        public void FileCreation()
        {
            IWebElement AppMenuBar = AppSession.FindElementByName("Application");
            ReadOnlyCollection<IWebElement> AppMenyBarOptions = AppMenuBar.FindElements(By.XPath(".//*"));
            AppMenyBarOptions[1].Click();
            Thread.Sleep(TimeToSleep);

            Actions PointerActions = new Actions(AppSession);
            PointerActions.MoveByOffset(0, 20).Click();
            PointerActions.Perform();
            Thread.Sleep(TimeToSleep);

            IWebElement SetUpDialogue = AppSession.FindElementByName("Document Set-up");
            ReadOnlyCollection<IWebElement> SetUpDialogueOptions = SetUpDialogue.FindElements(By.XPath(".//*"));
            SetUpDialogueOptions[2].Click();
            Thread.Sleep(TimeToSleep);

            IWebElement NewFileWorkspace = AppSession.FindElementByName("untitled");
            Assert.IsNotNull(NewFileWorkspace);
            AppSession.Close();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using FortunaTest.MenuBar;

namespace FortunaTest.Tests
{
    class FileMenuBarTests
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private readonly FileMenuBar fileMenuBar;
        private RemoteWebDriver AppSession { get; set; }

        public FileMenuBarTests(RemoteWebDriver appSession)
        {
            AppSession = appSession;
            fileMenuBar = new FileMenuBar(AppSession);
        }

        public void CreateFile()
        {
            fileMenuBar.OpenFileMenu();
            Thread.Sleep(timeToSleep);

            fileMenuBar.NewFileBtnClick();
            Thread.Sleep(timeToSleep);

            IWebElement SetUpDialogue = AppSession.FindElementByName("Document Set-up");
            ReadOnlyCollection<IWebElement> SetUpDialogueBtns = SetUpDialogue.FindElements(By.XPath(".//Button"));
            SetUpDialogueBtns[1].Click();
            Thread.Sleep(timeToSleep);

            IWebElement NewFileWorkspace = AppSession.FindElementByName("untitled");
            Assert.IsNotNull(NewFileWorkspace);
        }
    }
}

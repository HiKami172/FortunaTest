using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using FortunaTest.MenuBar;

namespace FortunaTest.Test
{
    class FileMenuBarTests
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private FileMenuBar fileMenuBar;
        private RemoteWebDriver appSession { get; set; }

        public FileMenuBarTests(RemoteWebDriver appSession)
        {
            this.appSession = appSession;
            fileMenuBar = new FileMenuBar(appSession);
        }

        public void CreateFile()
        {
            fileMenuBar.OpenFileMenu();
            Thread.Sleep(timeToSleep);

            fileMenuBar.NewFileBtnClick();
            Thread.Sleep(timeToSleep);

            IWebElement SetUpDialogue = appSession.FindElementByName("Document Set-up");
            ReadOnlyCollection<IWebElement> SetUpDialogueBtns = SetUpDialogue.FindElements(By.XPath(".//Button"));
            SetUpDialogueBtns[1].Click();
            Thread.Sleep(timeToSleep);

            IWebElement NewFileWorkspace = appSession.FindElementByName("untitled");
            Assert.IsNotNull(NewFileWorkspace);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;

namespace FortunaTest.MenuBar
{
    class FileMenuBar
    {
        private readonly TimeSpan timeToSleep = TimeSpan.FromSeconds(2);
        private ReadOnlyCollection<IWebElement> appMenuBarOptions;
        private Actions newFileBtnClick;

        private RemoteWebDriver appSession { get; set; }

        public FileMenuBar(RemoteWebDriver appSession)
        {
            this.appSession = appSession;

            IWebElement appMenuBar = appSession.FindElementByName("Application");
            appMenuBarOptions = appMenuBar.FindElements(By.XPath(".//*"));

            newFileBtnClick = new Actions(appSession)
                .MoveToElement(appMenuBarOptions[1], 0, 20)
                .Click();
        }

        public void OpenFileMenu()
        {
            appMenuBarOptions[1].Click();
        }

        public void NewFileBtnClick()
        {
            newFileBtnClick.Perform();
        }
    }
}

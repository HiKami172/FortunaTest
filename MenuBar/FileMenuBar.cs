using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System.Collections.ObjectModel;

namespace FortunaTest.MenuBar
{
    class FileMenuBar
    {
        private readonly ReadOnlyCollection<IWebElement> appMenuBarOptions;
        private readonly Actions newFileBtnClick;

        public FileMenuBar(RemoteWebDriver appSession)
        { 
            IWebElement appMenuBar = appSession.FindElementByName("Application");
            appMenuBarOptions = appMenuBar.FindElements(By.XPath(".//*"));

            newFileBtnClick = new Actions(appSession)
                .MoveToElement(appMenuBarOptions[1], 5, 25)
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

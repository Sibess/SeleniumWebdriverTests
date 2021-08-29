using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SeleniumWebdriverTests
{
    [TestFixture]
    [Category("VERY CRITICAL TESTS")]
    public class Tests
    {
        //initializing driver, explicit wait, baseUrl variables for the whole class
        public static IWebDriver Driver = new ChromeDriver();
        public static WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
        string baseUrl = "http://webdriveruniversity.com";

        [SetUp]
        public void Initialize()
        {
            //maximizing browser window before every test
            Driver.Manage().Window.Maximize();
        }

        [Test]
        public void ShouldDisplayAlertWhenCredentialsAreInvalid()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Login-Portal/fail.html");
            //submitting credentials
            Driver.FindElement(By.Id("text")).SendKeys("Admin");
            Driver.FindElement(By.Id("password")).SendKeys("Admin");
            Driver.FindElement(By.Id("login-button")).Click();
            //verifying alert text
            Assert.IsTrue(Driver.SwitchTo().Alert().Text.Contains("validation failed"));
            Driver.SwitchTo().Alert().Accept();
        }

        [Test]
        public void ShouldExpandAccordion()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Accordion/index.html");
            //waiting for 'loading complete' message
            Wait.Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.Id("hidden-text")), "LOADING COMPLETE"));
            //expanding according, verifying appeared text and collapsing accordion
            Driver.FindElement(By.Id("click-accordion")).Click();
            Assert.IsTrue(Driver.FindElement(By.Id("timeout")).Text.Contains("This text has appeared after 5 seconds!"));
            Driver.FindElement(By.Id("click-accordion")).Click();
        }

        [Test]
        public void ShouldSelectValidOptions()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Dropdown-Checkboxes-RadioButtons/index.html");
            //selecting values in dropdowns from "Dropdown Menu(s)" box
            Driver.FindElement(By.Id("dropdowm-menu-1")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-1']/option[2]")).Click();
            Driver.FindElement(By.Id("dropdowm-menu-2")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-2']/option[2]")).Click();
            Driver.FindElement(By.Id("dropdowm-menu-3")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-3']/option[1]")).Click();
            //checking and unchecking checkboxes
            Driver.FindElement(By.XPath("//*[@id='checkboxes']/label[1]")).Click();
            Driver.FindElement(By.XPath("//*[@id='checkboxes']/label[3]")).Click();
            //selecting radio button value in "Radio Button(s)" box
            Driver.FindElement(By.XPath("//*[@id='radio-buttons']/input[3]")).Click();
            //asserting “Cabbage” radio button is disabled in "Selected & Disabled" box
            Assert.IsFalse(Driver.FindElement(By.XPath("//*[@id='radio-buttons-selected-disabled']/input[2]")).Enabled, "Cabbage radio button is enabled.");
            //expanding dropdown from "Selected & Disabled" box
            Driver.FindElement(By.Id("fruit-selects")).Click();
            //asserting “Orange” option is disabled in dropdown
            Assert.IsFalse(Driver.FindElement(By.XPath("//*[@id='fruit-selects']/option[2]")).Enabled, "Orange option is enabled.");
            //selecting "Apple" value in dropdown
            Driver.FindElement(By.XPath("//*[@id='fruit-selects']/option[1]")).Click();
        }

        [Test]
        public void ShouldShowValidPopupMessage()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Ajax-Loader/index.html");
            var greenButton = Driver.FindElement(By.XPath("//span[@id='button1']/p"));

            //waiting for loader to disappear and clicking on the big green button
            Wait.Until(ExpectedConditions.TextToBePresentInElement(greenButton, "CLICK ME!"));
            greenButton.Click();
            //waiting until popup pops up and verifying its text
            Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("close")));
            Assert.IsTrue(Driver.FindElement(By.XPath("//div[@id='myModalClick']/div/div/div/h4")).Text.Contains("Well Done For Waiting....!!!"));
            Driver.FindElement(By.XPath("//*[@id='myModalClick']/div/div/div[3]/button")).Click();
        }

        [Test]
        public void ShouldDragAndDropElement()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Actions/index.html");
            var draggableElement = Driver.FindElement(By.Id("draggable"));
            var droppableElement = Driver.FindElement(By.Id("droppable"));
            Actions action = new Actions(Driver);

            //dragging and dropping element
            action.ClickAndHold(draggableElement).MoveToElement(droppableElement).Release().Build().Perform();
            Assert.IsTrue(Driver.FindElement(By.Id("droppable")).Text.Contains("Dropped!"));
            //asserting dark red color of the "Dropped" box
            Assert.AreEqual("rgba(244, 89, 80, 1)", Driver.FindElement(By.XPath("//*[@id='droppable']/p")).GetCssValue("background-color"));
            //Hovering over "Hover Over Me Second!” label and clicking on the expanded link
            action.MoveToElement(Driver.FindElement(By.XPath("//div[@id='div-hover']/div[2]/button"))).Build().Perform();
            Driver.FindElement(By.XPath("//*[@id='div-hover']/div[2]/div/a")).Click();
            //Accepting alert and veriying its message
            Assert.IsTrue(Driver.SwitchTo().Alert().Text.Contains("Well done you clicked on the link!"));
            Driver.SwitchTo().Alert().Accept();
            //clicking and holding footer and verifying showed up message
            action.ClickAndHold(Driver.FindElement(By.Id("click-box"))).Build().Perform();
            Assert.IsTrue(Driver.FindElement(By.Id("click-box")).Text.Contains("Well done! keep holding that click now....."));
        }

        [Test]
        public void ShouldDisplayAlertsAndPopUp()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/Popup-Alerts/index.html");
            //clicking on the first button and accepting its alert
            Driver.FindElement(By.Id("button1")).Click();
            Driver.SwitchTo().Alert().Accept();
            //clicking on the second button, waiting for its popup and closing the popup
            Driver.FindElement(By.Id("button2")).Click();
            Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-footer")));
            Driver.FindElement(By.XPath("//div[@id='myModal']/div/div/div[3]/button")).Click();
            //clicking on the forth button and accepting its alert
            Driver.FindElement(By.Id("button4")).Click();
            Driver.SwitchTo().Alert().Accept();
            //clicking on the forth button and canceling its alert
            Driver.FindElement(By.Id("button4")).Click();
            Driver.SwitchTo().Alert().Dismiss();
            Assert.IsTrue(Driver.FindElement(By.Id("confirm-alert-text")).Text.Contains("You pressed Cancel!"));
        }

        [Test]
        public void ShouldDisplayValidTextInWhoAreWeSection()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/IFrame/index.html");
            //switching to "frame" iframe and verifying section text
            Driver.SwitchTo().Frame("frame");
            Assert.IsTrue(Driver.FindElement(By.XPath("//div[@class='col-sm-8 col-lg-8 col-md-8'][1]/div/div[2]/p")).Text.Contains("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi elit sapien, tempus sit amet hendrerit volutpat, euismod vitae risus. Etiam consequat, sem et vulputate dapibus, diam enim tristique est, vitae porta eros mauris ut orci. Praesent sed velit odio. Ut massa arcu, suscipit viverra molestie at, aliquet a metus. Nullam sit amet tellus dui, ut tincidunt justo. Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
            //clicking on the “WebdriverUniversity.com (IFrame)” button
            Driver.FindElement(By.Id("nav-title")).Click();
        }

        [Test]
        public void ShouldUploadFile()
        {
            //setting up file and file location
            var filename = "sample.jpg";
            var filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestData\UploadFileSamples\" + filename);

            Driver.Navigate().GoToUrl($"{baseUrl}/File-Upload/index.html");
            //uploading file
            Driver.FindElement(By.Id("myFile")).SendKeys(filePath);
            Driver.FindElement(By.Id("submit-button")).Click();
            //accepting alert
            Driver.SwitchTo().Alert().Accept();
        }

        [Test]
        public void ShouldAddAndRemoveItemsFromToDoList()
        {
            Driver.Navigate().GoToUrl($"{baseUrl}/To-Do-List/index.html");
            var inputField = Driver.FindElement(By.XPath("//*[@id='container']/input"));

            //adding 2 items
            inputField.SendKeys("New item 1");
            inputField.SendKeys(Keys.Enter);
            inputField.SendKeys("New item 2");
            inputField.SendKeys(Keys.Enter);
            //removing 5th item
            Driver.FindElement(By.XPath("//*[@id='container']/ul/li[5]/span/i")).Click();
            //waiting for 5th row to disappear
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id='container']/ul/li[5]")));
            //counting displayed items
            List<IWebElement> sortableListTwo = Driver.FindElements(By.XPath("//*[@id='container']/ul/li")).ToList();
            Assert.AreEqual(4, sortableListTwo.Count());
        }

        [OneTimeTearDown]
        public static void CloseBrowser()
        {
            //closing browser after all tests are finished
            Driver.Quit();
        }
    }
}

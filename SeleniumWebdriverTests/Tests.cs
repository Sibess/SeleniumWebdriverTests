using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace SeleniumWebdriverTests
{
    [TestFixture]
    public class Tests
    {
        public static IWebDriver Driver = new ChromeDriver();
        public static WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

        [SetUp]
        public void Initialize()
        {
            Driver.Manage().Window.Maximize();
        }

        [Test]
        public void ShoudShowValidAlertText()
        {
            Driver.Navigate().GoToUrl("http://webdriveruniversity.com/Login-Portal/fail.html");
            Driver.FindElement(By.Id("text")).SendKeys("Admin");
            Driver.FindElement(By.Id("password")).SendKeys("Admin");
            Driver.FindElement(By.Id("login-button")).Click();
            string alertTextElement = Driver.SwitchTo().Alert().Text;
            Assert.IsTrue(alertTextElement.Contains("validation failed"));
            Driver.SwitchTo().Alert().Accept();
        }

        [Test]
        public void ShoudShowValidTextInExpandedAccordion()
        {
          Driver.Navigate().GoToUrl("http://webdriveruniversity.com/Accordion/index.html");
          var labelElement = Driver.FindElement(By.Id("hidden-text"));
          Wait.Until(ExpectedConditions.TextToBePresentInElement(labelElement, "LOADING COMPLETE"));
          Driver.FindElement(By.Id("click-accordion")).Click();
          string accordionTextElement = Driver.FindElement(By.Id("timeout")).Text;
          Assert.IsTrue(accordionTextElement.Contains("This text has appeared after 5 seconds!"));
          Driver.FindElement(By.Id("click-accordion")).Click();
        }

        [Test]
        public void ShoudShowValidTextIngfhfExpandedAccordion()
        {
            Driver.Navigate().GoToUrl("http://webdriveruniversity.com/Dropdown-Checkboxes-RadioButtons/index.html");
            Driver.FindElement(By.Id("dropdowm-menu-1")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-1']/option[2]")).Click();
            Driver.FindElement(By.Id("dropdowm-menu-2")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-2']/option[2]")).Click();
            Driver.FindElement(By.Id("dropdowm-menu-3")).Click();
            Driver.FindElement(By.XPath("//*[@id='dropdowm-menu-3']/option[1]")).Click();
            Driver.FindElement(By.XPath("//*[@id='checkboxes']/label[1]")).Click();
            Driver.FindElement(By.XPath("//*[@id='checkboxes']/label[3]")).Click();
            Driver.FindElement(By.XPath("//*[@id='radio-buttons']/input[3]")).Click();
            Assert.IsFalse(Driver.FindElement(By.XPath("//*[@id='radio-buttons-selected-disabled']/input[2]")).Enabled, "Cabbage radio button is enabled.");
            Driver.FindElement(By.Id("fruit-selects")).Click();
            Assert.IsFalse(Driver.FindElement(By.XPath("//*[@id='fruit-selects']/option[2]")).Enabled, "Orange option is enabled.");
            Driver.FindElement(By.XPath("//*[@id='fruit-selects']/option[1]")).Click();
        }

        [OneTimeTearDown]
        public static void CloseBrowser()

        {
            Driver.Quit();
        }

    }
}
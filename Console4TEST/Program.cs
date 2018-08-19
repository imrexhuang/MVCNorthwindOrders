using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


// https://www.cnblogs.com/enigmaxp/p/5374855.html

namespace Console4TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.SetupTest();
            p.TheUntitledTest();
            p.TeardownTest();
        }

        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;


        [SetUp]
        public void SetupTest()
        {

            //----------------//
            // 設定瀏覽器選項 //
            //----------------//
            ChromeOptions chromeOptions = new ChromeOptions();

            // 移除 Chrome 正在被自動化軟體控制的黃色提示列
            chromeOptions.AddArgument("--disable-infobars");

            //driver = new ChromeDriver();

            String chromeDriverDirectory = $@"..\..\..\chromedriver2.40";

            // 指定 chromedriver.exe 所在目錄並啟動瀏覽器
            driver = new ChromeDriver(chromeDriverDirectory, chromeOptions);
            baseURL = "http://northwindorders.main.tw/Order/Edit?oid=10248&pid=72";
            verificationErrors = new StringBuilder();
        }


        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void TheUntitledTest()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.FindElement(By.Id("UnitPrice")).Clear();
            driver.FindElement(By.Id("UnitPrice")).SendKeys("35.86");
            driver.FindElement(By.Id("Quantity")).Clear();
            driver.FindElement(By.Id("Quantity")).SendKeys("7");
            driver.FindElement(By.TagName("form")).Submit();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until<bool>((d) =>
            {
                try
                {
                    IWebElement element = d.FindElement(By.Id(""));
                    Assert.AreEqual(element.FindElement(By.ClassName("")).Text, "");
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

    }
}

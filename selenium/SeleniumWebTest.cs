 
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;


namespace selenium
{
    public class SeleniumWebTest
    { 
        IWebDriver _driver;

        private readonly SeleniumHelper _seleniumHelper = new SeleniumHelper();

        [Test]
        public void CreateTest()
        {
            //Startup not needed

            //Test
            _driver = new ChromeDriver(System.Environment.CurrentDirectory);
            _driver.Url = "http://localhost:5050/";
            _driver.FindElement(By.XPath("//button[@type='button'][contains(.,'Accept')]")).Click();
            _driver.FindElement(By.XPath("//a[contains(.,'Employee Records')]")).Click();
            _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/Create'][contains(.,'Create New')]")).Click();
            _driver.FindElement(By.XPath("//input[contains(@id,'firstName')]")).SendKeys("999");
            _driver.FindElement(By.XPath("//input[contains(@id,'lastName')]")).SendKeys("999");
            _driver.FindElement(By.XPath("//input[contains(@id,'email')]")).SendKeys("9999999999@ascensus.com");
            _driver.FindElement(By.XPath("//input[contains(@class,'btn btn-default')]")).Click();
            _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/employeeView/9999999999@ascensus.com?first=999&last=999'][contains(.,'View')]")).Click();
            System.Threading.Thread.Sleep(2000);
            _driver.Close();

            //Cleanup
            _seleniumHelper.DeleteEmployee("9999999999@ascensus.com");
        }

        [Test]
        public void UpdateTest()
        {
            //Startup
            _seleniumHelper.CreateEmployee("9999999999@ascensus.com", "999", "999");

            //Test
            _driver = new ChromeDriver(System.Environment.CurrentDirectory);
            _driver.Url = "http://localhost:5050/";
            _driver.FindElement(By.XPath("//button[@type='button'][contains(.,'Accept')]")).Click();
            _driver.FindElement(By.XPath("//a[contains(.,'Employee Records')]")).Click();
            _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/Edit/9999999999@ascensus.com?first=999&last=999'][contains(.,'Edit')]")).Click();
            _driver.FindElement(By.XPath("//input[contains(@id,'firstName')]")).Clear();
            _driver.FindElement(By.XPath("//input[contains(@id,'lastName')]")).Clear();
            _driver.FindElement(By.XPath("//input[contains(@id,'firstName')]")).SendKeys("888");
            _driver.FindElement(By.XPath("//input[contains(@id,'lastName')]")).SendKeys("888");
            _driver.FindElement(By.XPath("//input[contains(@class,'btn btn-default')]")).Click();
            _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/employeeView/9999999999@ascensus.com?first=888&last=888'][contains(.,'View')]")).Click();
            System.Threading.Thread.Sleep(2000);
            _driver.Close();

            //Cleanup
            _seleniumHelper.DeleteEmployee("9999999999@ascensus.com");
        }

        [Test]
        public void DeleteTest()
        {
            //Startup
            _seleniumHelper.CreateEmployee("9999999999@ascensus.com", "999", "999");

            //Test
            _driver = new ChromeDriver(System.Environment.CurrentDirectory);
            _driver.Url = "http://localhost:5050/";
            _driver.FindElement(By.XPath("//button[@type='button'][contains(.,'Accept')]")).Click();
            _driver.FindElement(By.XPath("//a[contains(.,'Employee Records')]")).Click();
            _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/Delete/9999999999@ascensus.com'][contains(.,'Delete')]")).Click();
            bool found = true;
            try
            {
                _driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/employeeView/9999999999@ascensus.com?first=999&last=999'][contains(.,'View')]"));
            }
            catch
            {
                found = false;
            }
            Assert.AreEqual(false, found);
            System.Threading.Thread.Sleep(2000);
            _driver.Close();

            //Cleanup not needed
        }

    }


}
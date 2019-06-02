using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace selenium
{
    public class SeleniumHelper
    {
        // Add user to csv file
        public void CreateEmployee(string emailSent, string fName, string lName)
        {
            EmployeeTestModel person = new EmployeeTestModel()
            {
                email = emailSent,
                firstName = fName,
                lastName = lName,
            };

            var clientGet = new RestClient("http://localhost:5000/api/values/getEmployees");
            var requestGet = new RestRequest(Method.GET);
            IRestResponse response = clientGet.Execute(requestGet);
            List<EmployeeTestModel> employeeList = JsonConvert.DeserializeObject<List<EmployeeTestModel>>(response.Content);
            bool found = false;
            foreach (var employee in employeeList)
            {
                if (employee.email == person.email)
                {
                    found = true;
                }
            }

            if (!found)
            {
                var client = new RestClient("http://localhost:5000/api/values/createEmployee");
                var request = new RestRequest(Method.PUT);
                request.AddJsonBody(person);
                client.Execute(request);
            }
        }

        // Delete user from csv file, allows testing a specific part
        public void DeleteEmployee(string id)
        {
            var client = new RestClient("http://localhost:5000/api/values/deleteEmployee/" + id);
            var request = new RestRequest(Method.DELETE);
            client.Execute(request);
        }

        /*
       public void CreateEmployee()
       {
           driver.FindElement(By.XPath("//a[contains(.,'Employee Records')]")).Click();
           driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/Create'][contains(.,'Create New')]")).Click();
           driver.FindElement(By.XPath("//input[contains(@id,'firstName')]")).SendKeys("999");
           driver.FindElement(By.XPath("//input[contains(@id,'lastName')]")).SendKeys("999");
           driver.FindElement(By.XPath("//input[contains(@id,'email')]")).SendKeys("9999999999@ascensus.com");
           driver.FindElement(By.XPath("//input[contains(@class,'btn btn-default')]")).Click();
       }

       public void DeleteEmployee()
       {
           driver.FindElement(By.XPath("//a[contains(.,'Employee Records')]")).Click();
           driver.FindElement(By.XPath("//a[@href='/EmployeeRecords/Delete/9999999999@ascensus.com'][contains(.,'Delete')]")).Click();
       }

       */
    }
}

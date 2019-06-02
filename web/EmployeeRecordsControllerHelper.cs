using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using web.Models;

namespace web
{
    public class EmployeeRecordsControllerHelper : IEmployeeRecordsControllerHelper
    {
        public List<EmployeeModel> GetEmployees()
        {
            var client = new RestClient("http://localhost:5000/api/values/getEmployees");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return(JsonConvert.DeserializeObject<List<EmployeeModel>>(response.Content));
        }

        public EmployeeModel GetSpecificEmployee(string email)
        {
            var client = new RestClient("http://localhost:5000/api/values/getEmployee/" + email);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return (JsonConvert.DeserializeObject<EmployeeModel>(response.Content));
        }

        public void SetOrUpdateEmployee(EmployeeModel person, String operations)
        {
            RestClient client = new RestClient("http://localhost:5000/api/values/Bad");

            if (operations == "create")
            {
                client = new RestClient("http://localhost:5000/api/values/createEmployee");
            }
            else if (operations == "update")
            {
                client = new RestClient("http://localhost:5000/api/values/updateEmployee");
            }

            var request = new RestRequest(Method.PUT);
            request.AddJsonBody(person);
            client.Execute(request);
        }

        public void DeleteEmployee(string email)
        {
            var client = new RestClient("http://localhost:5000/api/values/deleteEmployee/" + email);
            var request = new RestRequest(Method.DELETE);
            client.Execute(request);
        }
    }
}
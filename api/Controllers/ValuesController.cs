using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        

        private readonly IFileHelper _fileHelper;

        public ValuesController(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/employee/fakeemail@domain.com
        [HttpGet("getEmployee/{id}")]
        public JsonResult Get(string id)
        {
            Contact desiredEmployee = new Contact();
            bool found = false;

            foreach (var employee in _fileHelper.GetData<Contact>("Book1"))
            {
                if (employee.email == id)
                {
                    desiredEmployee = employee;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return new JsonResult(desiredEmployee);
            }
            else
            {
                return new JsonResult("ID NOT FOUND");
            }
        }

        // GET api/values/employees
        [HttpGet("getEmployees")]
        //public ActionResult<List<Contact>> GetEmployees()
        public JsonResult GetEmployees()
        {
            return new JsonResult(_fileHelper.GetData<Contact>("Book1"));
        }

        // POST api/values
        [HttpPut("createEmployee")]
        public void CreateEmployee([FromBody] Contact person)
        {
            if(person.email != null)
            {
                var records = _fileHelper.GetData<Contact>("Book1");
                records.Add(person);
                _fileHelper.SetData<Contact>("Book1", records);

            }
        }

        // PUT api/values/updateEmployee
        [HttpPut("updateEmployee")]
        public void UpdateEmployee([FromBody] Contact person)
        {
            var records = new List<Contact>();
            foreach (var employee in _fileHelper.GetData<Contact>("Book1"))
            {
                records.Add(person.email == employee.email ? person : employee);
            }

            _fileHelper.SetData<Contact>("Book1", records);
        }

        // DELETE api/values/deleteEmployee/email@domain.com
        [HttpDelete("deleteEmployee/{id}")]
        public void DeleteEmployee(string id)
        {
            Contact desiredEmployee = new Contact();
            //bool found = false;
            var records = new List<Contact>();
            foreach (var employee in _fileHelper.GetData<Contact>("Book1"))
            {
                if (employee.email != id) records.Add(employee);
            }

            _fileHelper.SetData<Contact>("Book1", records);

        }
    }
}


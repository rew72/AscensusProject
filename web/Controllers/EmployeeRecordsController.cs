using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using web.Models;
using web.ViewModels;

namespace web.Controllers
{
    public class EmployeeRecordsController : Controller
    {
        private readonly IEmployeeRecordsControllerHelper _employeeRecordsControllerHelper;


        public EmployeeRecordsController(IEmployeeRecordsControllerHelper employeeRecordsControllerHelper)
        {
            _employeeRecordsControllerHelper = employeeRecordsControllerHelper;
        }

        // GET: EmployeeRecords
        public ActionResult Index()
        {
            List<EmployeeModel> employeeList = _employeeRecordsControllerHelper.GetEmployees();
            RecordViewModel modelPassIn = new RecordViewModel()
            {
                employees = employeeList,
                title = "Get your records"
            };
            return View(modelPassIn);
        }

        public ActionResult EmployeeView(string id)
        {
            EmployeeModel person = _employeeRecordsControllerHelper.GetSpecificEmployee(id);
            return View(person);
        }

        public ActionResult SalaryView(string id)
        {
            EmployeeModel person = _employeeRecordsControllerHelper.GetSpecificEmployee(id);
            return View(person);
        }

        // GET: EmployeeRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateSent(EmployeeModel test) // Add a builder pattern here
        // The build pattern would be passed to SetOrUpdate
        {
            List<EmployeeModel> employeeList = _employeeRecordsControllerHelper.GetEmployees();
            bool found = false;
            foreach (var employee in employeeList)
            {
                if (employee.email == test.email)
                {
                    found = true;
                }
            }

            if (!found)
            {
                _employeeRecordsControllerHelper.SetOrUpdateEmployee(test, "create");
            }
            return RedirectToAction("index");
        }

        // GET: EmployeeRecords/Edit/5
        public ActionResult Edit(string id)
        {
            EmployeeModel person = _employeeRecordsControllerHelper.GetSpecificEmployee(id);
            return View(person);
        }

        public ActionResult EditSent(EmployeeModel test)
        {
            _employeeRecordsControllerHelper.SetOrUpdateEmployee(test, "update");

            return RedirectToAction("index");
        }

        // GET: EmployeeRecords/Delete/5
        public ActionResult Delete(string id)
        {
            // Do the deletions
            _employeeRecordsControllerHelper.DeleteEmployee(id);
            return RedirectToAction("index");
        }
    }
}
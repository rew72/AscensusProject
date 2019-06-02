using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Models;

namespace web
{
    public interface IEmployeeRecordsControllerHelper
    {
        List<EmployeeModel> GetEmployees();
        EmployeeModel GetSpecificEmployee(string email);
        void SetOrUpdateEmployee(EmployeeModel person, String operations);
        void DeleteEmployee(string email);
    }
}

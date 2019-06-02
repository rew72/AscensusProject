using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Models;

namespace web.ViewModels
{
    /// <summary>
    /// Model for Contact Employee view
    /// </summary>
    public class RecordViewModel
    {
        public List<EmployeeModel> employees { get; set; }
        public string title { get; set; }
    }
}

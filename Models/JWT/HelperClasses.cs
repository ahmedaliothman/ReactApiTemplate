using Api.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models.JWT
{
    public class commonUserInfo
    {
        public int UserId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool ResidencyByMoa { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsSapUser { get; set; }
        public bool IsActive { get; set; }
        public string CivilId { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public string Sector { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Nationality { get; set; }
        public DateTime? BirthDate { get; set; }
        public string JobTitle { get; set; }
        public DateTime? HireDate { get; set; }
        public string Organization { get; set; }
        public int UserTypeId { get; set; }
        public int RegistrationStatusId { get; set; }

        public commonUserInfo()
        {


        }


    }


    public class ResponseRequest
    {
        public int Status { set; get; }

        public string Message { set; get; }

        public object Response { set; get; }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class LogInfoDTO : DTO
    {
        public LogInfoDTO()
        {
        }

        public string EmployeeId { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
        public string ticket { get; set; }
        public Int32? RoleId { get; set; }
        public string RoleName { get; set; }        
    }
}
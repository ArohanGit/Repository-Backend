
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class DepartmentDTO : DTO
    {
        public DepartmentDTO()
        {
        }

        public Int32? DepartmentID { get; set; }    //**PK
        public string Code { get; set; }
        public string Name { get; set; }
    }
}

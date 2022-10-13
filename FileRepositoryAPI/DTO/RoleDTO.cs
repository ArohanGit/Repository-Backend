
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class RoleDTO : DTO
    {

        public RoleDTO()
        {
        }
        public Int32? RoleID { get; set; }    //**PK

        public string Name { get; set; }

        public string ShowGSTChallan { get; set; }

        public string OnApprovalSaveChallan { get; set; }


        //Child Class Properties if any.


        //Use following code in lib.js
        //var Role = function Role() {
        //this.RoleID = null;

        //this.Name = null;        
        //}

    }
}

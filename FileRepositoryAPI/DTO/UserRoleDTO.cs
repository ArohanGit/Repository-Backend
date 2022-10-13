
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class UserRoleDTO : DTO
    {

        public UserRoleDTO()
        {
        }
        public Int32? UserRoleID { get; set; }    //**PK

        public Int32? UserID { get; set; }
        public Int32? RoleID { get; set; }
        

        //Child Class Properties if any.
        

        //Use following code in lib.js
        //var UserRole = function UserRole() {
        //this.UserRoleID = null;
        
        //this.UserID = null;        
        //this.RoleID = null;        
        //}

    }
}

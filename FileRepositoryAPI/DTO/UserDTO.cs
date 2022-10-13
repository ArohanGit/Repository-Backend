
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class UserDTO : DTO
    {
        public UserDTO()
        {
        }
        public Int32? UserID { get; set; }    //**PK

        public string Code { get; set; }
        public string Name { get; set; }
        public string IsLeft { get; set; }
        public string EMailAddress { get; set; }
        public string WebUserID { get; set; }
        public string password { get; set; }

        public Int32? RoleID { get; set; }
        public string RoleName { get; set; }

        //Child Class Properties if any.
        //Use following code in lib.js
        //var User = function User() {
        //this.UserID = null;

        //this.Code = null;        
        //this.Name = null;        
        //this.IsLeft = null;        
        //this.EMailAddress = null;        
        //this.WebUserID = null;        
        //this.password = null;        
        //}
    }
}

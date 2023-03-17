using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class NotificationToDTO : DTO
    {
        public NotificationToDTO()
        {
        }

        public string EmployeeId { get; set; }
        public Int32? NotificationToID { get; set; }
        public Int32? RepositoryID { get; set; }
        public string Email { get; set; }
        public string WebUserID { get; set; }
        public Int32? ApproverLevel { get; set; }
        public string AllowUpload { get; set; }
        public DateTime? ApprovedOn { get; set; }

        public string UserName { get; set; }
    }
}
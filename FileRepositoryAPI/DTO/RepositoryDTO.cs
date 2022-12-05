using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class RepositoryDTO : DTO
    {
        public RepositoryDTO()
        {
        }

        public Int32? RepositoryID { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryDescr { get; set; }
        public Int32? version { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string IsDelete { get; set; }
        public Int32? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int32? UpdtedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Int32? NotificationDays { get; set; }
        public string IsApproved { get; set; }
        public Int32? ApprovalLevel { get; set; }
        public string RejectRemark { get; set; }
        public Int32? NoOfLevel { get; set; }
        public Int32? DepartmentID { get; set; }        
    }
}


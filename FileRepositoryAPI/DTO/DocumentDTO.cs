using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class DocumentDTO : DTO
    {
        public DocumentDTO()
        {
        }
        
        public Int32? DocumentID { get; set; }    //**PK        
        public string FileName { get; set; }
        public string FileDescr { get; set;  }
        public Int32? version { get;  set; } 
        public string Extension { get; set; } 
        public string FilePath { get; set; }
        public Int32? FileSrl { get;  set; } 
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string IsDelete { get; set; }
        public Int32? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int32? UpdtedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Int32? NotificationDays { get; set; }
        public string RefTableID { get; set; }
        public string NotificationToUserIDs { get; set; }
    }
}


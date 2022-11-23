using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileRepositoryAPI.WebAPI
{
    public class FilesDTO : DTO
    {
        public FilesDTO()
        {
        }

        public Int32? FilesID { get; set; }
        public Int32? RepositoryID { get; set; }
        public string FileName { get; set; }
        public string FileDescr { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
        public Int32? FileSrl { get; set; }
        public string IsDelete { get; set; }
        public Int32? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int32? UpdtedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }   
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace FileRepositoryAPI.WebAPI
{
    public class DTO
    {
        public string Href { get; set; }

        private int _pageNo = 0;
        public int PageNo { get { return _pageNo; } set { _pageNo = value; } }

        private int _pageSize = 10;
        public int PageSize { get { return _pageSize; } set { _pageSize = value; } }

        public int TotalRows { get; set; }

        public bool IsMoreRecords { get { return (TotalRows > PageNo * PageSize); }}

        public string ScalarColumn { get; set; }

        public string ValidationRules { get; set; }
    }
}
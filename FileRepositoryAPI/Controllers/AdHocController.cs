using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FileRepositoryAPI.WebAPI
{
    [RoutePrefix("api/AdHoc")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [AppAuthenticationFilter]
    public class AdHocController : ApiController
    {     
    }

    public class JsonObject
    {
        public string action;
        public int key;
        public string keyColumn;
        public Object value;
    }

    public class SearchKeywordDTO
    {
        public string keyword;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Configuration;
using Arohan.Web.WebApi;
using Arohan.Utilities;
using System.DirectoryServices;

namespace FileRepositoryAPI.WebAPI
{
    public class AppAuthenticationFilter : BasicAuthenticationFilterAttribute // BasicAuthFilter //
    {

        public AppAuthenticationFilter()
        { }

        public AppAuthenticationFilter(bool active)
            : base(active)
        { }


        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            //return true;

            if (!base.OnAuthorizeUser(username, password, actionContext)) return false;

            //
            //if (username != password) return false;
            //
            // Overide above and implement application logic 
            // We will use Arohan.Utilities.Authentication
            // 

            string ticket = Authenticate.Login(username, password);
            if (string.IsNullOrEmpty(ticket))
                return false;

            return true;
        }
    }

    public class Authenticate
    {
        static string ldapConnectionString;
        static string ldapDomain;
        private string userName;
        private string password;
        static Authenticate()
        {
            ldapConnectionString = ConfigurationManager.AppSettings["LDAPConnectionString"];
            ldapDomain = ConfigurationManager.AppSettings["LDAPDomain"];
        }
        public Authenticate(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }
        public static string Login(string userName, string password)
        {
            return new Authenticate(userName, password).Login();
        }
        public string Login()
        {
            string token = null;
            Authentication authenticated = Authentication.ValidatedUser(ldapConnectionString, ldapDomain, "FileRepository.BusinessObjects.User", "IsValidUser", "GetRoles", this.userName, this.password);
            if (authenticated != null)
            {
                string credentials = String.Format("{0}:{1}", this.userName, this.password);
                token = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
            }
            return token;
        }
    }
}
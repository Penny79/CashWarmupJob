using CacheWarmUp.Model;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CacheWarmUp.Core
{
    /// <summary>
    /// Implements the basic calls ´to the rest api
    /// </summary>
    public class ApiClient
    {

        #region private fields

        private string baseUrl;
        private string username;
        private string password;

        private RestClient client;

        #endregion

        #region ctor

        public ApiClient(string baseurl, string username, string password)
        {
            this.baseUrl = baseurl;
            this.username = username;
            this.password = password;

            this.client = new RestClient(baseurl);
            client.CookieContainer = new System.Net.CookieContainer();
        }

      

        #endregion

        #region interface

        public void Logon()
        {
            var request = new RestRequest("gisa.de~lib~cum~service~rest~api~impl/customCumLogon", Method.POST);
            request.AddParameter("j_user", this.username);
            request.AddParameter("j_password", this.password);
            request.AddParameter("portal", "APPC");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            var response = client.Execute(request);


        }

        public IEnumerable<BusinessPartner> ListBusinessPartners()
        {
            var request = new RestRequest("appc-api/restricted/configuration/customer/businessPartners/selection", Method.GET);
        
            var response = client.Execute<RootObject>(request);

            return response.Data.selectable.ELECTRICITY.Union(response.Data.selectable.GAS);           
        }

        public void SetBusinessPartner(BusinessPartner partner)
        {
            var request = new RestRequest("appc-api/restricted/configuration/customer/businessPartners/selected", Method.POST);
           
            request.RequestFormat = DataFormat.Json;
            request.AddBody(partner);
            var response = client.Execute(request);
        }

        public Selected GetSelected()
        {
            var request = new RestRequest("appc-api/restricted/configuration/customer/businessPartners/selection", Method.GET);

            var response = client.Execute<RootObject>(request);

            return response.Data.selected;
        }

        public void LoadMasterData(BusinessPartner businessPartner)
        {
            
            var request = new RestRequest(String.Format("esales-api/restricted/application/{0}/config",businessPartner.commodity.ToLower()), Method.GET);

            var response = client.Execute(request);
           
        }

        #endregion
    }
}

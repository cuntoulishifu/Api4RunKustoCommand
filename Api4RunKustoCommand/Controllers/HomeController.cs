namespace Api4RunKustoCommand.Controllers
{
    using Kusto.Data;
    using Kusto.Data.Net.Client;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    public class HomeController : ApiController
    {


        internal string AuthenticateInteractiveUser(string authority, string clientId, string clientSecret)
        {
            var request = WebRequest
                .Create(new Uri("https://t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn"));

            var authContext = new AuthenticationContext(authority);
            var applicationCredentials = new ClientCredential(clientId, clientSecret);

            var result = authContext
                .AcquireTokenAsync("https://t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn", applicationCredentials)
                .GetAwaiter().GetResult();

            return result.AccessToken;
        }

        [HttpPost]
        public string InvokeKustoCommand([FromBody]InvokeInputObj invokeInputObj)//string endpoint, string authority, string command,string clientId,string clientSecret)
        {
            var db = "trident";

            //".create table MyLogs1 ( Level:string, Timestamp:datetime, UserId:string, TraceId:string, Message:string, ProcessId:int32 ) ";

            var kcsbEngine = new KustoConnectionStringBuilder(invokeInputObj.endpoint)
                             .WithAadApplicationTokenAuthentication(AuthenticateInteractiveUser(invokeInputObj.authority, invokeInputObj.clientId, invokeInputObj.clientSecret));
            using (var kustoAdminClient = KustoClientFactory.CreateCslAdminProvider(kcsbEngine))
            {
                var commands = Regex.Split(invokeInputObj.commands, ".create", RegexOptions.IgnoreCase);
                for (int i = 1; i < commands.Length; i++)
                {
                    var command = ".create " + commands[i];
                    kustoAdminClient.ExecuteControlCommand( db, command);
                }

            }
            return "the command is finished";
        }












        // GET: api/Home
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Home/5
        public string Get(int id)
        {
            return "value";
        }
        public string TestPost([FromBody]Person person)
        {
            return person.name;
        }
    }
    public class Person {
        public int age { get; set; }
        public string name { get; set; }
    }
    public class InvokeInputObj
    {
        public string endpoint { get; set; }
        public string authority { get; set; }
        public string commands { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}

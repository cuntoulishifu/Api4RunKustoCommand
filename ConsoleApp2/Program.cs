namespace ConsoleApp2
{
    using Kusto.Data;
    using Kusto.Data.Net.Client;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    class Program
    {

     
        static void Main(string[] args)
        {
            //string endpoint = "https://t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn";
            //string authority = "";
            //string command = "";  
            // InvokeKustoCommand();

          var  command = @".create table Apdex (Context:string, ServiceType:string, StartTime:datetime, EndTime:datetime, CumulativeDistributedFunction:string, HappinessDistribution:string)

.create table Percentile ( Context:string, LatencyType:string, StartTime:datetime, Percentile:string)";
          var pp=  Regex.Split(command, ".create", RegexOptions.IgnoreCase);
            for (int i = 1; i < pp.Length; i++)
            {

            }
                      var mm=  Regex.Matches(command, @".create \W");

            Console.WriteLine("your command is finished !");
        }
        internal static string AuthenticateInteractiveUser(string authority)
        {
            var request = WebRequest
                .Create(new Uri("https://t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn"));

            var authContext = new AuthenticationContext(authority);
            var applicationCredentials = new ClientCredential(clientId: "", clientSecret: "");

            var result = authContext
                .AcquireTokenAsync("https://t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn", applicationCredentials)
                .GetAwaiter().GetResult();

            return result.AccessToken;
        }
        internal static void InvokeKustoCommand(string endpoint,string authority,string command)
        {
            var db = "trident";
           
            //".create table MyLogs1 ( Level:string, Timestamp:datetime, UserId:string, TraceId:string, Message:string, ProcessId:int32 ) ";

            var kcsbEngine = new KustoConnectionStringBuilder(endpoint)
                             .WithAadApplicationTokenAuthentication(AuthenticateInteractiveUser(authority));
            using (var kustoAdminClient = KustoClientFactory.CreateCslAdminProvider(kcsbEngine))
            {


                kustoAdminClient.ExecuteControlCommand(databaseName: db, command: command);
            }
        }


        #region 没啥用的代码

        //static void insertData(string db, string table, string mappingName)
        //{
        //    var kcsbDM =
        //  new KustoConnectionStringBuilder($"https://{serviceNameAndRegion}.kusto.windows.net").WithAadUserPromptAuthentication(authority: $"{authority}");
        //    new KustoConnectionStringBuilder("https://ingest-t3kustochinaeast2.chinaeast2.kusto.chinacloudapi.cn").WithAadApplicationTokenAuthentication(CreateToken());

        //    using (var ingestClient = KustoIngestFactory.CreateQueuedIngestClient(kcsbDM))
        //    {
        //        var ingestProps = new KustoQueuedIngestionProperties(db, table);
        //        For the sake of getting both failure and success notifications we set this to IngestionReportLevel.FailuresAndSuccesses
        //        Usually the recommended level is IngestionReportLevel.FailuresOnly
        //       ingestProps.ReportLevel = IngestionReportLevel.FailuresAndSuccesses;
        //        ingestProps.ReportMethod = IngestionReportMethod.Queue;
        //        ingestProps.JSONMappingReference = mappingName;
        //        ingestProps.Format = DataSourceFormat.json;

        //        Prepare data for ingestion
        //        using (var memStream = new MemoryStream())
        //            using (var writer = new StreamWriter(memStream))
        //            {
        //                for (int counter = 1; counter <= 10; ++counter)
        //                {
        //                    writer.WriteLine(
        //                        "{{ \"Id\":\"{0}\", \"Timestamp\":\"{1}\", \"Message\":\"{2}\" }}",
        //                        counter, DateTime.UtcNow.AddSeconds(100 * counter),
        //                        $"This is a dummy message number {counter}");
        //                }

        //                writer.Flush();
        //                memStream.Seek(0, SeekOrigin.Begin);

        //                Post ingestion message
        //                ingestClient.IngestFromStream(memStream, ingestProps);
        //            }

        //        Wait and retrieve all notifications
        //         - Actual duration should be decided based on the effective Ingestion Batching Policy set on the table / database
        //        Thread.Sleep(11);
        //        var errors = ingestClient.GetAndDiscardTopIngestionFailures().GetAwaiter().GetResult();
        //        var successes = ingestClient.GetAndDiscardTopIngestionSuccesses().GetAwaiter().GetResult();

        //        errors.ForEach((f) => { Console.WriteLine($"Ingestion error: {f.Info.Details}"); });
        //        successes.ForEach((s) => { Console.WriteLine($"Ingested: {s.Info.IngestionSourcePath}"); });
        //    }
        //} 
        #endregion

     
    }
}

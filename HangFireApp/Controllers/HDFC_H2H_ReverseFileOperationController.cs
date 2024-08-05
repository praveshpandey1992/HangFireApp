using HangFireApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HangFireApp.Controllers
{
    public class HDFC_H2H_ReverseFileOperationController : Controller
    {
        private IConfiguration configuration;
        
        public HDFC_H2H_ReverseFileOperationController(IConfiguration iConfig)
        {
            configuration = iConfig;
        }

        public void GetReverseFileStatus()
        {
            //string path = @"D:/HDFC_Integration/ReverseFile/";
            string path = configuration.GetValue<string>("FilePath:reverseCsvFilePath");

            MonitorDirectory(path);
        }

        private static void MonitorDirectory(string path)
        {
            //var directoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "writereaddata")).Root;
            //var imagePath = Path.Combine(directoryPath, imageName);

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Path = path;

            fileSystemWatcher.Created += FileSystemWatcher_Created;

            fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created: {0}", e.FullPath);

            if (System.IO.File.Exists(e.FullPath) != null)
            {
                List<HDFC_H2H_ReverseFormatViewModel> values = System.IO.File.ReadAllLines(e.FullPath)
                                           .Select(v => HDFC_H2H_Operations.FromCsv(v))
                                           .ToList();
                if (values != null)
                {
                    // sent response to welfare scheme
                    using (var client = new HttpClient())
                    {
                        var welfareSchemeURL = "http://bocw.punjab.gov.in/Stg_wbapi/TradeUnions/SBIWelfare";
                        var myContent = JsonConvert.SerializeObject(values);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var result = client.PostAsync(welfareSchemeURL, byteContent).Result;

                        if(result.IsSuccessStatusCode == true)
                        {
                            updateLogs(Convert.ToString(myContent), "true", welfareSchemeURL);
                        }
                    }
                }
            }
        }

        private static void FileSystemWatcher_Renamed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File renamed: {0}", e.Name);
        }

        private static void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File deleted: {0}", e.Name);
        }


        public static void updateLogs(string myContent, string result, string welfareSchemeURL)
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-V8AO5CE;initial catalog=HDFC_H2H_Integration ; User ID=sa;Password=Admin@123;Integrated Security=SSPI;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("updateLogsOfHDFC_H2H_ReverseFile", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@RequestJson", SqlDbType.NVarChar).Value = myContent;
                    cmd.Parameters.Add("@RequestSentOn", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now);
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar).Value = result;
                    cmd.Parameters.Add("@ResponseReceivedOn", SqlDbType.DateTime).Value = Convert.ToDateTime(DateTime.Now);
                    cmd.Parameters.Add("@RequestURL", SqlDbType.NVarChar).Value = welfareSchemeURL;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Log the exception.
            }
        }
    }
}

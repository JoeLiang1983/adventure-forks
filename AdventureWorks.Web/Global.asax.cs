using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AdventureWorks.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            WriteFileInTheBlob();      

        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            ILog log = LogManager.GetLogger("SleepyCore");
            log.Error("Exception - \n" + ex);
        }

        protected void WriteFileInTheBlob()
        {
            //Create a simple text file HelloWorld file in blob storage each time when the web application starts
            string connstring = ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connstring);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer Container = client.GetContainerReference("module5container");
            CloudBlockBlob blob = Container.GetBlockBlobReference("Hello World");



            blob.UploadText(string.Format("Hello World!\n\rServer name: {0}\n\rTime: {1}", Environment.MachineName, DateTime.Now.ToString()));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace GildedRose.Web
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

      GlobalConfiguration.Configure(WebApiConfig.Register);

      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      // Get Global Configuration
      HttpConfiguration config = GlobalConfiguration.Configuration;

      // Handle Entity Framework objects that reference other objects
      config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling =
         Newtonsoft.Json.ReferenceLoopHandling.Ignore;

      // Convert to camelCase
      var jsonFormatter = config.Formatters
        .OfType<JsonMediaTypeFormatter>()
          .FirstOrDefault();
      jsonFormatter.SerializerSettings
        .ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
  }
}

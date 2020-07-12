using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace GraphFileSample
{
    class Program
    {
        static Dictionary<string, string> LoadClientSecretAppSettings()
        {
            Dictionary<string, string> result = null;
            // Get config ftom AppSettings
            var appConfig = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
            var appId = appConfig["appId"];
            var scopes = appConfig["scopes"];
            var tenantId = appConfig["tenantId"];
            var clientSecret = appConfig["clientSecret"];
            var siteId = appConfig["siteId"];
            if (string.IsNullOrEmpty(appId) == false &&
                string.IsNullOrEmpty(scopes) == false &&
                string.IsNullOrEmpty(tenantId) == false &&
                string.IsNullOrEmpty(clientSecret) == false &&
                string.IsNullOrEmpty(siteId) == false)
            {
                result = new Dictionary<string, string>()
                {
                    {"appId", appId},
                    {"scopes", scopes},
                    {"tenantId", tenantId},
                    {"clientSecret", clientSecret},
                    {"siteId", siteId}
                };
            }
            return result;
        }

        static void GetSiteDocuments(string siteId)
        {
            var site = GraphHelper.GetSiteAsync(siteId).Result;
            var items = GraphHelper.GetDriveRootChildrenAsync(site.Id).Result;
            foreach (var item in items)
            {
                Console.WriteLine($"Entity.Id: {item.Id}");
                Console.WriteLine($"BaseItem.File: {item.Name}");
                using (var itemContent =  GraphHelper.GetDriveContentAsync(site.Id, item.Id).Result)
                using (var fileStream = new FileStream(item.Name, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    itemContent.CopyTo(fileStream);
                }
            }
        }

        static void Main(string[] args)
        {
            IAuthenticationProvider authProvider = null;

            var appConfig = LoadClientSecretAppSettings();
            if (appConfig == null)
            {
                Console.WriteLine("Missing or invalid AppSettings");
                return;
            }
            var appId = appConfig["appId"];
            var scopesString = appConfig["scopes"];
            var scopes = scopesString.Split(';');
            var tenantId = appConfig["tenantId"];
            var clientSecret = appConfig["clientSecret"];
            var siteId = appConfig["siteId"];
            // Initialize the auth provider
            authProvider = new ClientSecretAuthProvider(appId, scopes, tenantId, clientSecret);
            // Initialize Graph client
            GraphHelper.Initialize(authProvider);
            // Get Files
            GetSiteDocuments(siteId);
        }
    }
}

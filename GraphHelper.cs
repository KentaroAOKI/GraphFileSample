using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GraphFileSample
{
    public class GraphHelper
    {
        private static GraphServiceClient graphClient;
        public static void Initialize(IAuthenticationProvider authProvider)
        {
            graphClient = new GraphServiceClient(authProvider);
        }

        public static async Task<Site> GetSiteAsync(string siteId)
        {
            // siteId format is "{hostname}:/{server-relative-path}"
            try
            {
                var site = await graphClient.Sites[siteId].Request().GetAsync();
                return site;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return null;
            }
        }

        public static async Task<IEnumerable<DriveItem>> GetDriveRootChildrenAsync(string siteObjId)
        {
            // siteObjId format is "xxxxxxxx,xxxxxxxxx,xxxxxxxxx"
            try
            {
                List<DriveItem> dirobjects = new List<DriveItem>();
                var resultPage = await graphClient.Sites[siteObjId].Drive.Root.Children.Request().GetAsync();
                while (true)
                {
                    dirobjects.AddRange(resultPage);
                    if (resultPage.NextPageRequest == null)
                    {
                        break;
                    }
                    resultPage = resultPage.NextPageRequest.GetAsync().Result;
                }
                return dirobjects;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return null;
            }
        }
        public static async Task<Stream> GetDriveContentAsync(string siteObjId, string itemId)
        {
            // siteObjId format is "xxxxxxxx,xxxxxxxxx,xxxxxxxxx"
            try
            {
                var result = await graphClient.Sites[siteObjId].Drive.Items[itemId].Content.Request().GetAsync();
                return result;
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting events: {ex.Message}");
                return null;
            }
        }
        
    }
}
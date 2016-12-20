using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace addUsertoGroups
{
    class utils
    {
        public static SPWeb RetrieveWeb(string siteUrl)
        {
                if (siteUrl.Trim() == "")
                {
                    Console.WriteLine("site URL is blank");
                    return null;
                }

                SPSite site;
                try
                {
                    site = new SPSite(siteUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("site not found: " + ex.Message);
                    return null;
                }

                SPWeb web = site.OpenWeb();
                Console.WriteLine("site found: " + web.Url );
                return web;
            }


    }// class utils
}

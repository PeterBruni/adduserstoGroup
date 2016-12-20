using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using System.Collections.Generic;


namespace addUsertoGroups
{
    class Program
    {


        static void populateAllUsersCombo(string TargetSite)
        {

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb web = utils.RetrieveWeb(TargetSite))
                {
                    if (web == null) return;

                    foreach (SPUser user in web.AllUsers)
                    {
                          Console.WriteLine(user);
                    }
                }
            });
        }


        static Dictionary<string,string> loadUserGroups(string TargetSite, string userLoginName)
        {
            Dictionary<string, string> sourceGroups = new Dictionary<string, string>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite siteCol = new SPSite(TargetSite))
                {
                        SPWeb web = siteCol.OpenWeb();
                        SPUser user = web.EnsureUser(userLoginName);
                        foreach (SPGroup group in user.Groups)
                        {
                            sourceGroups.Add(group.Name, group.Name);
                        } // foreach
                } // using
            });
            return sourceGroups;
        } // loadUserGroups



        static void AddUsertoGroup(string TargetSite, string userLoginName, string userGroupName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite siteCol = new SPSite(TargetSite))
                {
                        SPWeb web = siteCol.OpenWeb();
                        try
                        {
                            SPUser user = web.EnsureUser(userLoginName);
                            SPGroup spGroup = web.Groups[userGroupName];
                            if (spGroup != null)
                            {
                                Console.WriteLine("Add User: " + userLoginName + " Group: " + userGroupName);
                                spGroup.AddUser(user);

                            }
                        }
                        catch (Exception ex) {
                            Console.WriteLine(ex.Message);
                        }
                } // end using
            });
        } // AddUsertoGroup



        static void Main(string[] args)
        {
            string targetSite = args[0]; //  "http://dendevpop/";
            string sourceUser = args[1];   // source user
            string destUser = args[2];   // target user

            Dictionary<string, string> sourceGroups = new Dictionary<string, string>();
            Dictionary<string, string> DestinationGroups = new Dictionary<string, string>();

            sourceGroups = loadUserGroups(targetSite, sourceUser);
            DestinationGroups = loadUserGroups(targetSite, destUser);

            foreach (KeyValuePair<string, string> pair in sourceGroups)
            {
                if (! DestinationGroups.ContainsKey(pair.Key))
                {
                    AddUsertoGroup(targetSite, destUser, pair.Key);
                }
            }
            Console.Read();


/*
            string userLoginName = @"prologis-na\pbruni";
            Console.WriteLine(@"New SPSite:" + targetSite);
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite siteCol = new SPSite(targetSite))
                {
                    Console.WriteLine("Start Open Web");
                    SPWeb web = siteCol.OpenWeb();
                    Console.WriteLine("Complete Open Web");
                    SPUser user = web.EnsureUser(userLoginName);
                    foreach (SPGroup group in user.Groups)
                    {
                        Console.WriteLine(@"Group: " + group.Name);

                    }
                    Console.Read();
                }
            });
 */

        }// end main
    }
}

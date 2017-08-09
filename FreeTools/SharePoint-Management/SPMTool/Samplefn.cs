using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using Microsoft.SharePoint;
using System.Collections;
//using Microsoft.SharePoint.StsAdmin;
//using Microsoft.SharePoint.Administration;
//using System.Web.UI.WebControls.WebParts;
//using Microsoft.SharePoint.WebPartPages;
namespace SPMTool
{
    class Samplefn
    {
        /*public Hashtable GetListOfWebpartWithPage(SPWeb site)
        {
            int i = 0;
            Hashtable ht = new Hashtable();
            Hashtable ht1 = new Hashtable();
            SPFile file = null;
            foreach (SPList list in site.Lists)
            {
                foreach (SPListItem item in list.Items)
                {

                    //if (item[2].ToString().Equals("Document") )//&& item.Fields["File_x0020_Type"].Title.Contains("aspx") )
                    //{
                    //ht.Add("" + i, item.ToString());
                    if (item.ContentType != null && item.ContentType.Name.Equals("Document"))
                    {
                        file = item.File;
                        if (file != null && file.Exists && file.SourceLeafName.Contains("aspx"))
                        {
                            ht = GetWebPartsForFile(file);
                            ht1.Add("" + i, ht);
                            i++;
                            ht = null;
                            file = null;
                        }
                    }
                }
            }
            return ht1;
        }   
        public Hashtable GetAllLists(SPWeb site)
        {
            Hashtable ht = new Hashtable();
            int i = 0;
            SPListCollection listcollectiuon = site.Lists;
            foreach (SPList listitem in listcollectiuon)
            {
                ht.Add("" + i, listitem);
                i++;
            }
            return ht;
        }
        public Hashtable GetWebPartsForFile(SPFile file)
        {
            int i = 0;
            Hashtable ht = new Hashtable();
            SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
            SPLimitedWebPartCollection wc = wm.WebParts;
            //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);          
            foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
            {
                ht.Add("" + i, webpart);
                i++;
            }
            return ht;
        }
        public Hashtable GetFileCollection(SPSite site)
        {
            int i = 0;
            Hashtable ht = new Hashtable();
            SPFileCollection filecollection = null;
            SPWebCollection webcollection = null;
            String siteurl = site.Url;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                site = new SPSite(siteurl);
                webcollection = site.AllWebs;
                foreach (SPWeb web in webcollection)
                {
                    filecollection = web.Files;
                    ht.Add("" + i, filecollection);
                    i++;
                    web.Dispose();
                }
                site.Dispose();
            });
            return ht;
        }
        public SPListItemCollection GetWebPartGallery(SPSite site)
        {
            SPListItemCollection collListItems = null;
            SPList webpartlist = site.GetCatalog(SPListTemplateType.WebPartCatalog);
            if (webpartlist != null)
            {
                SPDocumentLibrary doclib = (SPDocumentLibrary)webpartlist;
                collListItems = doclib.Items;
            }
            return collListItems;
        }
        public SPWebApplication[] GetWebApplns(SPServer server)
        {
            SPWebApplication[] webapplns = null;
            int cnt = 0;

            foreach (SPServiceInstance serinst in server.ServiceInstances)
            {
                if (serinst.Service is SPWebService)
                {
                    SPWebService webservice = (SPWebService)serinst.Service;
                    webapplns = new SPWebApplication[webservice.WebApplications.Count];
                    foreach (SPWebApplication webappln in webservice.WebApplications)
                    {
                        if (webappln != null && webappln.IsAdministrationWebApplication != true)
                        {
                            webapplns[cnt] = webappln;
                            cnt++;
                        }
                    }
                    return webapplns;
                }
            }

            return webapplns;
        }
        public SPWebApplication GetWebAdminApplns(SPServer server)
        {
            SPWebApplication webapplnn = null;//to avoid error
            foreach (SPServiceInstance serinst in server.ServiceInstances)
            {
                if (serinst.Service is SPWebService)
                {
                    SPWebService webservice = (SPWebService)serinst.Service;
                    foreach (SPWebApplication webappln in webservice.WebApplications)
                    {
                        if (webappln.IsAdministrationWebApplication)
                        {
                            return webappln;
                        }
                    }

                }
            }
            return webapplnn;
        }
        public SPServer[] getServers()
        {
            SPServerCollection servercollection = SPFarm.Local.Servers;
            SPServer[] servers = new SPServer[servercollection.Count];
            int i = 0;
            try
            {

                foreach (SPServer server in servercollection)
                {
                    servers[i] = server;
                    i++;
                }

            }
            catch (Exception e)
            {
               Console.WriteLine( "Exception in getServers Method: " + e);
            }
            return servers;
        }   */    
    }
}

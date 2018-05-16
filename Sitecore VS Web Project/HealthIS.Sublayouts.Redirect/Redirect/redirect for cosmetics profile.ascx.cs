﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HealthIS.Apps;

namespace HealthIS.apps.SubLayouts
{
  
	/// <summary>
	/// Summary description for Redirect_for_cosmetics_profileSublayout
	/// </summary>
  public partial class Redirect_for_profileSublayout : System.Web.UI.UserControl 
	{

     
    private void Page_Load(object sender, EventArgs e) {
      // Put user code to initialize the page here
        
        Sitecore.Data.Items.Item MyItem = null;
        Sitecore.Web.UI.WebControls.Sublayout myLayout = ((Sitecore.Web.UI.WebControls.Sublayout)this.Parent);
        int statusCode = 302;
        bool handled = false;
        if (String.IsNullOrEmpty(myLayout.DataSource) || myLayout.DataSource.Equals("RedirectDataPersonIDNewDataSource"))
        {
            MyItem = HealthIS.Apps.Util.createDatasourceItem("User Defined/Sublayouts/RedirectDataPersonID", "RedirectDataPersonIDNewDataSource", "_RD");
            //Response.Write("<script type='text/javascript'>setTimeout(function(){location.reload();},3000);</script>");
        }
        else
            MyItem = Sitecore.Context.Database.GetItem(myLayout.DataSource);
        if (MyItem == null)
        {
            Response.Write("No DatasourceDefined");
            return;
        }

        if (Sitecore.Context.PageMode.IsPageEditorEditing)
        {
            loc.Item = MyItem;
            sc.Item = MyItem;
            QueryStringField.Item = MyItem;
            NewBaseLocation.Item = MyItem;
        }
        else
        {
            try
            {
                statusCode = System.Convert.ToInt16(MyItem.Fields["Status Code"].Value);
                string pid = Request.QueryString[MyItem.Fields["queryStringVariable"].Value.ToString()].ToString();
                if (pid != null && pid.Trim() != "")
                {
                    //statusCode = 302; 
                    HealthIS.Apps.Util.person p = HealthIS.Apps.Util.getFacultyBasicInfo(pid);
                    string fname = p.first_name.Trim().Replace(' ', '-');
                    string lname = p.last_name.Trim().Replace(' ', '-');

                    string urlEnding = MyItem.Fields["New Base Location"].Value.ToString() + pid.Trim() + "/" + fname + '-' + lname + ".aspx";
                    if (HealthIS.Apps.Util.isOnProduction())
                    {
                        Response.Redirect("http://health.usf.edu" +  urlEnding, false);

                    }
                    else
                    {
                        Response.Redirect("http://sitecoreqa2.hscnet.hsc.usf.edu"  + urlEnding, false);
                    }
                }
                else
                {

                    Response.Redirect(MyItem.Fields["New Default Location"].Value, false);
                }
                handled = true;
            }
            catch (Exception ex)
            {
                //Response.StatusCode = 404;
                //Response.End();
                //Response.Redirect("http://health.usf.edu/doctors/dermatology/cosmetics/providers");
                Response.Write("<div style='width:100%; height:500px; background:#000; color:#FFF'>There was an error with your request." + ex.Message + "<br/>" + ex.StackTrace + "</div>");
            }
            if (!handled)
            {
                statusCode = System.Convert.ToInt16(MyItem.Fields["Status Code"].Value);
                Response.Redirect(MyItem.Fields["New Default Location"].Value, false);
            }
            Response.StatusCode = statusCode;
            Response.End();
        }



/*        Sitecore.Data.Items.Item MyItem = null;
        Sitecore.Web.UI.WebControls.Sublayout myLayout = ((Sitecore.Web.UI.WebControls.Sublayout)this.Parent);



        if (String.IsNullOrEmpty(myLayout.DataSource) || myLayout.DataSource.Equals("SublayoutContentAreaNewDataSource"))
            MyItem = HealthIS.Apps.Util.createDatasourceItem("User Defined/Sublayouts/RedirectData", "SublayoutContentAreaNewDataSource", "_RD");
         else
             MyItem = Sitecore.Context.Database.GetItem(myLayout.DataSource);
         if (MyItem == null){
            Response.Write(myLayout.DataSource+"Had problems getting settings object");
             return;
         }

         if (Sitecore.Context.PageMode.IsPageEditorEditing)
             showPageEditorView(MyItem);
         else
             showPageNormalView(MyItem);*/

    }
   

/*    private void showPageNormalView(Sitecore.Data.Items.Item MyItem)
    {
        try
        {
            string pid = Request.QueryString["person_id"];
            if (pid != null && pid.Trim() != "")
            {
                Response.StatusCode = 302;

                HealthIS.Apps.Util.person p = HealthIS.Apps.Util.getFacultyBasicInfo(pid);
                string fname = p.first_name.Trim().Replace(' ', '-');
                string lname = p.last_name.Trim().Replace(' ', '-');

                string urlEnding = pid.Trim() + "/" + fname + '-' + lname + ".aspx";
                if (false)
                {
                    Response.Redirect("http://health.usf.edu/doctors/dermatology/cosmetics/" + urlEnding);
                }
                else
                {
                    Response.Redirect("http://sitecoreqa2.hscnet.hsc.usf.edu/doctors/dermatology/cosmetics/" + urlEnding);
                }
            }
            else
            {
                Response.StatusCode = System.Convert.ToInt16(MyItem.Fields["Status Code"].Value);
                Response.Redirect(MyItem.Fields["Location"].Value);
                //Response.StatusCode = 404;
                //Response.Redirect("~/doctors/dermatology/cosmetics");
            }
        }
        catch (Exception ex)
        {
            //Response.StatusCode = 404;
            //Response.Redirect("http://health.usf.edu/doctors/dermatology/cosmetics/providers");
            Response.Write("<div style='width:100%; height:500px; background:#000; color:#FFF'>There was an error with your request." + ex.Message + "<br/>" + ex.StackTrace + "</div>");
        }
        Response.StatusCode = System.Convert.ToInt16(MyItem.Fields["Status Code"].Value);
        Response.Redirect(MyItem.Fields["Location"].Value);

    }
    private void showPageEditorView(Sitecore.Data.Items.Item MyItem)
    {
        loc.Item = MyItem;
        sc.Item = MyItem;

    }*/
  }
}
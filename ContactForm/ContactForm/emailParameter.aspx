<%@ Page Language="C#" %>


        <% 
            try
            {
                NameValueCollection nvc = Request.Form;
                string msg = "";
                foreach (string key in nvc.Keys)
                {
                    if (!key.StartsWith("ef_"))
                    {
                        msg += key + ": " + nvc[key] + "<br/>";
                    }
                }

                Sitecore.Data.Items.Item it = Sitecore.Context.Database.GetItem(Request.Form["ef_datasource"]);

                if (it == null)
                {
                    Response.Write("There was an error submitting your information.");
                    throw(new Exception("Webform submitted without valid datasource object."));
                }
                
                Response.Write(it["SuccessMessage"]);

                if (!String.IsNullOrEmpty(it["FromAddress"]))
                {
                    HealthIS.Apps.Util.sendEmail(it["ToAddress"], it["Subject"], msg, it["FromAddress"], true);
                }
                else
                {
                    HealthIS.Apps.Util.sendEmail(it["ToAddress"], it["Subject"], msg, true);
                }

            }
            catch (Exception)
            {
                Response.Write("An error has occured");
            }
        %>

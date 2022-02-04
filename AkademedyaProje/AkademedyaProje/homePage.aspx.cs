using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkademedyaProje
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["nickName"] != null)
                {
                    

                }
                else
                {
                    Response.Redirect("loginPage.aspx");
                }

            }

        }
    }
}
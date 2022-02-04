using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkademedyaProje
{
    public partial class Home : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["nickName"] != null) //Session Kontrol
                {
                    Label1.Text = Session["nickName"].ToString();
                }
                else
                {
                    HttpCookie cookieMaster = new HttpCookie("cookieMasterMaster"); //çerezimize isim verdik
                    cookieMaster.Values.Add("username", Label1.Text); //eposta çerezine değeri atadık                 
                    cookieMaster.Expires = DateTime.Now.AddDays(10); //çerezimizin geçerli olacağı süreyi girdik 10 gün
                    Response.Cookies.Add(cookieMaster); //çerezi ekledik 
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e) //Çıkış İşlemi
        {
            Session.RemoveAll();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            Response.Redirect("loginPage.aspx");
        }
    }
}
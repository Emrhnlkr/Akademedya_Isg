using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AkademedyaProje
{
    public partial class LoginPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Cookies["cookie"] != null) //çerezimiz var ise
                {
                    HttpCookie registeredCookie = Request.Cookies["cookie"]; //ismini verdiğimiz çerezi yakalıyoruz
                    txtUserName.Text = registeredCookie.Values["username"]; //cookiese değeri atıyoruz
                    CheckBox1.Checked = true;
                }
                ActivateMyAccount();
            }
        }
        private void ActivateMyAccount()
        {
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
                if ((!string.IsNullOrEmpty(Request.QueryString["UserID"])) & (!string.IsNullOrEmpty(Request.QueryString["Email"])))
                {
                    cmd = new SqlCommand("UPDATE Tbl_Users SET Is_Approved=1 WHERE UserId=@UserID AND Email=@Email", con);
                    cmd.Parameters.AddWithValue("@UserID", Request.QueryString["UserID"]);
                    cmd.Parameters.AddWithValue("@Email", Request.QueryString["Email"]);
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Hesabınız aktif edildi.Hesaba Giriş yapabilirsin');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                return;
            }
            finally
            {
                con.Close();
                cmd.Dispose();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
            cmd = new SqlCommand("Select * from Tbl_Users WHERE UserName=@username AND Password=@password and Is_Approved=1", con);
            cmd.Parameters.AddWithValue("@username", txtUserName.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (CheckBox1.Checked)
                {
                    HttpCookie cerez = new HttpCookie("cerezim"); //çerezimize isim verdik
                    cerez.Values.Add("username", txtUserName.Text); //kullanıcı adı çerezine değeri atadık
                    cerez.Values.Add("password", txtPassword.Text); //şifre çerezine değeri atadık
                    cerez.Expires = DateTime.Now.AddDays(10); //çerezimizin geçerli olacağı süreyi girdik 10 gün
                    Response.Cookies.Add(cerez); //çerezi ekledik
                }
                Session["nickName"] = txtUserName.Text;
                Response.Redirect("HomePage.aspx");
            }
            else
            {
                if (txtUserName.Text == " ")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kullanıcı Adı Boş Olamaz');", true);
                }
                if (txtPassword.Text == " ")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Şifre Boş Olamaz');", true);
                }
                else
                {
                    string myStringVariable = "Kullanıcı Adını veya Şifreni Kontrol Et!";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + myStringVariable + "');", true);
                }
            }

        }
    }
}
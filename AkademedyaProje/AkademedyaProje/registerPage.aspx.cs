using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web.Security;

namespace AkademedyaProje
{
    public partial class RegisterPage : System.Web.UI.Page
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
        MailMessage msg;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Button1_Click(object sender, EventArgs e)//Kayıt İşlemi
        {
            SqlCommand cmd = new SqlCommand();
            string activationUrl = string.Empty;
            string email = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);//Aynı Kullanıcı Adı İle Tekrar Kayıt Engeli
                cmd = new SqlCommand("Select UserId from Tbl_Users WHERE UserName=@username", con);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Aynı Kullanıcı Adı ile Kayıt Olamazsınız');", true);
                    con.Close();
                    cmd.Dispose();
                }
                else
                {
                    cmd = new SqlCommand("insert into Tbl_Users (Name,Surname,Email,UserName,Password) values (@Name,@Surname,@Email,@UserName,@Password) ", connection);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surname", txtSurname.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.Trim());
                    /*txtPassword.Text = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text, "md5");*/ //Girilen şifreyi md5 formatında şifreliyoruz veri güvenliği amacıyla
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    connection.Open();
                    schemaName(txtUsername.Text);//Şema Oluşturma Fonk.
                    cmd.ExecuteNonQuery();
                    //Mail gönderme işlemi buradan itibaren başlıyor.
                    msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    msg.From = new MailAddress("emirhanlkr3667@gmail.com"); //Gönderen mail adresi
                    msg.To.Add(txtEmail.Text); //Alıcı mail adresi
                    msg.Subject = "Hesap aktivasyonu için onay e-postası"; //Mail konu başlığı
                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                    activationUrl = Server.HtmlEncode("http://localhost:55348/LoginPage.aspx?UserID=" + fetchUser(txtEmail.Text) + "&Email=" + txtEmail.Text); //Aktiflik için url oluşturuyoruz.
                    msg.Body = "Selam !\n" + txtUsername.Text.Trim() + "\n" + "Kullanıcı Adı ile Giriş Yapabilirsiniz." + "!\n" +
                          " Lütfen <a href='" + activationUrl + "'> Hesabınızı aktif etmek için buraya tıklayınız.</a>. \n Teşekkür ederiz!";
                    msg.IsBodyHtml = true;
                    msg.BodyEncoding = UTF8Encoding.UTF8;
                    smtp.Credentials = new NetworkCredential("emirhanlkr3667@gmail.com", "Emirhan.3667");
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(msg);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Hesabınızı etkinleştirmek için onay linki e-posta adresinize gönderildi');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Mesaj", "alert('Hata : " + ex.Message.ToString() + "');", true);
                return;
            }
            finally
            {
                activationUrl = string.Empty;
                email = string.Empty;
                connection.Close();
                cmd.Dispose();
            }
        }
        private string fetchUser(string email) //Userıd erişimi için url'de
        {
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("SELECT UserId FROM Tbl_Users WHERE Email=@email", connection);
            cmd.Parameters.AddWithValue("@email", email);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            string userID = Convert.ToString(cmd.ExecuteScalar());
            connection.Close();
            cmd.Dispose();
            return userID;
        }
        private string schemaName(string userName) //Veritabanında Schema Oluşturmak için
        {
            SqlConnection con = new SqlConnection(); //Veritabanı bağlantı ve kullanımı için
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
            con.Open();
            StringBuilder command = new StringBuilder();
            command.Append("CREATE SCHEMA " + userName);
            SqlCommand komut = new SqlCommand(command.ToString(), con);
            komut.ExecuteNonQuery();
            con.Close();
            return userName;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkademedyaProje
{
    public partial class tableCreatePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack) //Sayfa ilk kez çalışıyorsa.
            {
                if (HttpContext.Current.Session == null && HttpContext.Current.Session["nickName"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                }
            }
        }

        private int nextId //Her yeni element için bir sonraki id değeri veriyor.
        {
            get
            {
                return controlsTextbox.Count + 1;
            }
        }
        protected override void LoadViewState(object savedState) //Sayfa ilk açıldığında çalışacak kod parçası.Sayfadaki elementleri oluşturuyor.
        {
            base.LoadViewState(savedState);
            foreach (string txtId in controlsTextbox) //Textbox elementelerini oluşturuyor
            {
                TextBox txt = new TextBox();
                txt.ID = txtId;
                PlaceHolder1.Controls.Add(txt);
            }
            foreach (string dropdownId in controlsDropdown) //Dropdown elementlerini oluşturuyor
            {
                DropDownList dropdown = new DropDownList();
                dropdown.Items.Add("Tamsayı");
                dropdown.Items.Add("Metin");
                dropdown.Items.Add("Ondalık");
                dropdown.Items.Add("Tarih");
                dropdown.ID = dropdownId;
                PlaceHolder2.Controls.Add(dropdown);
            }
            foreach (string chkboxId in controlsCheckbox) //Checkbox elemenlerini oluşturuyor
            {
                CheckBox chkbx = new CheckBox();
                chkbx.ID = chkboxId;
                PlaceHolder3.Controls.Add(chkbx);
                PlaceHolder3.Controls.Add(new LiteralControl("<br>"));
            }
        }
        protected void Button1_Click(object sender, EventArgs e) //Tablo oluşturma işlemlerine burada başlıyoruz.
        {
            try
            {
                SqlConnection con = new SqlConnection(); //Veritabanı bağlantı ve kullanımı için
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
                con.Open();
                StringBuilder command = new StringBuilder();
                if (txtTblName.Text == "") //Tablo ismi boş olamaz.
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Tablo Adı Boş Olamaz! Lütfen bir tablo adı giriniz.');", true);
                }
                string userName = (HttpContext.Current.Session["nickName"].ToString());
                command.Append("Create Table " + userName + "." + txtTblName.Text + " ( tbl_Id Int Identity(1,1) Primary Key, ");
                List<String> textboxList = new List<String>();//Elemente girilen verileri kayıt etmek için bir liste de tutuyoruz.
                List<String> dropdownList = new List<String>();
                List<bool> checkList = new List<bool>();
                foreach (Control txtbx in PlaceHolder1.Controls)
                {
                    if (txtbx is TextBox)
                    {
                        TextBox txtbox = txtbx as TextBox;
                        if (txtbox.Text == "") //Boş alan kontolü yapıyoruz.
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Tablo Sütun Alanı Boş Olamaz Doldurunuz.!');", true);
                        }
                        textboxList.Add(txtbox.Text);
                    }
                }
                foreach (Control dropdown in PlaceHolder2.Controls)
                {
                    if (dropdown is DropDownList)
                    {
                        DropDownList dropdwn = dropdown as DropDownList;
                        if (dropdwn.SelectedValue == "Tamsayı")
                        {
                            dropdownList.Add("int");
                        }
                        if (dropdwn.SelectedValue == "Metin")
                        {
                            dropdownList.Add("varchar(100)");
                        }
                        if (dropdwn.SelectedValue == "Ondalık")
                        {
                            dropdownList.Add("decimal(5,2)");
                        }
                        if (dropdwn.SelectedValue == "Tarih")
                        {
                            dropdownList.Add("datetime");
                        }
                        //dropdownList.Add(dropdwn.SelectedValue);
                    }
                }
                foreach (Control chkbx in PlaceHolder3.Controls)
                {
                    if (chkbx is CheckBox)
                    {
                        CheckBox chkbox = chkbx as CheckBox;
                        checkList.Add(chkbox.Checked);
                    }
                }
                for (int i = 0; i < controlsTextbox.Count; i++)
                {
                    string Is_null;
                    if (checkList[i])
                    {
                        Is_null = " NULL ";
                    }
                    else
                    {
                        Is_null = " NOT NULL ";
                    }
                    command.Append(textboxList[i] + " " + dropdownList[i] + Is_null + " , ");
                }
                command.Length -= 2; //sondaki virgül için
                command.Append(")");
                SqlCommand komut = new SqlCommand(command.ToString(), con);
                komut.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Tablo Oluşturma İşlemi Başarılı');", true);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Aynı Tablo Adıyla Kayıt Yapılamaz.Tablo Adını değiştiriniz.');", true);
            }

        }
        protected void Button2_Click(object sender, EventArgs e) //Butona2 ye basıldığında çalışacak kod.Elemenleri artıracak kod.
        {
            TextBox txtbx = new TextBox();
            DropDownList dropdown = new DropDownList();
            CheckBox checkbx = new CheckBox();

            dropdown.Items.Add("Tamsayı");
            dropdown.Items.Add("Metin");
            dropdown.Items.Add("Ondalık");
            dropdown.Items.Add("Tarih");

            txtbx.ID = "Textbox" + nextId;
            dropdown.ID = "Dropdown" + nextId;
            checkbx.ID = "Checkbox" + nextId;

            PlaceHolder1.Controls.Add(txtbx);
            controlsTextbox.Add(txtbx.ID);

            PlaceHolder2.Controls.Add(dropdown);
            controlsDropdown.Add(dropdown.ID);

            PlaceHolder3.Controls.Add(checkbx);
            controlsCheckbox.Add(checkbx.ID);
        }
        protected void Button3_Click(object sender, EventArgs e) //Sayfayı Sıfırlamak için
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        private List<string> controlsTextbox //Textbox için control işlemi
        {
            get
            {
                if (ViewState["controlsTxtbox"] == null)
                {
                    ViewState["controlsTxtbox"] = new List<string>();
                }

                return (List<string>)ViewState["controlsTxtbox"];
            }
        }
        private List<string> controlsDropdown
        {
            get
            {
                if (ViewState["controlsDropdown"] == null)
                {
                    ViewState["controlsDropdown"] = new List<string>();
                }

                return (List<string>)ViewState["controlsDropdown"];
            }
        }
        private List<string> controlsCheckbox
        {
            get
            {
                if (ViewState["controlsCheckbox"] == null)
                {
                    ViewState["controlsCheckbox"] = new List<string>();
                }

                return (List<string>)ViewState["controlsCheckbox"];
            }
        }
    }

}
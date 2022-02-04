using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Web.UI.WebControls;
using Table = Microsoft.SqlServer.Management.Smo.Table;
namespace AkademedyaProje
{
    public partial class tableAddListPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (HttpContext.Current.Session != null && HttpContext.Current.Session["nickName"] != null)
                {
                    string username = (HttpContext.Current.Session["nickName"].ToString());
                    this.DropDownList1.DataSource = getData("select  t.name as table_name from sys.tables t where schema_name(t.schema_id) = '" + username + "' order by table_name");
                    this.DropDownList1.DataValueField = "TABLE_NAME";
                    this.DropDownList1.DataTextField = "TABLE_NAME";
                    this.DropDownList1.DataBind();
                    DropDownList1.Items.Insert(0, new ListItem("Tablo Seçiniz", "0"));
                    DropDownList1.Enabled = true;
                    for (int i = 1; i < this.DropDownList1.Items.Count; i++) //Veritabaına Kayıtlı Tablo Adeti
                    {
                        this.DropDownList1.Items[i].Attributes.Add("onclick", "MutExChkList(this)");
                    }
                }
                else
                {
                    Response.Redirect("loginPage.aspx");
                }

            }
        }
        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.gvData.DataSource = null;
            this.gvData.DataBind();
            string tableName = DropDownList1.SelectedValue;
            string username = (HttpContext.Current.Session["nickName"].ToString());
            if (tableName != "0") //Dropdown'ın ilk elementi "seçiniz" işleme dikkate alınmaması için
            {
                DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
                if (dtData.Rows.Count == 0) //Tablo boş ise sütün başlıkları gelsin
                {
                    dtData.Rows.Add(dtData.NewRow());
                    gvData.DataSource = dtData;
                    gvData.DataBind();
                    gvData.Rows[0].Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Tablo Boş');", true);
                }
                else
                {
                    int totalColumns = dtData.Columns.Count;
                    for (int i = 1; i < totalColumns; i++)
                    {
                        TemplateField field = new TemplateField();
                        field.ItemTemplate = new GridViewTemplate("Column" + i.ToString(), i.ToString());
                    }
                    this.gvData.DataSource = dtData;
                    this.gvData.DataBind();
                }
            }
        }
        protected void deleteData_Click(object sender, EventArgs e) //Tablodan Kayıt Silmek İçin
        {
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string columnName = (sender as Button).CommandArgument;
            string tableName = DropDownList1.SelectedValue;
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            this.gvData.DataSource = dtData;
            int totalColumns = dtData.Columns.Count;
            DataTable dtDelete = getData(string.Format("DELETE FROM {0} WHERE tbl_Id = {1}", username + "." + tableName, columnName));
            this.gvData.DataBind();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kayıt Silindi');", true);
            OnSelectedIndexChanged(null, null);
        }
        protected void updateData_Click(object sender, EventArgs e) //Tabloyu Güncellemek için
        {
            try
            {
                string username = (HttpContext.Current.Session["nickName"].ToString());
                string columnName = (sender as Button).CommandArgument;
                string tableName = DropDownList1.SelectedValue;
                DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
                SqlConnection con = new SqlConnection();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
                con.Open();
                StringBuilder command = new StringBuilder();
                command.Append("UPDATE " + username + "." + tableName + " SET ");
                int totalColumns = dtData.Columns.Count;
                int index = 1;
                int minIndex = PlaceHolder5.Controls.Count / 2 - totalColumns; //Listeinin hep sonundakini column sayısını almak için
                List<String> textmodalvalueList = new List<String>();
                List<String> idDeger = new List<String>();

                foreach (Control ctlmodal in PlaceHolder5.Controls)
                {
                    if (ctlmodal is TextBox)
                    {
                        if (index > minIndex)
                        {
                            TextBox txtmodal = ctlmodal as TextBox;
                            textmodalvalueList.Add(txtmodal.Text);
                        }
                        index++;
                    }
                }
                for (int i = 0; i < totalColumns; i++)
                {
                    if (dtData.Columns[i].ToString() == "tbl_Id")
                    {

                        idDeger.Add(textmodalvalueList[0]);
                    }
                    else
                    {
                        command.Append(dtData.Columns[i].ToString() + " = ' " + textmodalvalueList[i] + " ' , ");
                    }

                }
                command.Length -= 2; //sondaki virgül için
                command.Append(" WHERE tbl_Id = " + idDeger[0]);
                SqlCommand komut = new SqlCommand(command.ToString(), con);
                komut.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Güncelleme Başarılı.');", true);
                this.gvData.DataBind();
            }
            catch (Exception ex)
            {
                var exmsg = ex.Message;
                errorMessage(exmsg);

            }
            OnSelectedIndexChanged(null, null);
        }
        protected void errorMessage(string exmsg)
        {
            if (exmsg.Contains("Conversion failed when converting the varchar"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('İnt olarak tanımlanmış alana string girilmez. KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("date and/or time from character string"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Datetime olarak tanımlanmış alana string girilmez. KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("data type varchar to numeric"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Decimal alana bu tipe uymayan veri tipte veri girilemez. KAYIT BAŞARISIZ');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kayıt Başarısız');", true);
            }
        }

        protected void addData_Click(object sender, EventArgs e) //modaldan veri ekleme
        {
            try
            {
                string username = (HttpContext.Current.Session["nickName"].ToString());
                SqlConnection con = new SqlConnection();
                string tableName = DropDownList1.SelectedValue;
                DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
                con.Open();
                StringBuilder command = new StringBuilder();
                command.Append("INSERT INTO " + username + "." + tableName + " VALUES ( ");
                int totalColumns = dtData.Columns.Count;
                int index = 0;
                int minIndex = PlaceHolder4.Controls.Count / 2 - totalColumns; //Listeinin hep sonundakini column sayısını almak için
                foreach (Control ctl in PlaceHolder4.Controls)
                {
                    if (ctl is TextBox)
                    {
                        if (index > minIndex)
                        {
                            TextBox txt = ctl as TextBox;
                            if (index == 0)
                            {
                                //command.Append(txt.Text + " , ");
                            }
                            else
                            {
                                command.Append("'" + txt.Text + "' , ");
                            }
                        }
                        index++;
                    }
                }
                command.Length -= 2; //sondaki virgül için
                command.Append(" ) ");
                SqlCommand komut = new SqlCommand(command.ToString(), con);
                komut.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kayıt Başarılı.');", true);
            }
            catch (Exception ex)
            {
                var exmsg = ex.Message;
                errorMessage(exmsg);
            }
            OnSelectedIndexChanged(null, null);
        }
        protected void close_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        protected void updateModal_Click(object sender, EventArgs e) //Güncelleme yapılan Modal için
        {
            PlaceHolder5.Controls.Clear();
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string columnName = (sender as Button).CommandArgument;
            string tableName = DropDownList1.SelectedValue;
            DataTable dtData = getData(string.Format("SELECT * FROM {0} WHERE tbl_Id={1}", username + "." + tableName, columnName));
            int totalColumns = dtData.Columns.Count;
            Random rnd = new Random();
            for (int i = 0; i < totalColumns; i++)
            {
                int index = rnd.Next(0, 100000);
                if (dtData.Columns[i].ToString() == "tbl_Id")
                {

                    TextBox txtupdatemdl = new TextBox();
                    txtupdatemdl.ID = "textboxUpdateModal" + index;
                    txtupdatemdl.Text = dtData.Rows[0][i].ToString();
                    txtupdatemdl.ReadOnly = true;
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    ControlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }
                else
                {
                    TextBox txtupdatemdl = new TextBox();
                    txtupdatemdl.ID = "textboxUpdateModal" + index;
                    txtupdatemdl.Text = dtData.Rows[0][i].ToString();
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    ControlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }

            }
            Label1.Text = "Tablodan Veri Güncelle";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "updateModal", "$('#updateModal').modal();", true);
            OnSelectedIndexChanged(null, null);
        }

        protected void dataAddModal_Click(object sender, EventArgs e) //Modala element eklemek için
        {
            PlaceHolder4.Controls.Clear();
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string tableName = DropDownList1.SelectedValue;
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            int totalColumns = dtData.Columns.Count;
            Random rnd = new Random();
            for (int i = 0; i < totalColumns; i++)
            {
                int index = rnd.Next(0, 100000);
                if (dtData.Columns[i].ToString() == "tbl_Id")
                {
                    TextBox txtmdl = new TextBox();
                    txtmdl.ID = "textboxModal" + index;
                    txtmdl.Text = dtData.Columns[i].ToString();
                    txtmdl.ReadOnly = true;
                    PlaceHolder4.Controls.Add(txtmdl);
                    ControlsListModal.Add(txtmdl.ID);
                    PlaceHolder4.Controls.Add(new LiteralControl("<br>"));

                }
                else
                {
                    TextBox txtmdl = new TextBox();
                    txtmdl.ID = "textboxModal" + index;
                    txtmdl.Text = dtData.Columns[i].ToString();
                    PlaceHolder4.Controls.Add(txtmdl);
                    ControlsListModal.Add(txtmdl.ID);
                    PlaceHolder4.Controls.Add(new LiteralControl("<br>"));
                }

            }

            lblModalTitle.Text = "Tabloya Veri Ekle";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
        }
        private List<string> ControlsListlabel
        {
            get
            {
                if (ViewState["controlslabel"] == null)
                {
                    ViewState["controlslabel"] = new List<string>();
                }
                return (List<string>)ViewState["controlslabel"];
            }
        }
        private List<string> ControlsListModal
        {
            get
            {
                if (ViewState["controlsmdl"] == null)
                {
                    ViewState["controlsmdl"] = new List<string>();
                }
                return (List<string>)ViewState["controlsmdl"];
            }
        }
        private List<string> ControlsUpdateModal
        {
            get
            {
                if (ViewState["controlsupdatemdl"] == null)
                {
                    ViewState["controlsupdatemdl"] = new List<string>();
                }
                return (List<string>)ViewState["controlsupdatemdl"];
            }
        }
        protected void search_Click(object sender, EventArgs e) //Arama yapılabilmesi için
        {
            string tableName = DropDownList1.SelectedValue;
            string username = (HttpContext.Current.Session["nickName"].ToString());
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM " + username + "." + tableName + " WHERE " + dtData.Columns[1].ToString() + " LIKE '%' + @searchText + '%'";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@searchText", txtSearch.Text.Trim());
                    if (txtSearch.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Message", "alert('Arama Boş');", true);
                    }
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvData.DataSource = dt;
                        gvData.DataBind();
                    }
                }
            }

        }
        protected void sqlScript_Click(object sender, EventArgs e) //Sql Script Almak için
        {
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string tableName = DropDownList1.SelectedValue;
            string appPath = @"C:\Users\emirh\Downloads\";
            string filePath = appPath + username + "." + tableName + ".sql";
            StreamWriter yaz;
            yaz = File.CreateText(filePath);
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            int totalColumns = dtData.Columns.Count;
            string firstQuery = "CREATE SCHEMA [Bdenizli]" + "\n" +
                     "GO" + "\n" +
                    "SET ANSI_NULLS ON" + "\n" +
                    "GO" + "\n" +
                    "SET QUOTED_IDENTIFIER ON" + "\n" +
                    "GO" + "\n" +
                    " CREATE TABLE " + "[" + username + "]" + ".[" + tableName + "]" + "(" + "\n" +
                    "[tbl_Id] [int] IDENTITY(1,1) NOT NULL,";
            string lastQuery = "PRIMARY KEY CLUSTERED" + "\n" +
                "(" + "\n" +
                "[tbl_Id] ASC" + "\n" +
                ")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]" +
                ")ON [PRIMARY]";
            List<String> queryList = new List<String>();
            queryList.Add("0");
            yaz.WriteLine(firstQuery);
            for (int i = 1; i < totalColumns; i++)
            {
                if (dtData.Columns[i].DataType.Name.ToString() == "Int32")
                {
                    queryList.Add("[" + dtData.Columns[i].ToString() + "]" + "[" + dtData.Columns[i].DataType.Name.ToString() + "] NOT NULL,");
                }
                else
                {
                    queryList.Add("[" + dtData.Columns[i].ToString() + "]" + "[" + dtData.Columns[i].DataType.Name.ToString() + "](100) NOT NULL,");
                }
                yaz.WriteLine(queryList[i]);
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Message", "alert('İndirme Başarılı.İNDİRİLENLERE bakınız');", true);
            yaz.WriteLine(lastQuery);
            yaz.Flush();
            yaz.Close();
        }
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            foreach (string txtmdl in ControlsListModal)
            {
                TextBox txtmodal = new TextBox();
                txtmodal.ID = txtmdl;
                PlaceHolder4.Controls.Add(txtmodal);
                PlaceHolder4.Controls.Add(new LiteralControl("<br>"));
            }
            foreach (string txtupdatemdl in ControlsUpdateModal)
            {
                TextBox txtUpdatetMdl = new TextBox();
                txtUpdatetMdl.ID = txtupdatemdl;
                PlaceHolder5.Controls.Add(txtUpdatetMdl);
                PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
            }

        }
        private DataTable getData(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
        public class GridViewTemplate : ITemplate //Tabloda Sayfalama işlemi için
        {
            private string columnNameBinding;
            public GridViewTemplate(string colname, string colNameBinding)
            {
                columnNameBinding = colNameBinding;
            }
            public void InstantiateIn(System.Web.UI.Control container)
            {
                List<String> inputId = new List<String>();
                TextBox tb = new TextBox();
                tb.ID = "txtDynamic" + columnNameBinding;
                container.Controls.Add(tb);
            }
        }
        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            OnSelectedIndexChanged(null, null);
        }
    }
}
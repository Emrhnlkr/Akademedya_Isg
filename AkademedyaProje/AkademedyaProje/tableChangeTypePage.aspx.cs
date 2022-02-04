using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AkademedyaProje
{
    public partial class tableChangeTypePage : System.Web.UI.Page
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
                    for (int i = 1; i < this.DropDownList1.Items.Count; i++) //Veritabaına Kyıtlı Tablo Adeti
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
            if (tableName != "0") //Dropdown'ın ilk elementi "seçiniz" işleme dikkate alınmaması için
            {
                string username = (HttpContext.Current.Session["nickName"].ToString());
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
                        //field.HeaderText = dt1.Columns[i].ToString();
                        field.ItemTemplate = new GridViewTemplate("Column" + i.ToString(), i.ToString());
                    }
                    this.gvData.DataSource = dtData;
                    this.gvData.DataBind();
                }
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
        public class GridViewTemplate : ITemplate
        {
            private string columnNameBinding;
            public GridViewTemplate(string colname, string colNameBinding)
            {
                columnNameBinding = colNameBinding;
            }
            public void InstantiateIn(System.Web.UI.Control container)
            {

                List<String> inputId = new List<String>();
                DropDownList tb = new DropDownList();
                tb.ID = "txtDynamic" + columnNameBinding;
                tb.Items.Add("int");
                tb.Items.Add("varchar(100)");
                tb.Items.Add("decimal(5,2)");
                tb.Items.Add("datetime");
                container.Controls.Add(tb);
            }
        }
        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvData.PageIndex = e.NewPageIndex;
            OnSelectedIndexChanged(null, null);
        }
        protected void updateData_Click(object sender, EventArgs e) //Tabloyu Güncellemek için
        {
            try
            {
                string username = (HttpContext.Current.Session["nickName"].ToString());
                string tableName = DropDownList1.SelectedValue;
                DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
                SqlConnection con = new SqlConnection();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["conStr"].ConnectionString);
                con.Open();
                int totalColumns = dtData.Columns.Count;
                StringBuilder command = new StringBuilder();
                List<String> idDeger = new List<String>();
                List<String> ilktblname = new List<string>();
                List<String> ilktypename = new List<string>();
                int index = 1;
                int minIndex = PlaceHolder5.Controls.Count / 2 - totalColumns;
                foreach (Control ctlmodal in PlaceHolder5.Controls)
                {
                    if (ctlmodal is DropDownList)
                    {
                        if (index > minIndex)
                        {

                            DropDownList txtmodal = ctlmodal as DropDownList;
                            ilktypename.Add(txtmodal.SelectedValue);
                        }
                        index++;
                    }
                }
                for (int i = 0; i < totalColumns; i++)
                {
                    ilktblname.Add(dtData.Columns[i].ToString());
                    if (dtData.Columns[i].ToString() == "tbl_Id")
                    {
                        idDeger.Add(ilktypename[0]);
                    }
                    else
                    {
                        if (dtData.Columns[i].DataType.Name.ToString() == "Decimal" && ilktypename[i] == "int")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Decimal tip den İnt tipine dönüşümde veri kaybı olacaktır');", true);
                        }
                        if (dtData.Columns[i].DataType.Name.ToString() == "Int32" && ilktypename[i] == "datetime")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Decimal tip den İnt tipine dönüşümde veri kaybı olacaktır');", true);
                        }
                        if (dtData.Columns[i].DataType.Name.ToString() == "Decimal" && ilktypename[i] == "datetime")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Decimal tip den datetime tipine dönüşümde veri kaybı olacaktır');", true);
                        }
                        if (dtData.Columns[i].DataType.Name.ToString() == "Int32")
                        {
                            ////Response.Write("Tip integer: " + dt1.Columns[i].DataType.Name.ToString());
                            ////Response.Write("Seçilen Tip : " + Ilktypename[i]);
                            //Response.Write("Alan adı: " + Ilktblname[i]);
                        }
                        command.Append(" ALTER TABLE " + username + "." + tableName + " ALTER COLUMN ");
                        command.Append(ilktblname[i] + " " + ilktypename[i]);
                    }
                }
                SqlCommand komut = new SqlCommand(command.ToString(), con);
                komut.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Güncelleme Başarılı.');", true);
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
            if (exmsg.Contains("Conversion failed when converting the varchar value"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('String olarak tanımlanmış tip İnt tipe dönüştürülemez . KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("Error converting data type varchar to numeric"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('String olarak tanımlanmış tip Decimal tipe dönüştürülemez. KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("Conversion failed when converting date and/or time from character string"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('String olarak tanımlanmış tip Datetime tipine dönüştüülemez. KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("Implicit conversion from data type datetime to int is not allowed"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Datetime olarak tanımlanmış tip int tipine dönüştüülemez. KAYIT BAŞARISIZ');", true);
            }
            else if (exmsg.Contains("Implicit conversion from data type datetime to decimal is not allowed"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Datetime olarak tanımlanmış tip Decimal tipine dönüştüülemez. KAYIT BAŞARISIZ');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Kayıt Başarısız');", true);
            }
        }

        private int nextId //Her yeni element için bir sonraki id değeri veriyor.
        {
            get
            {
                return controlsUpdateModal.Count + 1;
            }
        }
        protected void updateModal_Click(object sender, EventArgs e)
        {
            PlaceHolder5.Controls.Clear();
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string columnName = (sender as Button).CommandArgument;
            string tableName = DropDownList1.SelectedValue;
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            int totalColumns = dtData.Columns.Count;
            List<String> selectedType = new List<string>();
            for (int i = 0; i < totalColumns; i++)
            {
                selectedType.Add(dtData.Columns[i].DataType.Name.ToString());
                if (dtData.Columns[i].ToString() == "tbl_Id")
                {
                    DropDownList txtupdatemdl = new DropDownList();
                    txtupdatemdl.Items.Add("int");
                    txtupdatemdl.Items.Add("varchar(100)");
                    txtupdatemdl.Items.Add("decimal(5,2)");
                    txtupdatemdl.Items.Add("datetime");
                    txtupdatemdl.ID = "dropdownUpdateModal" + nextId;
                    txtupdatemdl.Enabled = false;
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    controlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }
                else
                {
                    DropDownList txtupdatemdl = new DropDownList();
                    //txtupdatemdl.Items.Add("seçilen değer ");
                    txtupdatemdl.Items.Add("int");
                    txtupdatemdl.Items.Add("varchar(100)");
                    txtupdatemdl.Items.Add("decimal(5,2)");
                    txtupdatemdl.Items.Add("datetime");
                    txtupdatemdl.ID = "dropdownUpdateModal" + nextId;
                    //txtupdatemdl.SelectedItem.Text = selectedType[i];
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    controlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }
            }
            Label1.Text = "Tablodan Veri Güncelle";
            //lblModalBody.Text = "Burası modal metin bölümü";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "updateModal", "$('#updateModal').modal();", true);
        }
        protected override void LoadViewState(object savedState)
        {

            base.LoadViewState(savedState);
            foreach (string txtupdatemdl in controlsUpdateModal)
            {
                DropDownList txtUpdatetMdl = new DropDownList();

                txtUpdatetMdl.Items.Add("int");
                txtUpdatetMdl.Items.Add("varchar(100)");
                txtUpdatetMdl.Items.Add("decimal(5,2)");
                txtUpdatetMdl.Items.Add("datetime");
                txtUpdatetMdl.ID = txtupdatemdl;
                PlaceHolder5.Controls.Add(txtUpdatetMdl);
            }
        }
        private List<string> controlsUpdateModal
        {
            get
            {
                if (ViewState["controlsUpdatemdl"] == null)
                {
                    ViewState["controlsUpdatemdl"] = new List<string>();
                }

                return (List<string>)ViewState["controlsUpdatemdl"];
            }
        }
        protected void search_Click(object sender, EventArgs e)
        {
            string username = (HttpContext.Current.Session["nickName"].ToString());
            string tableName = DropDownList1.SelectedValue;
            DataTable dtData = getData(string.Format("SELECT * FROM {0}", username + "." + tableName));
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM " + username + "." + tableName + " WHERE " + dtData.Columns[1].ToString() + " LIKE '%' + @searchText + '%'";
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@searchText", txtSearch.Text.Trim());
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
    }
}
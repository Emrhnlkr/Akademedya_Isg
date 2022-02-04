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
    public partial class tableChangeColumnPage : System.Web.UI.Page
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
                else //Boş değilse verileri gösteriyor
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
                StringBuilder command = new StringBuilder();
                int totalColumns = dtData.Columns.Count;
                int index = 1;
                int minIndex = PlaceHolder5.Controls.Count / 2 - totalColumns; //Listeinin hep sonundakini column sayısını almak için
                List<String> textmodalvalueList = new List<String>();
                List<String> idDeger = new List<String>();
                List<String> Ilktblname = new List<string>();
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
                    Ilktblname.Add(dtData.Columns[i].ToString());
                    if (dtData.Columns[i].ToString() == "tbl_Id")
                    {
                        idDeger.Add(textmodalvalueList[0]);
                    }
                    else
                    {
                        command.Append(" EXEC sp_rename " + " '" + username + "." + tableName + ".");
                        command.Append(Ilktblname[i].ToString() + "' " + " , " + "'" + textmodalvalueList[i].ToString() + "' " + " ," + "'" + "COLUMN" + "'");
                    }
                }
                //command.Length -= 2; //sondaki virgül için
                SqlCommand komut = new SqlCommand(command.ToString(), con);
                komut.ExecuteNonQuery();
                con.Close();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Güncelleme Başarılı.');", true);
                OnSelectedIndexChanged(null, null);
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Güncelleme Başarısız.Tekrar Deneyiniz');", true);
            }
            OnSelectedIndexChanged(null, null);
        }
        private int nextId //Her yeni element için bir sonraki id değeri veriyor.
        {
            get
            {
                return ControlsUpdateModal.Count + 1;
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
            for (int i = 0; i < totalColumns; i++)
            {
                if (dtData.Columns[i].ToString() == "tbl_Id")
                {
                    TextBox txtupdatemdl = new TextBox();
                    txtupdatemdl.ID = "TextBoxUpdateModal" + nextId;
                    txtupdatemdl.Text = dtData.Columns[i].ToString();
                    txtupdatemdl.ReadOnly = true;
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    ControlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }
                else
                {
                    TextBox txtupdatemdl = new TextBox();
                    txtupdatemdl.ID = "TextBoxUpdateModal" + nextId;
                    txtupdatemdl.Text = dtData.Columns[i].ToString();
                    PlaceHolder5.Controls.Add(txtupdatemdl);
                    ControlsUpdateModal.Add(txtupdatemdl.ID);
                    PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
                }
            }
            Label1.Text = "Tablodan Veri Güncelle";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "updateModal", "$('#updateModal').modal();", true);
            OnSelectedIndexChanged(null, null);
        }
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            foreach (string txtupdatemdl in ControlsUpdateModal)
            {
                TextBox txtUpdatetMdl = new TextBox();
                txtUpdatetMdl.ID = txtupdatemdl;
                PlaceHolder5.Controls.Add(txtUpdatetMdl);
                PlaceHolder5.Controls.Add(new LiteralControl("<br>"));
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
<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeBehind="tableAddListPage.aspx.cs" Inherits="AkademedyaProje.tableAddListPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="~/Scripts/applicationStyles.css" rel="stylesheet" />
    <style type="text/css">
        .tableCreate {
            text-align: center;
            width: 60%;
            margin-top: 2%;
            margin-left: 5%;
        }

        TextBox[type="text"] {
            height: 30px;
            width: 200px;
        }
    </style>
    <div class="container">
        <div class="hero-unit tableCreate">
            <asp:dropdownlist id="DropDownList1" runat="server" autopostback="true"
                onselectedindexchanged="OnSelectedIndexChanged" repeatdirection="Horizontal">
                <asp:ListItem Value="all" Selected="True">Seçiniz</asp:ListItem>
            </asp:dropdownlist>
            <hr />
            Ara:
            <asp:textbox id="txtSearch" runat="server" placeholder="İlk alan adıyla arama yapınız." />
            <asp:button id="btnSearch" text="Ara" runat="server" onclick="search_Click" cssclass="btn btn-info" />
            <asp:gridview runat="server" id="gvData" allowpaging="true" pagesize="3" onpageindexchanging="GridView_PageIndexChanging" cssclass="table table-bordered table-striped">
                <HeaderStyle BackColor="Yellow" />
                <AlternatingRowStyle BackColor="LightGray" />
                <RowStyle BackColor="LightGray" />
                <Columns>
                    <asp:TemplateField HeaderText="İşlemler">
                        <ItemTemplate>
                            <asp:Button Text="Sil" runat="server" OnClick="deleteData_Click" CommandArgument='<%# Eval("tbl_Id") %>' CssClass="btn btn-info" />
                            <asp:Button ID="Button1" runat="server" Text="Güncelle" CssClass="btn btn-success" OnClick="updateModal_Click" CommandArgument='<%# Eval("tbl_Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:gridview>
            <%-- Tabloya Veri Eklemek--%>
            <asp:button id="Button3" class="btn-info" runat="server" text="Veri Ekle" cssclass="btn btn-info btnStyle"
                onclick="dataAddModal_Click"></asp:button>
            <asp:button id="Button5" runat="server" cssclass="btn btn-info btnStyle" text="Sql Script Al" class="btn-info" onclick="sqlScript_Click" />
            <div class="modal fade" id="myModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:label id="lblModalTitle" runat="server" text=""></asp:label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <%--      <asp:Label ID="lblModalBody" runat="server" Text=""></asp:Label>--%>
                            <asp:placeholder id="PlaceHolder4" runat="server"></asp:placeholder>
                        </div>
                        <div class="modal-footer">
                            <asp:button id="Button2" data-dismiss="modal" aria-hidden="true" runat="server" text="Kapat" onclick="close_Click" cssclass="btn btn-danger" />
                            <asp:button id="Button4" runat="server" text="Ekle" onclick="addData_Click" cssclass="btn btn-info btnStyle" />
                        </div>
                    </div>
                </div>
            </div>
            <%-- Veri Güncelleme --%>
            <div class="modal fade" id="updateModal" role="dialog" aria-labelledby="updateModal_Click" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:label id="Label1" runat="server" text=""></asp:label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <asp:placeholder id="PlaceHolder5" runat="server"></asp:placeholder>
                        </div>
                        <div class="modal-footer">
                            <asp:button id="Button6" data-dismiss="modal" aria-hidden="true" runat="server" text="Kapat" cssclass="btn btn-danger" />
                            <asp:button id="Button7" runat="server" text="Güncelle" onclick="updateData_Click" cssclass="btn btn-info btnStyle" commandargument='<%# Eval("tbl_Id") %>' />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

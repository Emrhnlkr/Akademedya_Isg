<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeBehind="tableChangeTypePage.aspx.cs" Inherits="AkademedyaProje.tableChangeTypePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="OnSelectedIndexChanged" RepeatDirection="Horizontal">
                <asp:ListItem Value="all" Selected="True">Seçiniz</asp:ListItem>
            </asp:DropDownList>
            <hr />
            Ara:
            <asp:TextBox ID="txtSearch" runat="server" placeholder="İlk alan adıyla arama yapınız." />
            <asp:Button Text="Ara" runat="server" OnClick="search_Click" CssClass="btn btn-info btnStyle" />
            <asp:GridView runat="server" ID="gvData" AllowPaging="true" PageSize="2" OnPageIndexChanging="GridView_PageIndexChanging" CssClass="table table-bordered table-striped">
                <HeaderStyle BackColor="Yellow" />
                <AlternatingRowStyle BackColor="LightGray" />
                <RowStyle BackColor="LightGray" />
                <Columns>
                </Columns>
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" Text="Güncelle" CssClass="btn btn-success" OnClick="updateModal_Click" CommandArgument='<%# Eval("tbl_Id") %>' />
            <div class="modal fade" id="updateModal" role="dialog" aria-labelledby="updateModal" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title">
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <asp:PlaceHolder ID="PlaceHolder5" runat="server"></asp:PlaceHolder>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="Button6" data-dismiss="modal" aria-hidden="true" runat="server" Text="Kapat" CssClass="btn btn-danger" />
                            <asp:Button ID="Button7" runat="server" Text="Güncelle" OnClick="updateData_Click" CssClass="btn btn-info btnStyle" CommandArgument='<%# Eval("tbl_Id") %>' />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

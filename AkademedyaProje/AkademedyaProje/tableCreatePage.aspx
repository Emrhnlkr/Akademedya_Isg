<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeBehind="tableCreatePage.aspx.cs" Inherits="AkademedyaProje.tableCreatePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--    <link href="Scripts/applicationStyles.css" rel="stylesheet" />--%>
    <style>
        .tableStyles {
            width: 199%;
            border-collapse: collapse;
            font-family: Verdana;
            font-size: 12px;
            background-color: #D5E6F8;
            border: Solid 1px #5497E2;
        }

        .tableTdStyle {
            width: 200px;
        }

        input#ContentPlaceHolder3 {
            padding: 15px;
            margin: -15px 0 0;
        }

        .tableCreate {
            text-align: center;
            width: 60%;
            margin-top: 2%;
            margin-left: 5%;
        }

        .tableElementInc {
            margin-bottom: 1.5%;
            color: #fff;
            background-color: #778899;
            height: 38px;
            padding: 10px;
            border: none 0px transparent;
            font-size: 15px;
            font-weight: lighter;
        }

        .tableCreateBtn {
            margin-left: 85%;
            margin-top: 10%;
            color: #fff;
            background-color: #4682B4;
            height: 35px;
            width: 20px;
            padding: 10px;
            border: none 0px transparent;
            font-size: 15px;
            font-weight: lighter;
        }
    </style>
    <div class="container">
        <div class="hero-unit tableCreate">
            <h5 style="color: red;">*Her tablonun tablo id alanı mevcuttur.Tablo için tekil bir sayı oluşturmayınız!</h5>
            <asp:Label ID="Label1" runat="server" Text="Tablo Adı"></asp:Label>
            <asp:TextBox ID="txtTblName" runat="server"></asp:TextBox>
            <asp:Button ID="Button2" runat="server" Text="Tablo Alan Arttır" OnClick="Button2_Click" CssClass="tableElementInc" />
            <asp:Button ID="Button3" runat="server" Text="Temizle" OnClick="Button3_Click" CssClass="tableElementInc" />
            <table class="tableStyles">
                <tr>
                    <td class="tableTdStyle">Sütun Adı</td>
                    <td class="tableTdStyle">Veri Tipi</td>
                    <td class="tableTdStyle">Boş Olsun mu?</td>
                </tr>
                <tr>
                    <div style="text-align: center; display: inline">
                        <td class="tableTdStyle">
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </td>
                    </div>
                    <div style="text-align: center; display: inline">
                        <td class="tableTdStyle">
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                        </td>
                    </div>
                    <div style="text-align: center; display: inline-flex; position: absolute;">
                        <td class="tableTdStyle">
                            <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                        </td>
                    </div>
                </tr>
            </table>
            <asp:Button ID="Button1" runat="server" Text="Tablo Oluştur" OnClick="Button1_Click" CssClass="tableCreateBtn" />
        </div>
    </div>
</asp:Content>

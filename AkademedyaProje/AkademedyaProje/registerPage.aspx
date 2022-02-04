<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="registerPage.aspx.cs" Inherits="AkademedyaProje.RegisterPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" class="htmlRegister">
<head runat="server">
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/bootstrap-responsive.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.11.1.min.js"></script>
    <link href="~/Scripts/applicationStyles.css" rel="stylesheet" />
    <title>Akademedya Project</title>
</head>
<body class="bodyRegister">
    <form id="form1" runat="server">
        <div class="containerRegister">
            <div class="span4 offset4 well rowRegister">
                <div class="cardHeader">
                    KAYIT OL
                </div>
                <asp:TextBox ID="txtName" runat="server" class="span4" placeholder="Adı"></asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="rfvName" runat="server" ErrorMessage="Lütfen Adınızı Giriniz"
                    ControlToValidate="txtName" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:TextBox ID="txtSurname" runat="server" class="span4" placeholder="Soyadı"></asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="rfvSurname" runat="server" ErrorMessage="Lütfen Soyadınızı Giriniz"
                    ControlToValidate="txtSurname" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:TextBox ID="txtEmail" runat="server" class="span4" placeholder="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                    ControlToValidate="txtEmail" Display="Dynamic"
                    ErrorMessage="Lütfen Mail Adresinizi Giriniz " ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator ID="rgeEmailId" runat="server"
                    ControlToValidate="txtEmail" Display="Dynamic"
                    ErrorMessage="Lütfen Mail Formatına Uygun Giriniz" ForeColor="Red"
                    SetFocusOnError="True"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>

                <asp:TextBox ID="txtUsername" runat="server" class="span4" placeholder="Kullanıcı Adı"></asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="rfvKAd" runat="server" ErrorMessage="Lütfen Kullanıcı Adını Giriniz"
                    ControlToValidate="txtUsername" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="span4" placeholder="Şifre" MinLength="5" />
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen Şifre Giriniz"
                    ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:TextBox ID="txtConfirmPassword" TextMode="Password" MinLength="5" class="span4" placeholder="Şifre Tekrar" EnableViewState="true" runat="server" />
                <asp:RequiredFieldValidator ID="reqConfirmPassword" ControlToValidate="txtConfirmPassword" ForeColor="#FF3300"
                    SetFocusOnError="True" runat="server" Display="Dynamic" ErrorMessage="Lütfen Şifrenizi Tekrar Giriniz" EnableClientScript="True"></asp:RequiredFieldValidator>
                <%-- Şifre Tekrarı--%>
                <asp:CompareValidator ID="cmpvalTxtConfirmPassword" ControlToCompare="txtPassword"
                    ValidationGroup="Required" Display="Dynamic" ControlToValidate="txtConfirmPassword" ErrorMessage="Şifre Aynı Değil" ForeColor="#FF3300"
                    runat="server" EnableClientScript="true"></asp:CompareValidator>
                <div class="btnRegister">
                    <asp:Button ID="Button1" runat="server" Text="Kayıt Ol" name="submit" class="btn btn-info btn-block" OnClick="Button1_Click" />
                </div>
                <div class="loginLink">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl="loginPage.aspx" runat="server">Mevcut Hesabınız Var mı?</asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

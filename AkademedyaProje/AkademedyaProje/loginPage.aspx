<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="loginPage.aspx.cs" Inherits="AkademedyaProje.LoginPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" class="htmlLogin">
<head runat="server">
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/bootstrap-responsive.css" rel="stylesheet" />
    <script src="~/Scripts/jquery-1.11.1.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" />
    <link href="~/Scripts/applicationStyles.css" rel="stylesheet" />
    <title>Akademedya Project</title>
</head>
<body class="bodyLogin">
    <form id="form1" runat="server">
        <div class="containerLogin container">
            <div class="span4 offset4 well rowLogin">
                <div class="cardHeader">
                    GİRİŞ YAP
                </div>
                <asp:TextBox ID="txtUserName" runat="server" class="span4" placeholder="Kullanıcı Adınızı Giriniz."></asp:TextBox>
                <asp:RequiredFieldValidator
                    ID="rfvUName" runat="server" ErrorMessage="Lütfen Kullanıcı Adını Giriniz"
                    ControlToValidate="txtUserName" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="span4" placeholder="Şifrenizi Giriniz." />
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Lütfen Şifre Giriniz"
                    ControlToValidate="txtPassword" Display="Dynamic" ForeColor="#FF3300"
                    SetFocusOnError="True"></asp:RequiredFieldValidator>

                <div class="rememberMe">
                    Şifre Göster
                    <asp:CheckBox ID="togglePassword" runat="server" /><br />
                    <asp:Label CssClass="lblRemember" ID="Label1" runat="server" Text=" Beni Hatırla"></asp:Label>
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </div>
                <br />
                <div class="btnLogin">
                    <asp:Button ID="Button1" runat="server" Text="Button" name="submit" class="btn btn-info btn-block" OnClick="Button1_Click" />
                </div>
                <asp:HyperLink ID="HyperLink1" NavigateUrl="registerPage.aspx" runat="server">Mevcut Hesabınız Yok mu?</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    var togglePassword = document.querySelector('#togglePassword');
    var password = document.querySelector('#txtPassword');
    togglePassword.addEventListener('click', function (e) {
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
    });
</script>

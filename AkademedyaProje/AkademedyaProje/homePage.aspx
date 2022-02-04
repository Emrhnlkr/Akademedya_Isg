<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeBehind="homePage.aspx.cs" Inherits="AkademedyaProje.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/2.3.2/js/bootstrap.min.js"></script>
    <style>
        .carousel-inner > .item > img,
        .carousel-inner > .item > a > img {
            width: 100%;
            margin: auto;
        }

        .container {
            height: 500px;
            width: 1000px;
        }
    </style>
    <div class="container">
        <div id="myCarousel" class="carousel slide" data-ride="carousel">
            <!-- Responsive -->
            <ol class="carousel-indicators">
                <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                <li data-target="#myCarousel" data-slide-to="1"></li>
                <li data-target="#myCarousel" data-slide-to="2"></li>
            </ol>
            <div class="carousel-inner row" role="listbox">
                <div class="item active" style="height: 500px; width: 1000px;">
                    <img src="https://i.dunya.com/storage/files/images/2020/12/08/isg-AqdZ_cover.jpg" class="img-responsive" style="height: 80%; width: 70%; border-radius: 30px;">
                    <div class="carousel-caption">
                        <p>
                            İş güvenliği; işyerlerinde, çalışanlara sağlıklı çalışma ortam sunmak, 
                            çalışanları çalışma ortamındaki olumsuz etkilerinden korumak, iş ile işçi arasındaki en iyi uyumu sağlamak, 
                            oluşabilecek riskler ile maddi ve manevi zararları tamamen engellemek veya en aza indirgemek, 
                            çalışma ve üretim verimini en üst seviyeye çıkarmaktır. 
                        </p>
                    </div>
                </div>
                <div class="item" style="height: 500px; width: 1000px;">
                    <img src="https://www.calismamevzuati.com/wp-content/uploads/2018/08/isg-uzman%C4%B1-735x400.jpg" class="img-responsive" style="height: 80%; width: 70%; border-radius: 30px;">
                    <div class="carousel-caption">
                        <p>
                            Türkiye’de önceden “işçi sağlığı ve iş güvenliği” olarak bilinen bu kavram, 
                            2012 yılında yayımlanan 6331 sayılı iş sağlığı ve güvenliği kanunu ile “iş sağlığı ve güvenliği” olarak
                            tanımlanmakta olup kısaca İSG olarak adlandırılır.
                        </p>
                    </div>
                </div>
                <div class="item" style="height: 500px; width: 1000px;">
                    <img src="https://kaleosgb.com.tr/wp-content/uploads/2019/11/%C4%B0SG-%C4%B0stanbul.png" class="img-responsive" style="height: 80%; width: 70%; border-radius: 30px;">
                    <div class="carousel-caption">
                        <p>
                            İş güvenliğinin amacı, yapılan işler sırasında veya işler nedeniyle iş kazası yaşanma ihtimalini en aza indirmek, 
                            çalışma ortamı nedeniyle oluşabilecek sağlık sorunlarını önlemek ve meslek hastalığı oluşturabilecek işlerde önlemler almaktır.
                        </p>
                    </div>
                </div>
            </div>
            <!-- Sağ Sol Sayfalama-->
            <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
            </a>
            <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
            </a>
        </div>
    </div>
</asp:Content>

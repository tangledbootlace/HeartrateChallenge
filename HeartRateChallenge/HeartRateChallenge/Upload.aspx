<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="HeartRateChallenge.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function improperType()
        {
            alert("Please only upload files of type .zip. See Instructions for information on how to export your heart rate data.");
        }

        function improperID() {
            alert("Please input proper CompetitorID.")
        }

        function repeatedUpload() {
            alert("You have already uploaded this file and received credit. Please upload new heart rate data.")
        }

    </script>
    <style>
        .checkboxMargin {
            margin-right: 10px;
        }

        .rainbow-text {
            background-image: linear-gradient(to left, violet, indigo, blue, green, yellow, orange, red);
        }

        .divbackground{
            text-align:center;
        }

        .error { 
            background-image: linear-gradient(to left, violet, indigo, blue, green, yellow, orange, red);
            -webkit-background-clip: text;
            -webkit-background-clip: text;
            -moz-background-clip: text;
            background-clip: text;
            color: transparent;
            margin-left: 30px;
            font-family: 'Comic Sans MS';
            font-weight: bold;
            text-align: center;
            font-size: 52px;
        }
    </style>

     <div class="jumbotron" style="text-align: center;">
        <h1><%: Title %></h1>
        <p class="lead">Use this page to upload Heart Rate Data from .zip file.</p>
        <img class="img-responsive center-block" height="500px" width="750px" src="https://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/Broken-lift.jpg" alt="gif image"  />
    </div>   
    <div>
        <asp:DropDownList ID="ddlCompetitorName" runat="server" CssClass="checkboxMargin">
            <asp:ListItem Value="1" Text="Blake" Selected="True" />
            <asp:ListItem Value="2" Text="Kyle" />
            <asp:ListItem Value="3" Text="Liam" />
        </asp:DropDownList>
        
        <asp:CheckBox ID="cbConfirm" runat="server" Checked="false" />        
        <asp:Label ID="lblConfirm" runat="server" Text="I affirm that I have selected my own name from the dropdown list so that Blake does not have to edit the database to fix my mistake." />
    </div>
    <br />
    <div>
        <asp:Image ID="imgError" runat="server" class="img-responsive center-block" Height="400px" Width="600px" src="https://media.giphy.com/media/l2YWoFU3Bmum4yyLC/giphy.gif" alt="inconceivable" Visible="false"/>
    </div>
    <div class="divbackground">
        <asp:Label ID="lblError" CssClass="error" Visible="false" runat="server" />        
    </div>
    <br />
    <asp:Label ID="lblUpload" Class="custom-file-label" runat="server" Text="Upload .Zip File:" />
    <div class="row">
        <div class="col-sm-3">
            <input type="file" class="custom-file-input" id="FileInput" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="FileInputRequiredValidator" ControlToValidate="FileInput" ErrorMessage="Please upload a file."/>
        </div>
        <div class="col-sm-3">
            <asp:Button CssClass="btn btn-primary" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
    </div>
</asp:Content>

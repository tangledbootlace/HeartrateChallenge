<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="HeartRateChallenge.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    
     <div class="jumbotron" style="text-align: center;">
        <h1><%: Title %></h1>
        <p class="lead">Use this page to YEEEEEEEEEEEEET upload Heart Rate Data from .zip file.</p>
        <img class="img-responsive center-block" src="https://media1.tenor.com/images/73db9b409372e6d759f08f51bf114945/tenor.gif?itemid=15734436" alt="gif image"  />
    </div>    
    <div>
        <asp:Label ID="lblCompetitorID" runat="server" Text="Competitor ID:" />
        <asp:TextBox ID="txtCompetitorID" Width="50px" CssClass="form-control" runat="server" />
    </div>
    <br />
    <asp:Label ID="lblUpload" Class="custom-file-label" runat="server" Text="Upload .Zip File:" />
    <div class="row">
        <div class="col-sm-3">

            <input type="file" class="custom-file-input" id="FileInput" runat="server" />
        </div>
        <div class="col-sm-3">
            <asp:Button CssClass="btn btn-primary" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        </div>
        <div>
            <asp:Button ID="btnTest" CssClass="btn btn-secondary" runat="server" Text="Test" OnClick="btnTest_Click" />
        </div>
    </div>
</asp:Content>

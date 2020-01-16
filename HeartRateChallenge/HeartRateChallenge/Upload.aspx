<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="HeartRateChallenge.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Upload Heart Rate Data.</h3>
    <p>Use this page to upload Heart Rate Data from .csv file.</p>
    <div>
        <asp:Label ID="lblCompetitorID" runat="server" Text="Competitor ID:" />
        <asp:TextBox ID="txtCompetitorID" runat="server" />
    </div>
    <div>
        <asp:Label ID="lblUpload" runat="server" Text="Upload .Zip File:" />
        <input type="file" id="FileInput" runat="server" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
    </div>
    
    
</asp:Content>

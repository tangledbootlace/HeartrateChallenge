<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="HeartRateChallenge.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Upload Heart Rate Data.</h3>
    <p>Use this page to upload Heart Rate Data from .csv file.</p>
    <asp:Label ID="lblCompetitorID" runat="server" Text="Competitor ID:" />
    <asp:TextBox ID="txtCompetitorID" runat="server" />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
</asp:Content>

<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HeartRateChallenge._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align: center;">
        <h1>Heart Rate Challenge</h1>
        <p class="lead">Welcome to the Home Page of the February, 2020 Heart Rate Challenge.</p>
        <img src="https://media.giphy.com/media/10hYJnJLGtbUvC/giphy.gif" alt="gif image" />
    </div>
    <div>
        <p class="lead" style="text-align: center;">Current Leaderboard Standings:</p>
    </div>
    <div>
        <asp:GridView ID="gvLeaderboard" CssClass="table table-responsive table-striped table-hover" HeaderStyle-BackColor="#343A40" HeaderStyle-ForeColor="White" HeaderStyle-CssClass="thead-dark" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Rank" HeaderText="Rank" ItemStyle-CssClass="table-responsive" />
                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="table-responsive"/>
                <asp:BoundField DataField="TotalPoints" HeaderText="Total Points" ItemStyle-CssClass="table-responsive" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <div>        
        <h2 style="text-align: center; border: solid; border-color:#343A40; background-color: #343A40; color: white;">Instructions:</h2>
    </div>
    <div class="row" style="text-align: center;">
        <div class="col-md-4">
            <h3>Download Your Heartrate Data:</h3>
            <p>
                Export your heartrate data files over the past week as zipped .csv files from "Flow-Exporter" website.
            </p>
            <p>
                <a class="btn btn-default" href="https://flow-exporter.ddns.net/" target="_blank">Flow-Exporter &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h3>Upload your .csv file on the Upload Page</h3>
            <p>
                Navigate to the Upload page, enter your CompetitorID, upload your .zip file, and click submit. 
            </p>
            <p>
                <a class="btn btn-default" href="/Upload">Upload Page &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h3>View Current Leaderboard Standings:</h3>
            <p>
                After uploading your data, the changes to your Total Points will reflect in the Current Leaderboard Standings above.
            </p>            
        </div>
    </div>

</asp:Content>

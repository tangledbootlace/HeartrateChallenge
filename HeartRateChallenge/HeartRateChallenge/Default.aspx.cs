using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace HeartRateChallenge
{
    public partial class _Default : Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                StartUp();
        }
        protected void StartUp()
        {
            gvLeaderboard.Dispose();
            SqlDataSource SqlDataSource1 = new SqlDataSource();
            SqlDataSource1.ID = "SqlDataSource1";
            this.Page.Controls.Add(SqlDataSource1);
            SqlDataSource1.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
            SqlDataSource1.SelectCommand = "SELECT ROW_NUMBER() OVER (ORDER BY [TotalPoints] DESC) AS Rank, [Name], [TotalPoints] FROM [dbo].[tbl_Leaderboard] ORDER BY [TotalPoints] DESC";
            gvLeaderboard.DataSource = SqlDataSource1;
            gvLeaderboard.DataBind();
            gvLeaderboard.HeaderRow.TableSection = TableRowSection.TableHeader;

            //SELECT ROW_NUMBER() OVER (ORDER BY [Total Points] DESC) AS Rank, 
            //SqlConnection sqlCon = new SqlConnection(@"Data Source=heartratechallenge.database.windows.net;Initial Catalog=HeartRateChallenge;User ID=tangledbootlace;Password=MarryHelen2020!");
            //SqlCommand sqlCmd = new SqlCommand("SELECT * FROM [tbl_Leaderboard] ORDER BY [Total Points] DESC", sqlCon);

            //sqlCon.Open();
            //SqlDataReader reader = sqlCmd.ExecuteReader();
            //gvLeaderboard.DataSource = reader;
            //gvLeaderboard.DataBind();
            //sqlCon.Close();
        }
    }
}
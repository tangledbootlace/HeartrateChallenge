using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace HeartRateChallenge
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dtHeartRateZones = new DataTable();
            dtHeartRateZones = GetHeartRateZones();
            
            //Receive zip upload
            //foreach excel file in .zip
            //startat Cell C4 then loop downward to end
            var heartrate = 136; // dummy variable but will be excelreader[intRowNum, 4]
            var UserRange1LB = 104;
            //UserRange1LB = dtHeartRateZones[Zone1LowerBound];
            var UserRange1UB = 114;
            var UserRange2LB = 114;
            var UserRange2UB = 133;
            var UserRange3LB = 133;
            var UserRange3UB = 152;
            var UserRange4LB = 152;
            var UserRange4UB = 172;
            var UserRange5LB = 172;
            var UserRange5UB = 220; //needs to be higher than max heart rate
            int Range1Seconds, Range2Seconds, Range3Seconds, Range4Seconds, Range5Seconds;
            Range1Seconds = Range2Seconds = Range3Seconds = Range4Seconds = Range5Seconds = 0; //initialize seconds to 0

            if (heartrate >= UserRange1LB && heartrate < UserRange1UB)
                Range1Seconds++;
            else if (heartrate >= UserRange2LB && heartrate < UserRange2UB)
                Range2Seconds++;
            else if (heartrate >= UserRange3LB && heartrate < UserRange3UB)
                Range3Seconds++;
            else if (heartrate >= UserRange4LB && heartrate < UserRange4UB)
                Range4Seconds++;
            else if (heartrate >= UserRange5LB && heartrate < UserRange5UB)
                Range5Seconds++;

            //Convert seconds to points

        }

        protected DataTable GetHeartRateZones()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"SELECT * FROM [dbo].[tbl_CompetitorHeartRates] WHERE [CompetitorID] = {txtCompetitorID.Text}", sqlCon);

            sqlCon.Open();
            using (SqlDataAdapter a = new SqlDataAdapter(sqlCmd))
            {
                a.Fill(dt);
            }
            sqlCon.Close();
            return dt;
        }

    }
}
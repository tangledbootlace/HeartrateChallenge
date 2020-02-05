using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;

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
            
            ProcessZip();
        }
        
        protected void ProcessZip()
        {
            //TODO: Check for proper competitorID or make competitorID control a dropdown
            string FileName = System.IO.Path.GetFileName(FileInput.PostedFile.FileName);
            if (int.TryParse(ddlCompetitorName.SelectedValue, out var id))
            {
                if (FileName.EndsWith(".zip"))
                {
                    if (!CheckFileRecords(id, FileName))
                    {
                        string SaveLocation = Server.MapPath("~/App_Data/") + FileName;

                        try
                        {
                            FileInput.PostedFile.SaveAs(SaveLocation);
                            Response.Write("The file has been uploaded.");
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Error: " + ex.Message);
                        }

                        DataTable dtHeartRateZones = new DataTable();
                        dtHeartRateZones = GetHeartRateZones();
                        DataTable dtPointsPerMinute = new DataTable();
                        dtPointsPerMinute = GetPointsPerMinute();

                        int heartrate; //probably make class for this
                        int TotalPoints = 0;
                        var UserRange1LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone1LowerBound"];
                        var UserRange1UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone1UpperBound"];
                        var UserRange2LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone2LowerBound"];
                        var UserRange2UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone2UpperBound"];
                        var UserRange3LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone3LowerBound"];
                        var UserRange3UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone3UpperBound"];
                        var UserRange4LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone4LowerBound"];
                        var UserRange4UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone4UpperBound"];
                        var UserRange5LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone5LowerBound"];
                        var UserRange5UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(id)).FirstOrDefault()["Zone5UpperBound"];


                        using (ZipArchive archive = ZipFile.OpenRead(SaveLocation))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                int Range1Seconds, Range2Seconds, Range3Seconds, Range4Seconds, Range5Seconds, Range1Minutes, Range2Minutes, Range3Minutes, Range4Minutes, Range5Minutes, Range1Points, Range2Points, Range3Points, Range4Points, Range5Points;


                                Range1Seconds = Range2Seconds = Range3Seconds = Range4Seconds = Range5Seconds = 0; //initialize seconds to 0

                                Range1Minutes = Range2Minutes = Range3Minutes = Range4Minutes = Range5Minutes = 0;

                                Range1Points = Range2Points = Range3Points = Range4Points = Range5Points = 0;

                                if (entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                                {
                                    try
                                    {
                                        DataTable dt = new DataTable();
                                        using (StreamReader sr = new StreamReader(entry.Open())) //build dt from .csv
                                        {
                                            sr.ReadLine();
                                            sr.ReadLine();
                                            string[] headers = sr.ReadLine().Split(',');
                                            foreach (string header in headers)
                                            {
                                                dt.Columns.Add(header);
                                            }
                                            while (!sr.EndOfStream)
                                            {
                                                string[] rows = sr.ReadLine().Split(',');
                                                DataRow dr = dt.NewRow();
                                                for (int i = 0; i < headers.Length; i++)
                                                {
                                                    dr[i] = rows[i];
                                                }
                                                heartrate = int.Parse(dr[2].ToString());
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
                                                dt.Rows.Add(dr);
                                            }
                                        }

                                        //all done with csv, calculate points.
                                        Range1Minutes = (Range1Seconds / 60);
                                        Range2Minutes = (Range2Seconds / 60);
                                        Range3Minutes = (Range3Seconds / 60);
                                        Range4Minutes = (Range4Seconds / 60);
                                        Range5Minutes = (Range5Seconds / 60);

                                        Range1Points = Range1Minutes * ((int)dtPointsPerMinute.AsEnumerable().Where(x => x.Field<int>("HeartRateZone").Equals(1)).FirstOrDefault()["Points"]);
                                        Range2Points = Range2Minutes * ((int)dtPointsPerMinute.AsEnumerable().Where(x => x.Field<int>("HeartRateZone").Equals(2)).FirstOrDefault()["Points"]);
                                        Range3Points = Range3Minutes * ((int)dtPointsPerMinute.AsEnumerable().Where(x => x.Field<int>("HeartRateZone").Equals(3)).FirstOrDefault()["Points"]);
                                        Range4Points = Range4Minutes * ((int)dtPointsPerMinute.AsEnumerable().Where(x => x.Field<int>("HeartRateZone").Equals(4)).FirstOrDefault()["Points"]);
                                        Range5Points = Range5Minutes * ((int)dtPointsPerMinute.AsEnumerable().Where(x => x.Field<int>("HeartRateZone").Equals(5)).FirstOrDefault()["Points"]);

                                        TotalPoints += (Range1Points + Range2Points + Range3Points + Range4Points + Range5Points);

                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Write("Processing Error: " + ex.Message);
                                    }
                                }
                            }
                        }
                        ////Write final total
                        AddTotalPoints(TotalPoints);
                        RecordFileName(FileName, TotalPoints);

                        //Delete CSV from Server Data
                        File.Delete(SaveLocation);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(GetType(), "alert", "repeatedUpload();", true);
                    }
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "improperType();", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "alert", "improperID();", true);
            }
        }
        protected DataTable GetHeartRateZones()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"SELECT * FROM [dbo].[tbl_CompetitorHeartRateZones] WHERE [CompetitorID] = @CompetitorID", sqlCon);
            sqlCmd.Parameters.AddWithValue("@CompetitorID", ddlCompetitorName.SelectedValue);

            try
            {
                sqlCon.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(sqlCmd))
                {
                    a.Fill(dt);
                }
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
            return dt;
        }

        protected DataTable GetPointsPerMinute()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"SELECT * FROM [dbo].[tbl_PointsPerMinute]", sqlCon);

            try
            {
                sqlCon.Open();
                using (SqlDataAdapter a = new SqlDataAdapter(sqlCmd))
                {
                    a.Fill(dt);
                }
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.Message);
            }
            return dt;

        }

        protected void AddTotalPoints(int TP)
        {
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"UPDATE [tbl_Leaderboard] SET [TotalPoints] = [TotalPoints] + @TotalPoints WHERE [CompetitorID] = @CompetitorID", sqlCon);
            sqlCmd.Parameters.AddWithValue("@TotalPoints", TP);
            sqlCmd.Parameters.AddWithValue("@CompetitorID", ddlCompetitorName.SelectedValue);

            try
            {
                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error During AddTotalPoints: " + ex.Message);
            }
        }

        protected void RecordFileName(string FileName, int TP)
        {
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"INSERT INTO [tbl_UploadHistory] (CompetitorID, FileName, UploadDate, PointChange) VALUES (@CompetitorID, @FileName, @UploadDate, @TotalPoints)", sqlCon);
            sqlCmd.Parameters.AddWithValue("@CompetitorID", int.Parse(ddlCompetitorName.SelectedValue));
            sqlCmd.Parameters.AddWithValue("@FileName", FileName);
            sqlCmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);
            sqlCmd.Parameters.AddWithValue("@TotalPoints", TP);

            try
            {
                sqlCon.Open();
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error During RecordFileName: " + ex.Message);
            }
        }

        protected bool CheckFileRecords(int CompetitorID, string FileName)
        {
            bool Record = false;
            string Exists = "";
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"SELECT [FileName] FROM [tbl_UploadHistory] WHERE [CompetitorID] = @CompetitorID AND [FileName] = @FileName", sqlCon);
            sqlCmd.Parameters.AddWithValue("@CompetitorID", CompetitorID);
            sqlCmd.Parameters.AddWithValue("@FileName", FileName);

            try
            {
                sqlCon.Open();
                var rdr = sqlCmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    Exists = rdr.Read().ToString();
                }

                //TODO: STORE RETURNED RECORD FILENAME IN STRING
                
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error During RecordFileName: " + ex.Message);
            }

            if (Exists == "True")
                Record = true;

            return Record;
        }
    }
}
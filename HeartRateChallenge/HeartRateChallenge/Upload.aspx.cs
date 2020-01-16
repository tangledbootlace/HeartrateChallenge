using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using OfficeOpenXml;
using System.Globalization;

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
            GetUpload();
            ProcessZip();




            //using (GZipStream archive = GZipStream) ;
            //unpackage zip?
            //foreach excel file in .zip
            //startat Cell C4 then loop downward to end
            //var heartrate = 136; // dummy variable but will be excelreader[intRowNum, 4]
            //var UserRange1LB = 104;
            //UserRange1LB = dtHeartRateZones[Zone1LowerBound];
            //var UserRange1UB = 114;
            //var UserRange2LB = 114;
            //var UserRange2UB = 133;
            //var UserRange3LB = 133;
            //var UserRange3UB = 152;
            //var UserRange4LB = 152;
            //var UserRange4UB = 172;
            //var UserRange5LB = 172;
            //var UserRange5UB = 220; //needs to be higher than max heart rate
            //int Range1Seconds, Range2Seconds, Range3Seconds, Range4Seconds, Range5Seconds;
            //Range1Seconds = Range2Seconds = Range3Seconds = Range4Seconds = Range5Seconds = 0; //initialize seconds to 0

            //if (heartrate >= UserRange1LB && heartrate < UserRange1UB)
            //    Range1Seconds++;
            //else if (heartrate >= UserRange2LB && heartrate < UserRange2UB)
            //    Range2Seconds++;
            //else if (heartrate >= UserRange3LB && heartrate < UserRange3UB)
            //    Range3Seconds++;
            //else if (heartrate >= UserRange4LB && heartrate < UserRange4UB)
            //    Range4Seconds++;
            //else if (heartrate >= UserRange5LB && heartrate < UserRange5UB)
            //    Range5Seconds++;

            //Convert seconds to points

        }

        protected void GetUpload()
        {
            if ((FileInput.PostedFile != null) && FileInput.PostedFile.ContentLength > 0)
            {
                string FileName = System.IO.Path.GetFileName(FileInput.PostedFile.FileName);
                string SaveLocation = Server.MapPath("Data") + "\\" + FileName;
                try
                {
                    FileInput.PostedFile.SaveAs(SaveLocation);
                    Response.Write("The file has been uploaded.\n");
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
            else
            {
                Response.Write("Please select a file to upload.");
            }
        }

        protected void ProcessZip()
        {
            string FileName = System.IO.Path.GetFileName(FileInput.PostedFile.FileName);
            string SaveLocation = Server.MapPath("Data") + "\\" + FileName;
            try
            {
                FileInput.PostedFile.SaveAs(SaveLocation);
            }
            catch (Exception ex)
            {
                Response.Write("Error during SaveAs: " + ex.Message);
            }

            DataTable dtHeartRateZones = new DataTable();
            dtHeartRateZones = GetHeartRateZones();

            int heartrate; //probably make class for this
            int TotalPoints = 0;
            var UserRange1LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone1LowerBound"];
            var UserRange1UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone1UpperBound"];
            var UserRange2LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone2LowerBound"];
            var UserRange2UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone2UpperBound"];
            var UserRange3LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone3LowerBound"];
            var UserRange3UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone3UpperBound"];
            var UserRange4LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone4LowerBound"];
            var UserRange4UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone4UpperBound"];
            var UserRange5LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone5LowerBound"];
            var UserRange5UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(int.Parse(txtCompetitorID.Text.Trim()))).FirstOrDefault()["Zone5UpperBound"];
                        

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

                            Range1Points = (Range1Minutes * 1);
                            Range2Points = (Range2Minutes * 2);
                            Range3Points = (Range3Minutes * 4);
                            Range4Points = (Range4Minutes * 6);
                            Range5Points = (Range5Minutes * 9);

                            TotalPoints += (Range1Points + Range2Points + Range3Points + Range4Points + Range5Points);

                            ////for reference
                            //Response.Write($"POINTS: {Range1Points.ToString()}, {Range2Points.ToString()}, {Range3Points.ToString()}, {Range4Points.ToString()}, {Range5Points.ToString()}");
                            
                            //NEEDS TO DELETE FILE FROM DATA AFTER PROCESSING
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

            //Delete CSV from Server Data
            File.Delete(SaveLocation);
        }
        protected DataTable GetHeartRateZones()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            SqlCommand sqlCmd = new SqlCommand($"SELECT * FROM [dbo].[tbl_CompetitorHeartRateZones] WHERE [CompetitorID] = @CompetitorID", sqlCon);
            sqlCmd.Parameters.AddWithValue("@CompetitorID", txtCompetitorID.Text);

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
            sqlCmd.Parameters.AddWithValue("@CompetitorID", txtCompetitorID.Text);

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
    }
}
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
            if (cbConfirm.Checked)
            {
                lblError.Visible = false;
                ProcessZip();
            }
            else
            {                
                imgError.Visible = true;
                lblError.Text = "You fool!<br/>You fell victim to one of the classic blunders!<br/>You did not confirm that you know what your own name is!";
                lblError.Visible = true;
            }
            
        }
        
        protected void ProcessZip()
        {
            //TODO: Add functionality to just upload .csv
            var fileName = Path.GetFileName(FileInput.PostedFile.FileName);
            if (int.TryParse(ddlCompetitorName.SelectedValue, out var id))
            {
                if (fileName.EndsWith(".zip"))
                {
                    var saveLocation = Server.MapPath("~/App_Data/") + fileName;
                    if (!CheckFileRecords(id, fileName))
                    {
                        try
                        {
                            FileInput.PostedFile.SaveAs(saveLocation);                            
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Error: " + ex.Message);
                        }

                        var dtHeartRateZones = GetHeartRateZones();
                        var dtPointsPerMinute = GetPointsPerMinute();

                        var totalPoints = 0;
                        var userRange1Lb =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone1LowerBound"];
                        var userRange1Ub =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone1UpperBound"];
                        var userRange2Lb =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone2LowerBound"];
                        var userRange2Ub =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone2UpperBound"];
                        var userRange3Lb =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone3LowerBound"];
                        var userRange3Ub =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone3UpperBound"];
                        var userRange4Lb =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone4LowerBound"];
                        var userRange4Ub =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone4UpperBound"];
                        var userRange5Lb =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone5LowerBound"];
                        var userRange5Ub =
                            (int) dtHeartRateZones.AsEnumerable().FirstOrDefault(x =>
                                DataRowExtensions.Field<int>(x, "CompetitorID").Equals(id))?["Zone5UpperBound"];


                        using (var archive = ZipFile.OpenRead(saveLocation))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                int range2Seconds, range3Seconds, range4Seconds, range5Seconds;

                                var range1Seconds = range2Seconds = range3Seconds = range4Seconds = range5Seconds = 0;

                                if (!entry.FullName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)) continue;
                                try
                                {
                                    var dt = new DataTable();
                                    using (var sr = new StreamReader(entry.Open())) //build dt from .csv
                                    {
                                        sr.ReadLine();
                                        sr.ReadLine();
                                        var headers = sr.ReadLine().Split(',');
                                        foreach (var header in headers)
                                        {
                                            dt.Columns.Add(header);
                                        }
                                        while (!sr.EndOfStream)
                                        {
                                            var rows = sr.ReadLine().Split(',');
                                            var dr = dt.NewRow();
                                            for (var i = 0; i < headers.Length; i++)
                                            {
                                                dr[i] = rows[i];
                                            }

                                            var heartrate = int.TryParse(dr[2].ToString(), out var s) ? s : 0;
                                            if (heartrate >= userRange1Lb && heartrate <= userRange1Ub)
                                                range1Seconds++;
                                            else if (heartrate >= userRange2Lb && heartrate <= userRange2Ub)
                                                range2Seconds++;
                                            else if (heartrate >= userRange3Lb && heartrate <= userRange3Ub)
                                                range3Seconds++;
                                            else if (heartrate >= userRange4Lb && heartrate <= userRange4Ub)
                                                range4Seconds++;
                                            else if (heartrate >= userRange5Lb && heartrate <= userRange5Ub)
                                                range5Seconds++;
                                            dt.Rows.Add(dr);
                                        }
                                    }

                                    //all done with csv, calculate points.
                                    var range1Minutes = decimal.Divide(range1Seconds, 60);
                                    var range2Minutes = decimal.Divide(range2Seconds, 60);
                                    var range3Minutes = decimal.Divide(range3Seconds, 60);
                                    var range4Minutes = decimal.Divide(range4Seconds, 60);
                                    var range5Minutes = decimal.Divide(range5Seconds, 60);

                                    var range1Points = range1Minutes * ((int)dtPointsPerMinute.AsEnumerable().FirstOrDefault(x => x.Field<int>("HeartRateZone").Equals(1))["Points"]);
                                    var range2Points = range2Minutes * ((int)dtPointsPerMinute.AsEnumerable().FirstOrDefault(x => x.Field<int>("HeartRateZone").Equals(2))["Points"]);
                                    var range3Points = range3Minutes * ((int)dtPointsPerMinute.AsEnumerable().FirstOrDefault(x => x.Field<int>("HeartRateZone").Equals(3))["Points"]);
                                    var range4Points = range4Minutes * ((int)dtPointsPerMinute.AsEnumerable().FirstOrDefault(x => x.Field<int>("HeartRateZone").Equals(4))["Points"]);
                                    var range5Points = range5Minutes * ((int)dtPointsPerMinute.AsEnumerable().FirstOrDefault(x => x.Field<int>("HeartRateZone").Equals(5))["Points"]);

                                    var pointsToAdd = int.Parse(Math.Round(range1Points + range2Points + range3Points + range4Points + range5Points).ToString());
                                    totalPoints += pointsToAdd;                                        

                                    RecordFileName(entry.Name, pointsToAdd);
                                        

                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Processing Error: " + ex.Message + " ");
                                }
                            }
                        }
                        ////Write final total
                        UpdateTotalPoints();
                        Response.Write($"The file has been uploaded. {totalPoints.ToString()} points have been added to your total score.");
                        //RecordFileName(FileName, TotalPoints);

                        //Delete CSV from Server Data
                        File.Delete(saveLocation);
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
            var dt = new DataTable();
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            const string strSp = "dbo.sp_SelectHeartRateZones";
            var sqlCmd = new SqlCommand(strSp, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
            sqlCmd.Parameters.AddWithValue("@CompetitorID", ddlCompetitorName.SelectedValue);

            try
            {
                sqlCon.Open();
                using (var a = new SqlDataAdapter(sqlCmd))
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
            var dt = new DataTable();
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            const string strSp = "dbo.sp_SelectPointsPerMinute";
            var sqlCmd = new SqlCommand(strSp, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };

            try
            {
                sqlCon.Open();
                using (var a = new SqlDataAdapter(sqlCmd))
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

        protected void UpdateTotalPoints()
        {
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            const string strSp = "dbo.sp_UpdateTotalPoints";
            var sqlCmd = new SqlCommand(strSp, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
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

        protected void RecordFileName(string fileName, int pointChange)
        {
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            const string strSp = "dbo.sp_InsertUploadHistory";
            var sqlCmd = new SqlCommand(strSp, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
            sqlCmd.Parameters.AddWithValue("@CompetitorID", int.Parse(ddlCompetitorName.SelectedValue));
            sqlCmd.Parameters.AddWithValue("@FileName", fileName);
            sqlCmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);
            sqlCmd.Parameters.AddWithValue("@PointChange", pointChange);

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

        protected bool CheckFileRecords(int competitorId, string fileName)
        {
            var record = false;
            var exists = "";
            var sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ConnectionString);
            const string strSp = "dbo.sp_ValidateUploadHistory";
            var sqlCmd = new SqlCommand(strSp, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
            sqlCmd.Parameters.AddWithValue("@CompetitorID", competitorId);
            sqlCmd.Parameters.AddWithValue("@FileName", fileName);

            try
            {
                sqlCon.Open();
                var rdr = sqlCmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    exists = rdr.Read().ToString();
                }

                //TODO: STORE RETURNED RECORD FILENAME IN STRING
                
                sqlCon.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error During RecordFileName: " + ex.Message);
            }

            if (exists == "True")
                record = true;

            return record;
        }
    }
}
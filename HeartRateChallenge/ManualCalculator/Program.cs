﻿using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.IO.Compression;

namespace ManualCalculator
{
    class Program
    {
        static readonly string strInputFolder = ConfigurationManager.AppSettings["InputFolder"];
        static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        static readonly SqlConnection sqlCon = new SqlConnection(connStr);
        static string strInputFileName = "";
        static bool blProcessingError = false;
        static int intFileCnt = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("***START Manual HR Point Calculator");
            Console.WriteLine();
            ProcessDirectory();
            Console.WriteLine("***END Manual HR Point Calculator");
            Console.ReadKey(true);

        }

        private static void ProcessDirectory()
        {
            //TODO: Check for proper competitorID or make competitorID control a dropdown
            DirectoryInfo diInputFolder = new DirectoryInfo(strInputFolder);
            foreach (FileInfo fi in diInputFolder.GetFiles())
            {
                if (fi.Extension == ".zip" && !blProcessingError)
                {
                    intFileCnt++;
                    Console.WriteLine($"File Found: {fi.Name.ToString()}");
                    strInputFileName = fi.Name.ToString();
                    Console.WriteLine("Performing Calculation. . .");
                    ProcessZip(strInputFolder + fi.Name.ToString());
                }
            }
        }

        private static void ProcessZip(string strFileName)
        {
            int CompetitorID = int.Parse(ConfigurationManager.AppSettings["CompetitorID"]);
            DataTable dtHeartRateZones = new DataTable();
            dtHeartRateZones = GetHeartRateZones(CompetitorID);
            DataTable dtPointsPerMinute = new DataTable();
            dtPointsPerMinute = GetPointsPerMinute();

            int heartrate; //probably make class for this
            int TotalPoints = 0;
            var UserRange1LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone1LowerBound"];
            var UserRange1UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone1UpperBound"];
            var UserRange2LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone2LowerBound"];
            var UserRange2UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone2UpperBound"];
            var UserRange3LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone3LowerBound"];
            var UserRange3UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone3UpperBound"];
            var UserRange4LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone4LowerBound"];
            var UserRange4UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone4UpperBound"];
            var UserRange5LB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone5LowerBound"];
            var UserRange5UB = (int)dtHeartRateZones.AsEnumerable().Where(x => x.Field<int>("CompetitorID").Equals(CompetitorID)).FirstOrDefault()["Zone5UpperBound"];

            using (ZipArchive archive = ZipFile.OpenRead(strFileName))
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

                            Console.WriteLine($"{entry.Name.ToString()} Calculated Points:");
                            Console.WriteLine($"Range 1 Points: {Range1Points.ToString()}");
                            Console.WriteLine($"Range 2 Points: {Range2Points.ToString()}");
                            Console.WriteLine($"Range 3 Points: {Range3Points.ToString()}");
                            Console.WriteLine($"Range 4 Points: {Range4Points.ToString()}");
                            Console.WriteLine($"Range 5 Points: {Range5Points.ToString()}");
                            Console.WriteLine($"Total Points: {TotalPoints.ToString()}");
                            Console.WriteLine();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Processing Error: " + ex.Message);
                        }
                    }
                }
            }

        }

        private static DataTable GetHeartRateZones(int CompetitorID)
        {
            DataTable dt = new DataTable();
            string strSP = "dbo.sp_SelectHeartRateZones";
            SqlCommand sqlCmd = new SqlCommand(strSP, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };
            sqlCmd.Parameters.AddWithValue("@CompetitorID", CompetitorID);

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
                Console.WriteLine("Error: " + ex.Message);
            }
            return dt;
        }

        private static DataTable GetPointsPerMinute()
        {
            DataTable dt = new DataTable();
            string strSP = "dbo.sp_SelectPointsPerMinute";
            SqlCommand sqlCmd = new SqlCommand(strSP, sqlCon)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 0
            };

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
                Console.WriteLine("Error: " + ex.Message);
            }
            return dt;

        }
    }
}

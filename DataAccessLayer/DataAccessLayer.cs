using System.Data;
using AutoSendEmail.Constants;
using AutoSendEmail.Models;
using Microsoft.Data.SqlClient;

namespace AutoSendEmail.DataAccessLayer
{
    public class DataAccessLayer
    {
        #region KE Tracking by Time
        public ChartModel GetKETrackingByTime(string startTime, string endTime)
        {
            ChartModel chart = new ChartModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppConstants.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(AppConstants.KETrackingByTime, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@startTime", SqlDbType.VarChar).Value = startTime;
                        command.Parameters.AddWithValue("@endTime", SqlDbType.VarChar).Value = endTime;
                        command.Parameters.AddWithValue("@familyName", SqlDbType.VarChar).Value = "empty";
                        command.Parameters.AddWithValue("@productId", SqlDbType.VarChar).Value = "empty";
                        command.CommandTimeout = 0;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                chart.DataLabel.Add(reader["Time"].ToString().Replace(" 12:00:00 AM", ""));
                                chart.DataValue.Add(Convert.IsDBNull(reader["KE"]) ? 0.0 : Convert.ToDouble(reader["KE"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.InnerException} Function GetKETrackingByTime: " + ex.Message);
            }
            return chart;
        }
        #endregion

        #region KE Tracking Target
        public DataTable GetKETrackingTarget(string startTime, string endTime)
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppConstants.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(AppConstants.KETrackingTarget, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@startTime", SqlDbType.VarChar).Value = startTime;
                        command.Parameters.AddWithValue("@endTime", SqlDbType.VarChar).Value = endTime;
                        command.CommandTimeout = 0;
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(table);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception {ex.InnerException} Function GetKETrackingTarget: " + ex.Message);
            }
            return table;
        }
        #endregion

        #region KE Tracking Sub Quantity
        public List<KETrackingSubQuantity> GetKETrackingSubQuantity(string startTime, string endTime)
        {
            List<KETrackingSubQuantity> listQuantity = new List<KETrackingSubQuantity>();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppConstants.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(AppConstants.KETrackingSub, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@startTime", SqlDbType.VarChar).Value = startTime;
                        command.Parameters.AddWithValue("@endTime", SqlDbType.VarChar).Value = endTime;
                        command.CommandTimeout = 0;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listQuantity.Add(new KETrackingSubQuantity
                                {
                                    AssyName = reader["Sub_Reference"].ToString() ?? "",
                                    SubRequiredFG = reader["Total_SUB_required"].ToString() ?? "",
                                    SumLDS = reader["Total_SUB_record"].ToString() ?? "",
                                    SubQuantityGap = reader["Total_Gap"].ToString() ?? "",
                                    Deviation = reader["Deviation"].ToString() ?? ""
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Function GetKETrackingSubQuantity: " + ex.Message);
            }
            return listQuantity;
        }
        #endregion
    }
}
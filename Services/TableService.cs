using System.Data;
using System.Text;
using AutoSendEmail.DataAccessLayer;
using AutoSendEmail.Models;

namespace AutoSendEmail.Services
{
    public class TableService
    {
        public static StringBuilder CreateTableTarget(string startTime, string endTime)
        {
            DataAccessLayer.DataAccessLayer accessLayer = new DataAccessLayer.DataAccessLayer();

            DataTable dt = accessLayer.GetKETrackingTarget(startTime: startTime, endTime: endTime);

            StringBuilder htmlTable = new StringBuilder();

            htmlTable.AppendLine("<style>");
            htmlTable.AppendLine("table {table-layout: fixed;}");
            htmlTable.AppendLine("thead {background-color: green; color: white;}");
            htmlTable.AppendLine("th, td { border: 1px solid #dddddd; text-align: center; padding: 10px; width: 300px; max-width: 350px;}");
            htmlTable.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            htmlTable.AppendLine("</style>");

            htmlTable.AppendLine("<table>");
            htmlTable.AppendLine("<thead>");
            htmlTable.AppendLine("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                htmlTable.AppendLine($"<th>{column.ColumnName}</th>");
            }
            htmlTable.AppendLine("</tr>");
            htmlTable.AppendLine("</thead>");
            htmlTable.AppendLine("<tbody>");
            foreach (DataRow row in dt.Rows)
            {
                htmlTable.AppendLine("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    if (dt.Columns.IndexOf(column) >= 1 && dt.Columns.IndexOf(column) < dt.Columns.Count - 3)
                    {
                        if (!string.IsNullOrEmpty(row[column].ToString()))
                        {
                            if (Convert.ToDouble(row[column]) < Convert.ToDouble(row["Target KE"]))
                            {
                                htmlTable.AppendLine($"<td style = 'color: red;'>{row[column]}</td>");
                            }
                            else
                            {
                                htmlTable.AppendLine($"<td style = 'color: green;'>{row[column]}</td>");
                            }
                        }
                        else
                        {
                            htmlTable.AppendLine($"<td></td>");
                        }
                    }
                    else
                    {
                        if (column.ColumnName == "Total Grand" || column.ColumnName == "Total OT" || column.ColumnName == "Target KE")
                        {
                            if (column.ColumnName == "Target KE")
                            {
                                htmlTable.AppendLine($"<td style='font-weight: bold'>{Convert.ToDouble(row[column]).ToString("N0")}</td>");
                            }
                            else
                            {
                                htmlTable.AppendLine($"<td style='font-weight: bold'>{row[column]}</td>");
                            }
                        }
                        else
                        {
                            htmlTable.AppendLine($"<td style='text-align: left;'>{row[column]}</td>");
                        }
                    }
                }
                htmlTable.AppendLine("</tr>");
            }
            htmlTable.AppendLine("</tbody>");
            htmlTable.AppendLine("</table>");

            return htmlTable;
        }

        public static StringBuilder CreateTableSubQuantity(string startTime, string endTime)
        {
            DataAccessLayer.DataAccessLayer accessLayer = new DataAccessLayer.DataAccessLayer();
            List<KETrackingSubQuantity> models = accessLayer.GetKETrackingSubQuantity(startTime: startTime, endTime: endTime);
            StringBuilder htmlTable = new StringBuilder();

            htmlTable.AppendLine("<style>");
            htmlTable.AppendLine("table {table-layout: fixed;}");
            htmlTable.AppendLine("thead {background-color: green; color: white;}");
            htmlTable.AppendLine("th, td { border: 1px solid #dddddd; text-align: center; width: 300px; max-width: 350px;}");
            htmlTable.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            htmlTable.AppendLine("</style>");

            htmlTable.AppendLine("<table>");
            htmlTable.AppendLine("<thead>");
            htmlTable.AppendLine("<tr>");
            htmlTable.AppendLine("<th>Sub Assy Name</th>");
            htmlTable.AppendLine("<th>Sub Required by FG</th>");
            htmlTable.AppendLine("<th>Sum of Sub key in LDS</th>");
            htmlTable.AppendLine("<th>Sub Quantity Gap</th>");
            htmlTable.AppendLine("<th>Deviation</th>");
            htmlTable.AppendLine("</tr>");
            htmlTable.AppendLine("</thead>");

            htmlTable.AppendLine("<tbody>");
            foreach (KETrackingSubQuantity item in models)
            {
                string textColor = (Convert.ToDouble(item.Deviation) > 20) ? "red" : "black";
                htmlTable.AppendLine("<tr>");
                htmlTable.AppendLine($"<td style='text-align: left;'>{item.AssyName}</td>");
                htmlTable.AppendLine($"<td>{Convert.ToDouble(item.SubRequiredFG).ToString("N0")}</td>");
                htmlTable.AppendLine($"<td>{Convert.ToDouble(item.SumLDS).ToString("N0")}</td>");
                htmlTable.AppendLine($"<td style='color: {textColor}'>{Convert.ToDouble(item.SubQuantityGap).ToString("N0")}</td>");
                htmlTable.AppendLine($"<td style='color: {textColor}'>{Convert.ToDouble(item.Deviation).ToString("N0")}%</td>");
                htmlTable.AppendLine("</tr>");
            }
            htmlTable.AppendLine("</tbody>");
            htmlTable.AppendLine("</table>");

            return htmlTable;
        }
    }
}
using AutoSendEmail.Models;
using ScottPlot;

namespace AutoSendEmail.Services
{
    public class ChartService
    {
        public static void CreateChart(string startTime, string endTime)
        {
            DataAccessLayer.DataAccessLayer accessLayer = new DataAccessLayer.DataAccessLayer();
            ChartModel chartModel = accessLayer.GetKETrackingByTime(startTime: startTime, endTime: endTime);

            Plot myPlot = new();
            double[] values = chartModel.DataValue.ToArray();

            Tick[] ticks = new Tick[chartModel.DataLabel.Count];

            for (int i = 0; i < chartModel.DataLabel.Count; i++)
            {
                ticks[i] = new Tick(i, chartModel.DataLabel[i]);
            }

            myPlot.Title("KE Tracking");
            myPlot.Grid.MajorLineColor = Colors.Black.WithOpacity(.1);
            myPlot.Axes.Bottom.FrameLineStyle.Width = 0;
            myPlot.Axes.Top.FrameLineStyle.Width = 0;
            myPlot.Axes.Left.FrameLineStyle.Width = 0;
            myPlot.Axes.Right.FrameLineStyle.Width = 0;
            myPlot.Axes.Left.Label.Text = "Percent";
            myPlot.Axes.Bottom.Label.Text = "Date";
            
            myPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks);
            // myPlot.Axes.Bottom.MajorTickStyle.Length = 0;

            var barPlot = myPlot.Add.Bars(values);
            barPlot.Color = Colors.LightGreen;

            foreach (var bar in barPlot.Bars)
            {
                bar.Label = bar.Value.ToString();
            }

            
            barPlot.ValueLabelStyle.Bold = false;
            barPlot.ValueLabelStyle.FontSize = 14;
            barPlot.ValueLabelStyle.Alignment = Alignment.MiddleCenter;

            myPlot.Axes.SetLimitsY(0, 100);
            myPlot.Axes.Margins(bottom: 0);

            myPlot.SavePng("Assets/chart.png", 1500, 450);
        }
    }
}
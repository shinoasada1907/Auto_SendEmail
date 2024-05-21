namespace AutoSendEmail.Models
{
    public class ChartModel
    {
        public List<string> DataLabel { get; set; }
        public List<double> DataValue { get; set; }

        public ChartModel()
        {
            DataLabel = new List<string>();
            DataValue = new List<double>();
        }
    }
}
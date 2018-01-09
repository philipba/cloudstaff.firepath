namespace CloudStaff.BarCode
{
    public class BarCodeVideoResult : BarCodeResult
    {
        public BarCodeVideoResult()
        {
        }

        public BarCodeVideoResult(BarCodeResult initialValues)
        {
            Format = initialValues.Format;
            Value = initialValues.Value;
        }

        public int TimeInSeconds { get; set; }
    }
}

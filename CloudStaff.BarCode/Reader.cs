using System.Drawing;
using System.Threading.Tasks;
using ZXing;

namespace CloudStaff.BarCode
{
    public class Reader : IReader
    {
        public async Task<BarCodeResult> DecodeAsync(Bitmap image)
        {
            var barcodeReader = new BarcodeReader();
            
            var barcodeResult = await Task.Run(() => barcodeReader.Decode(image));

            if (barcodeResult == null) return null;
            
            return new BarCodeResult
            {
                Value = barcodeResult.Text,
                Format = (BarcodeFormat)barcodeResult.BarcodeFormat
            };
        }
    }
}

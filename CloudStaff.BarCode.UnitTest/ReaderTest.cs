using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CloudStaff.BarCode.UnitTest
{
    [TestClass]
    public class ReaderTest
    {
        [TestMethod]
        public async Task Decode_WhenValidBarCode128IsProvided_ShouldBeSuccessful()
        {
            var reader = new Reader();
            var barcodeImage = Properties.Resources.barcode_valid;
            var result = await reader.DecodeAsync(barcodeImage);

            Assert.AreEqual("CRK01-FEX-001-BEGIN", result.Value);
            Assert.AreEqual(BarcodeFormat.CODE_128, result.Format);
        }

        [TestMethod]
        public async Task Decode_WhenInvalidBarCodeIsProvided_ShouldReturnNull()
        {
            var reader = new Reader();
            var barcodeImage = Properties.Resources.barcode_invalid;
            var result = await reader.DecodeAsync(barcodeImage);

            Assert.IsNull(result);
        }
    }
}

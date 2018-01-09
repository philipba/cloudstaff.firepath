using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace CloudStaff.BarCode.UnitTest
{
    [TestClass]
    public class VideoReaderTest
    {
        [TestMethod]
        public async Task Decode_WhenValidBarCode128IsProvided_ShouldBeSuccessful()
        {
            var reader = new VideoReader();
            var result = await reader.Decode("C:\\Users\\Pao\\Desktop\\test\\test_vid.mp4");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("CRK01-FEX-001-BEGIN", result[0].Value);
            Assert.AreEqual(6, result[0].TimeInSeconds);
            Assert.AreEqual("CRK01-FEX-001-END", result[1].Value);
            Assert.AreEqual(19, result[1].TimeInSeconds);
        }
    }
}

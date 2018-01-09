using System.Drawing;
using System.Threading.Tasks;

namespace CloudStaff.BarCode
{
    public interface IReader
    {
        Task<BarCodeResult> DecodeAsync(Bitmap image);
    }
}

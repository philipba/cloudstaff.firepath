using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudStaff.BarCode
{
    public interface IVideoReader
    {
        Task<List<BarCodeVideoResult>> Decode(string videoPath);
    }
}

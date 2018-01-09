using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace CloudStaff.BarCode
{
    public class VideoReader : IVideoReader
    {
        private string _tempFilesBasePath;
        private string _ffmpegPath;
        private const string TempFilePrefix = "dump";

        public VideoReader()
        {
        }

        public VideoReader(string tempFilesBasePath, string ffmpegPath = "")
        {
            // Set FFMPEG executable path
            if (!string.IsNullOrEmpty(tempFilesBasePath))
            {
                _ffmpegPath = ffmpegPath;
                FFbase.FFmpegDir = _ffmpegPath;
            }

            // Setup directory for temporary files for video and image processing
            _tempFilesBasePath = tempFilesBasePath;
            Directory.CreateDirectory(_tempFilesBasePath);
        }

        public async Task<List<BarCodeVideoResult>> Decode(string videoPath)
        {            
            var tempFolderPath = $"{_tempFilesBasePath}\\{Guid.NewGuid().ToString()}";

            // Create temporary folder for image processing
            Directory.CreateDirectory(tempFolderPath);

            // Breakdown video to images per second
            bool conversionResult = await new Conversion().Start($"-i \"{videoPath}\" -vf fps=1 -r 1/1  \"{tempFolderPath}\\{TempFilePrefix}%05d.jpg\"");

            // Get valid barcodes based on the images
            var result = await GetBarCodesAsync(tempFolderPath);

            // Delete temporary folder and files
            await RemoveTempFiles(tempFolderPath);

            return result;
        }

        private async Task<List<BarCodeVideoResult>> GetBarCodesAsync(string tempFolderPath)
        {
            var reader = new Reader();
            int counter = 1;
            var result = new List<BarCodeVideoResult>();

            while (true)
            {
                var tempFileCounter = counter.ToString().PadLeft(5, '0');
                var filePath = $@"{tempFolderPath}\{TempFilePrefix}{tempFileCounter}.jpg";

                if (File.Exists(filePath))
                {
                    using (Bitmap barcodeBitmap = (Bitmap)Image.FromFile(filePath))
                    {
                        var barcodeResult = await reader.DecodeAsync(barcodeBitmap);
                        if (barcodeResult != null && result.Find(barcode => barcode.Value == barcodeResult.Value) == null)
                        {
                            var barCodeVideResult = new BarCodeVideoResult(barcodeResult)
                            {
                                TimeInSeconds = counter - 1
                            };
                            result.Add(barCodeVideResult);
                        }
                    }

                    counter++;
                }
                else
                {
                    break;
                }
            }

            return result;
        }

        private async Task RemoveTempFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
            await Task.Run(() => dir.Delete(true));
        }
    }
}

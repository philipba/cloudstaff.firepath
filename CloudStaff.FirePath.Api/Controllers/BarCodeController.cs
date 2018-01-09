using CloudStaff.BarCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CloudStaff.FirePath.Api.Controllers
{
    public class BarCodeController : ApiController
    {
        private readonly string _appDataPath;
        private readonly string _uploadsPath;
        private readonly IReader _reader;

        public BarCodeController()
        {
            // TODO: Use dependency injection
            _appDataPath = HttpContext.Current.Server.MapPath("~/App_Data");
            _uploadsPath = HttpContext.Current.Server.MapPath("~/Uploads");
            Directory.CreateDirectory(_appDataPath);
            Directory.CreateDirectory(_uploadsPath);

            _reader = new Reader();
        }

        [HttpPost]
        public async Task<IHttpActionResult> Put()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartFormDataStreamProvider(_appDataPath);
            var response = new List<BarCodeResult>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string newPath = string.Empty;

                foreach (MultipartFileData fileData in provider.FileData)
                {

                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return BadRequest("This request is not properly formatted");
                    }

                    newPath = CreateNewUnploadFilePath(fileData.Headers.ContentDisposition.FileName);

                    File.Move(fileData.LocalFileName, newPath);

                    using (Bitmap barcodeBitmap = (Bitmap)Image.FromFile(newPath))
                    {
                        var barcodeResult = await _reader.DecodeAsync(barcodeBitmap);
                        if (barcodeResult != null)
                        {
                            barcodeResult.Id = CleanupFileName(fileData.Headers.ContentDisposition.FileName);
                            response.Add(barcodeResult);
                        }
                    }
                }
                
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private string CreateNewUnploadFilePath(string originalFileName)
        {
            originalFileName = CleanupFileName(originalFileName);

            var newFileName = Guid.NewGuid().ToString();
            var fileExtension = Path.GetExtension(originalFileName);
            return Path.Combine(_uploadsPath, newFileName + fileExtension);
        }

        private string CleanupFileName(string originalFileName)
        {
            if (originalFileName.StartsWith("\"") && originalFileName.EndsWith("\""))
            {
                originalFileName = originalFileName.Trim('"');
            }
            if (originalFileName.Contains(@"/") || originalFileName.Contains(@"\"))
            {
                originalFileName = Path.GetFileName(originalFileName);
            }

            return originalFileName;
        }
    }
}
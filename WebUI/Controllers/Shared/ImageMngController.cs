using CameraMap.Models.Files;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    public class ImageMngController : Controller
    {
        public ActionResult GetImageFile(long FileID, string FileName)
        {
            byte[] bytes = (byte[])Session[FileID.ToString()];

            if (bytes == null)
            {
                var f = FileModelReg.GetFileModelByID(FileID);
                if (f != null && f.FileID > 0)
                {
                    bytes = f.FileData;
                }
            }

            if (bytes == null)
            {
                return new HttpNotFoundResult();
            }


            var imgType = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            var pdfType = new string[] { ".pdf" };

            string contentType = ".abc";
            var lastIndex = FileName.LastIndexOf(".");
            if (lastIndex >= 0)
            {
                contentType = FileName.Substring(lastIndex);
            }
            if (imgType.Contains(contentType) || pdfType.Contains(contentType))
            {
                var f = new FileContentResult(bytes, contentType);
                return f;
            }
            else
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                var fsr = new FileStreamResult(ms, contentType);
                fsr.FileDownloadName = Url.Encode(FileName);
                return fsr;
            }

        }

        public ActionResult GetQRCode(string FileID, string FileName)
        {
            var content = FileID;
            try
            {
                if (!string.IsNullOrEmpty(FileName) && FileName.ToLower().StartsWith("qr64"))
                {
                    content = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(FileID.Replace("_2f","/")));
                }
            }
            catch (Exception)
            { }
            var ms = new MemoryStream();
            GenerateQRCode(content, ms);

            ms.Position = 0;
            var contentType = "image/Png";
            var fsr = new FileStreamResult(ms, contentType);
            return fsr;
        }

        private bool GenerateQRCode(string strContent, MemoryStream ms)
        {
            ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M; //误差校正水平   
            string Content = strContent;//待编码内容  
            QuietZoneModules QuietZones = QuietZoneModules.Two;  //空白区域   
            int ModuleSize = 12;//大小  
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (encoder.TryEncode(Content, out qr))//对内容进行编码，并保存生成的矩阵  
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
            }
            else
            {
                return false;
            }
            return true;
        }


        /*
        /// <summary>
        /// Class containing the description of the QR code and wrapping encoding and rendering.
        /// </summary>
        internal class CodeDescriptor
        {
            public ErrorCorrectionLevel Ecl;
            public string Content;
            public QuietZoneModules QuietZones;
            public int ModuleSize;
            public BitMatrix Matrix;
            public string ContentType;

            /// <summary>
            /// Parse QueryString that define the QR code properties
            /// </summary>
            /// <param name="request">HttpRequest containing HTTP GET data</param>
            /// <returns>A QR code descriptor object</returns>
            public static CodeDescriptor Init(HttpRequestBase request)
            {
                var cp = new CodeDescriptor();

                // Error correction level
                if (!Enum.TryParse(request.QueryString["e"], out cp.Ecl))
                    cp.Ecl = ErrorCorrectionLevel.L;

                // Code content to encode
                cp.Content = request.QueryString["t"];

                // Size of the quiet zone
                if (!Enum.TryParse(request.QueryString["q"], out cp.QuietZones))
                    cp.QuietZones = QuietZoneModules.Two;

                // Module size
                if (!int.TryParse(request.QueryString["s"], out cp.ModuleSize))
                    cp.ModuleSize = 12;

                return cp;
            }

            /// <summary>
            /// Encode the content with desired parameters and save the generated Matrix
            /// </summary>
            /// <returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>
            public bool TryEncode()
            {
                var encoder = new QrEncoder(Ecl);
                QrCode qr;
                if (!encoder.TryEncode(Content, out qr))
                    return false;

                Matrix = qr.Matrix;
                return true;
            }

            /// <summary>
            /// Render the Matrix as a PNG image
            /// </summary>
            /// <param name="ms">MemoryStream to store the image bytes into</param>
            internal void Render(MemoryStream ms)
            {
                var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                render.WriteToStream(Matrix, ImageFormat.Png, ms);
                ContentType = "image/png";
            }
        }
        */
    }
}

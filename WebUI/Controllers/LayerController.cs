using CameraMap.Models.Files;
using CameraMap.Models.Layer;
using CameraMap.Models.ResponseModel;
using CameraMap.Sessions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CameraMap.Controllers
{
    public class LayerController : Controller
    {
        //
        // GET: /Layer/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetLayerModel(long Id)
        {
            var model = LayerModelReg.GetModel(Id);
            model.SetUrlHelper(Url);
            var m = new
            {
                Id = model.Id.ToString(),
                LayerKey=model.LayerKey,
                LayerName = model.LayerName,
                IconFile = model.IconFile.ToString(),
                IconFileUrl = model.IconFileUrl,
                IconFileName = model.IconFileName,
            };
            var r = new
            {
                Model = m
            };
            return Json(r);
        }

        [HttpPost]
        public ActionResult EditLayerAjax(LayerModel model)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);

            if (!ModelState.IsValid)
            {
                var r = new
                {
                    Success = false
                };
                return Json(r);
            }

            bool bl = false;
            if (model.Id > 0)
            {
                try
                {
                    var oldmodel = LayerModelReg.GetModel(model.Id);
                    oldmodel.LayerName = model.LayerName;
                    oldmodel.IconFile = model.IconFile;
                    oldmodel.LastUserID = loginInfo.LoginID;
                    bl = LayerModelReg.Update(oldmodel);
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    model.OrganizationID = loginInfo.OrganizationID;
                    model.LastUserID = loginInfo.LoginID;
                    bl = LayerModelReg.Add(model);
                }
                catch (Exception) { }
            }
            var jsonObj = new
            {
                Success = bl
            };
            return Json(jsonObj);
        }

        [HttpPost]
        public ActionResult DeleteLayerAjax(long Id)
        {
            var loginInfo = SessionLoginInfo.GetInstance(Session);
            var model = new LayerModel()
            {
                Id = Id,
                LastUserID = loginInfo.LoginID,
                OrganizationID = loginInfo.OrganizationID,
            };

            var bl = LayerModelReg.Delete(model);
            var jsonObj = new
            {
                Success = bl
            };
            return Json(jsonObj);
        }

        public ActionResult SetLayerIconFile()
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(Request.Files["file"].FileName);
            string FileName = fi.Name;

            var bytes = new byte[Request.Files["file"].InputStream.Length];
            Request.Files["file"].InputStream.Read(bytes, 0, bytes.Length);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                var img = new System.Drawing.Bitmap(ms);
                var newBytes=ResizeImage(img, 32, 32);
                if (newBytes != null)
                {
                    bytes = newBytes;
                }
                img.Dispose();
                var idx= FileName.LastIndexOf(".");
                if (idx > 0) {
                    FileName = FileName.Substring(0, idx) + ".jpg";
                }
            }

            var fileID = FileModelReg.Insert(FileName, bytes);

            //string filePath = ParsePath(fileName);
            var sendObj = new ImageResponseModel
            {
                FileKey = fileID.ToString(),
                FileName = FileName,
                FileUrl = Url.RouteUrl("DownloadURL", new { controller = "ImageMng", action = "GetImageFile", FileID = fileID.ToString(), FileName = FileName }),
                FileUrlThumb = Url.RouteUrl("DownloadURL", new { controller = "ImageMng", action = "GetImageFile", FileID = fileID.ToString(), FileName = FileName }),
            };

            return View("FileUploaded", sendObj);
        }

        byte[] ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                var thumbImg = bmp.GetThumbnailImage(newW, newH, null, System.IntPtr.Zero);


                //Bitmap b = new Bitmap(newW, newH);
                //Graphics g = Graphics.FromImage(b);
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, newW, newH));
                //g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                //g.Dispose();

                using (var ms = new System.IO.MemoryStream())
                {
                    thumbImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    var bytes = new byte[Convert.ToInt32(ms.Length)];
                    ms.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}

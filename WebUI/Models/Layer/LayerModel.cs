using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.Layer
{
    public class LayerModel
    {
        private UrlHelper Url;
        public LayerModel() { }
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 利用者構成ID
        /// </summary>
        public string OrganizationID { get; set; }
        /// <summary>
        /// layer名
        /// </summary>
        public string LayerKey { get; set; }
        /// <summary>
        /// layer名
        /// </summary>
        public string LayerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long IconFile { get; set; }

        public string IconFileName { get; set; }

        public string IconFileUrl
        {
            get
            {
                if (Url != null && IconFile>0)
                {
                    return Url.RouteUrl("DownloadURL", new { controller = "ImageMng", action = "GetImageFile", FileID = IconFile.ToString(), FileName = IconFileName });
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DisplayNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DisplayFlag { get; set; }
        /// <summary>
        /// 最終更新者
        /// </summary>
        public long LastUserID { get; set; }

        public void SetUrlHelper(UrlHelper url)
        {
            this.Url = url;
        }

    }


}

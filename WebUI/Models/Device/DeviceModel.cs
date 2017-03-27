using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.Device
{
    public class DeviceModel
    {
        private UrlHelper Url;

        /// <summary>
        /// デバイスID
        /// </summary>
        public long DeviceId { get; set; }
        /// <summary>
        /// 利用者構成ID
        /// </summary>
        public string OrganizationID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long LayerId { get; set; }
        /// <summary>
        /// デバイスKey
        /// </summary>
        public string DeviceKey { get; set; }
        /// <summary>
        /// デバイス名
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// デバイスURL
        /// </summary>
        public string DeviceUrl { get; set; }
        /// <summary>
        /// 説明
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 地理纬度
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// 地理经度
        /// </summary>
        public decimal Lng { get; set; }
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
                if (Url != null && IconFile > 0)
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
        /// <summary>
        /// 最終更新時間
        /// </summary>
        public DateTime LastUpdatetime { get; set; }

        public void SetUrlHelper(UrlHelper url)
        {
            this.Url = url;
        }


    }
}

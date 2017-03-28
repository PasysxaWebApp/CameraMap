using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.Device
{
    public class LineModel
    {
        private UrlHelper Url;

        /// <summary>
        /// 
        /// </summary>
        public long LineId {get;set;}
        /// <summary>
        /// 利用者構成ID
        /// </summary>
        public string OrganizationID {get;set;}

        public MapDeviceType DeviceType
        {
            get
            {
                return MapDeviceType.Line;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long LayerId {get;set;}
        /// <summary>
        /// デバイスKey
        /// </summary>
        public string LineKey {get;set;}
        /// <summary>
        /// デバイス名
        /// </summary>
        public string LineName {get;set;}
        /// <summary>
        /// 地理纬度
        /// </summary>
        public string Points {get;set;}
        /// <summary>
        /// 説明
        /// </summary>
        public string Note {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public int MinZoom {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public int MaxZoom {get;set;}

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
        public int DisplayNo {get;set;}
        /// <summary>
        /// 
        /// </summary>
        public int DisplayFlag {get;set;}
        /// <summary>
        /// 最終更新者
        /// </summary>
        public long LastUserID {get;set;}
        /// <summary>
        /// 最終更新時間
        /// </summary>
        public DateTime LastUpdatetime {get;set;}

        public void SetUrlHelper(UrlHelper url)
        {
            this.Url = url;
        }

    }
}

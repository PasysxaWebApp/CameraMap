using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.Device
{
    public class LineModel : IDevice
    {
        private UrlHelper Url;

        /// <summary>
        /// 
        /// </summary>
        public long LineId {get;set;}

        public long DeviceId {
            get
            {
                return LineId;
            }
        }

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
        public string DeviceTypeName
        {
            get
            {
                return DeviceType.ToString();
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
        public string DeviceKey
        {
            get
            {
                return LineKey;
            }
        }

        /// <summary>
        /// デバイス名
        /// </summary>
        public string LineName {get;set;}
        public string DeviceName
        {
            get
            {
                return LineName;
            }
        }
        /// <summary>
        /// 地理纬度
        /// </summary>
        public string Points {get;set;}

        public BMapPoint[] MapPoints
        {
            get
            {
                var ps = new List<BMapPoint>();
                var lines = Points.Split(new char[] { '\n' });
                foreach (var l in lines) {
                    var p= l.Split(new char[] { ',' });
                    if (p.Length != 2) {
                        continue;
                    }
                    decimal lat,lng;
                    if (decimal.TryParse(p[0], out lng) && decimal.TryParse(p[1], out lat)) {
                        ps.Add(new BMapPoint() { Lat = lat, Lng = lng });
                    }
                }
                return ps.ToArray();
            }
        }


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

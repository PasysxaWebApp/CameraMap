using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.ResponseModel
{
    public class ImageResponseModel
    {
        public bool IsFail { get; set; }
        public string FileKey { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileUrlThumb { get; set; }
    }
}

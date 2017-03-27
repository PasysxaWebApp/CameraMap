using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Files
{
    public class FileModel
    {
        public long FileID { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}

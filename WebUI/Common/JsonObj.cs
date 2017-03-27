using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Common
{
    public class JsonResultObj
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
    public class FishSpeciesEditJsonResultObj : JsonResultObj
    {
        public bool IsMinFishCount { get; set; }
    }

}

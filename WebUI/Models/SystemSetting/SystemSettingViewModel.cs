using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.SystemSetting
{
    public class SystemSettingViewModel
    {
        public string OrganizationID { get; set; }
        public List<SelectListItem> OrganizationList { get; set; }
        public List<SystemSettingModel> Settings { get; set; }

        public string btnName { get; set; }
      
    }
}

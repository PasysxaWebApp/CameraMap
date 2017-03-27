using CameraMap.App_GlobalResources;
using SharedUtilitys.Attributes;
using SharedUtilitys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace CameraMap.Models.Master.Organization
{
    public class OrganizationModel : BaseModel
    { 
        [AutoProperty]
        [Display(Name = @"OrganizationID")]
        public string OrganizationID { get; set; }
        [AutoProperty]
        [StringLength(3)]
        [Display(Name = "labOrganizationCode", ResourceType = typeof(LanguageResource))]
        [RegularExpression(@"[A-Za-z]{3}",
            ErrorMessageResourceName = "AlertOrganizationCode", ErrorMessageResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string OrganizationCode { get; set; }
        [AutoProperty]
        [Display(Name = "ItemOfficeName", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string OfficeName { get; set; }
        [AutoProperty]
        [Display(Name = @"OfficeNameEn")]
        public string OfficeNameEn { get; set; }
        [AutoProperty]
        [Display(Name = "ItemStaffName", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string StaffName { get; set; }
        [AutoProperty]
        [Display(Name = @"StaffNameEn")]
        public string StaffNameEn { get; set; }
        [AutoProperty]
        [Display(Name = "ItemPostal", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        //[RegularExpression(@"/[\d,-]+/",
        //    ErrorMessageResourceName = "AlertMailAddress", ErrorMessageResourceType = typeof(LanguageResource))]
        public string Postal { get; set; }
        [AutoProperty]
        [Display(Name = "ItemStateCode", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public int StateCode { get; set; }
        [AutoProperty]
        [Display(Name = "ItemAddress1", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string Address1 { get; set; }
        [AutoProperty]
        [Display(Name = @"Address2")]
        public string Address2 { get; set; }
        [AutoProperty]
        [Display(Name = @"AddressEn")]
        public string AddressEn { get; set; }
        [AutoProperty]
        [Display(Name = "ItemMailAddress", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}",
            ErrorMessageResourceName = "AlertMailAddress", ErrorMessageResourceType = typeof(LanguageResource))]
        [DataType(DataType.EmailAddress)]
        public string MailAddress { get; set; }
        [AutoProperty]
        [Display(Name = "ItemPhoneNumber", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        //[RegularExpression(@"/[(,),\d,-]+/",
        //    ErrorMessageResourceName = "AlertMailAddress", ErrorMessageResourceType = typeof(LanguageResource))]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [AutoProperty]
        [Display(Name = @"FaxNumber")]
        public string FaxNumber { get; set; }
        //[AutoProperty]
        //[Display(Name = @"StartDate")]
        //public DateTime StartDate { get; set; }
        //[AutoProperty]
        //[Display(Name = @"EndDate")]
        //public DateTime EndDate { get; set; }
        [AutoProperty]
        [Display(Name = @"LastUserID")]
        public long LastUserID { get; set; }
        [AutoProperty]
        [Display(Name = @"LastUpdatetime")]
        public DateTime LastUpdatetime { get; set; }

        /// <summary>
        /// 日別一括登録方式
        /// 1.固定モード 2.可変モード
        /// </summary>
        public int BatchSaveWithDayMode { get; set; }

        public long? FarmPic { get; set; }
        public string FarmNote { get; set; }
        public long? FishinggroundPic { get; set; }
        public string FishinggroundNote { get; set; }

        public string FarmPicName { get; set; }
        public string FishinggroundPicName { get; set; }       


        public List<SelectListItem> StateCodeList { get; set; }
        public List<SelectListItem> BatchSaveWithDayModeList { get; set; }

        public OrganizationModel()
        {
            BatchSaveWithDayMode = 0;
            BatchSaveWithDayModeList = new List<SelectListItem>();
            BatchSaveWithDayModeList.Add(new SelectListItem() { Text = "", Value = "0" });
            BatchSaveWithDayModeList.Add(new SelectListItem() { Text = "固定モード", Value = "1" });
            BatchSaveWithDayModeList.Add(new SelectListItem() { Text = "可変モード", Value = "2" });
        }

    }
}
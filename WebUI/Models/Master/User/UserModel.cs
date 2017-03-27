using System;
using System.Collections.Generic;
using System.Linq;
using CameraMap.App_GlobalResources;
using SharedUtilitys.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CameraMap.Enums;
namespace CameraMap.Models.Master.User
{
    public class UserModel : BaseModel
    {
        public long Guid { get; set; }
        public string OrganizationID { get; set; }

        public string OfficeName { get; set; }
        public string OfficeNameEn { get; set; }

        [Display(Name = "ItemLoginName", ResourceType = typeof(LanguageResource))]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9._-]+\.[A-Za-z]{2,4}",
            ErrorMessageResourceName = "AlertMailAddress", ErrorMessageResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false,
            ErrorMessageResourceName = "AlertRequired",
            ErrorMessageResourceType = typeof(LanguageResource))]
        public string LoginName { get; set; }

        [Display(Name = "ItemPassword", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string Password { get; set; }

        [Display(Name = "ItemConfirmPassword", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        [Compare("Password", ErrorMessageResourceName = "AlertPasswordCompare", ErrorMessageResourceType = typeof(LanguageResource))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "ItemUserName", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string UserName { get; set; }

        [Display(Name = "ItemUserNameKana", ResourceType = typeof(LanguageResource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "AlertRequired", ErrorMessageResourceType = typeof(LanguageResource))]
        public string UserNameKana { get; set; }


        public string UserNameEn { get; set; }

        private int _authority;
        public int Authority
        {
            get
            {
                return _authority;
            }
            set
            {
                switch (value)
                {
                    case (int)SystemRollEnum.Visitor:
                        AuthorityStr = LanguageResource.ItemAuthorityReader;
                        break;
                    case (int)SystemRollEnum.User:
                        AuthorityStr = LanguageResource.ItemAuthorityUser;
                        break;
                    case (int)SystemRollEnum.Administrator:
                        AuthorityStr = LanguageResource.ItemAuthorityAdmin;
                        break;
                    case (int)SystemRollEnum.SysAdmin:
                        AuthorityStr = LanguageResource.ItemAuthoritySysAdmin;
                        break;
                    case (int)SystemRollEnum.UnionOfficeUser:
                        AuthorityStr = "統合事業所ユーザー";
                        break;
                    default:
                        AuthorityStr = "";
                        break;
                }

                _authority = value;
            }
        }

        public string AuthorityStr { get; set; }

        public List<SelectListItem> AuthorityList
        {
            get;
            private set;
        }


        public int DisplayNo { get; set; }


        public bool DisplayFlag { get; set; }


        public long LastUserID { get; set; }

        public DateTime LastUpdatetime { get; set; }

        public void SetAuthrityList(SystemRollEnum roll)
        {
            var authorityList = new List<SelectListItem>();
            if (roll == SystemRollEnum.SysAdmin)
            {
                authorityList.Add(new SelectListItem { Text = LanguageResource.ItemAuthorityAdmin, Value = "2" });
                authorityList.Add(new SelectListItem { Text = LanguageResource.ItemAuthoritySysAdmin, Value = "3" });

            }
            else if (roll == SystemRollEnum.Administrator)
            {
                authorityList.Add(new SelectListItem { Text = LanguageResource.ItemAuthorityReader, Value = "0" });
                authorityList.Add(new SelectListItem { Text = LanguageResource.ItemAuthorityUser, Value = "1" });
                authorityList.Add(new SelectListItem { Text = LanguageResource.ItemAuthorityAdmin, Value = "2" });
            }
            this.AuthorityList = authorityList;
        }

        public List<SelectListItem> Organizations { get; set; }

        public List<UnionOrganization> UnionOrganizations { get; set; }

        public bool UnionOfficeUser
        {
            get {
                return UnionOrganizations.FindAll(m => m.Checked == true).Count > 0;
            }
        }

        public string UnionOfficeIds
        {
            get
            {
                if (UnionOfficeUser)
                {
                    return UnionOrganizations.Where(m => m.Checked == true).Select(m => "'" + m.OrganizationID + "'").Aggregate((c, n) => c + "," + n);
                }
                else {
                    return "";
                }
            }
        }

        public UserModel()
        {
            Organizations = new List<SelectListItem>();
            UnionOrganizations = new List<UnionOrganization>();
        }

    }

    public class UnionOrganization : BaseModel
    {
        public long UnionId { get; set; }
        public long UserId { get; set; }
        public string OrganizationID { get; set; }
        public string OfficeName { get; set; }
        public string OfficeNameEn { get; set; }

        public bool Checked
        {
            get
            {
                return UnionId != 0;
            }
            set
            {
                UnionId = value ? 1 : 0;
            }
        }

    }

}
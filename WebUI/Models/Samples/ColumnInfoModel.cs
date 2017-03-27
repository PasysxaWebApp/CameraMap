using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedUtilitys.Attributes;
using SharedUtilitys.Models;

namespace CameraMap.Models.Samples
{
    public class ColumnInfoModel : BaseModel
    {
        [AutoProperty]
        [Display(Name = @"Field")]
        public string Field { get; set; }
        [AutoProperty]
        [Display(Name = @"Type")]
        public string Type { get; set; }
        [AutoProperty]
        [Display(Name = @"Null")]
        public string Null { get; set; }
        [AutoProperty]
        [Display(Name = @"Key")]
        public string Key { get; set; }
        [AutoProperty]
        [Display(Name = @"Default")]
        public string Default { get; set; }
        [AutoProperty]
        [Display(Name = @"Extra")]
        public string Extra { get; set; }

        public ColumnInfoModel(Dictionary<string, object> item)
        {
            SetDictionary(item);
        }
    }
}
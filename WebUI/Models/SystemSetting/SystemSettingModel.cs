using PacificSystem.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.SystemSetting
{
    [Serializable]
    public class SystemSettingModel
    {
        public string PartialViewName { get; set; }

        public bool IsPasysOnly { get; set; }

        public string SettingGroupName
        {
            get
            {
                switch (SettingGroup)
                {
                    case SystemSettingGroupEnum.BaseSetting:
                        return App_GlobalResources.LanguageResource.SystemSettingGroupEnum_BaseSetting; // "基本設定";
                    case SystemSettingGroupEnum.AdvanceSetting:
                        return App_GlobalResources.LanguageResource.SystemSettingGroupEnum_AdvanceSetting;// "詳細設定";
                    default:
                        return "";
                }
            }
        }

        public string SystemSettingKey { get; set; }
        /// <summary>
        /// 利用者構成ID
        /// </summary>
        public string OrganizationID { get; set; }

        public SystemSettingGroupEnum SettingGroup { get; set; }

        public string SystemSettingName { get; set; }

        public int DataType { get; set; }

        public decimal MinValue { get; set; }
  
        public decimal MaxValue { get; set; }

        public string Value { get; set; }

        public string Note { get; set; }

        public int Flag { get; set; }

        public SystemSettingModel()
        {
           
        }

        public SystemSettingModel(string systemSettingKey, int dataType)
        {
            this.SystemSettingKey = systemSettingKey;
            this.DataType = dataType;
        }

        public SystemSettingModel Clone()
        {
            BinaryFormatter b = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                b.Serialize(ms, this);
                ms.Position = 0;
                var newObj = b.Deserialize(ms) as SystemSettingModel;
                return newObj;
            }
        }

    }

    public enum SystemSettingGroupEnum
    {
        BaseSetting,
        AdvanceSetting
    }

    [Serializable]
    public class DigitSystemSetting : SystemSettingModel
    {
        public DigitSystemSetting(string systemSettingKey, decimal min, decimal max)
            : base(systemSettingKey, 0)
        {
            this.PartialViewName = "_digitSystemSettingPart";
            this.MinValue = Math.Min(min, max);
            this.MaxValue = Math.Max(min, max);
        }

        public decimal? SystemSettingValue
        {
            get
            {
                decimal o;
                if (decimal.TryParse(this.Value, out o))
                {
                    return o;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    var v = Math.Max(value.Value, MinValue);
                    v = Math.Min(v, MaxValue);
                    this.Value = v.ToString();
                }
                else
                {
                    this.Value = "";
                }
            }
        }

    }

    [Serializable]
    public class TextSystemSetting : SystemSettingModel
    {
        public TextSystemSetting(string systemSettingKey)
            : base(systemSettingKey, 1)
        {
            this.PartialViewName = "_textSystemSettingPart";
        }
    }

    [Serializable]
    public class ComboBoxSystemSetting : SystemSettingModel
    {
        public string[] ComboList { get; set; }
        public List<SelectListItem> GetListItems()
        {
            var listItems = new List<SelectListItem>();
            listItems.Clear();
            foreach (var v in ComboList)
            {
                listItems.Add(new SelectListItem() { Text = v, Value = v });
            }
            return listItems;
        }
        public ComboBoxSystemSetting(string systemSettingKey)
            : base(systemSettingKey, 2)
        {
            this.PartialViewName = "_comboBoxSystemSettingPart";

        }
    }

    [Serializable]
    public class BoolSystemSetting : SystemSettingModel
    {
        public BoolSystemSetting(string systemSettingKey)
            : base(systemSettingKey, 3)
        {
            this.PartialViewName = "_boolSystemSettingPart";
        }

        public int SystemSettingValue
        {
            get
            {
                return Converts.ToTryInt(this.Value,1);
            }
            set
            {
                this.Value = value.ToString();
            }
        }


    }



}

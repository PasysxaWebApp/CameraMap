using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CameraMap.Models.SystemSetting
{
    public class SystemSettingManager
    {
        public static SystemSettingManager GetInstance()
        {
            return new SystemSettingManager();
        }
        private SystemSettingManager() 
        { 
        }

        public List<SystemSettingModel> GetSystemSettingManagerList(string organizationID)
        {
            var _baseSettings = new List<SystemSettingModel>();

            var num = 0;
            _baseSettings.Add(new DigitSystemSetting("key1", 0, 100)
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.BaseSetting,
                Flag=num,
                OrganizationID="",
                SystemSettingName = "グリッドの表示件数",
                Value="0",
                Note = "画面に表示するグリッドの件数を設定します。(0～100)"
            });
            num++;
            _baseSettings.Add(new TextSystemSetting("key2")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.BaseSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "テキスト形式の項目",
                Value = "",
                Note = "説明を表示する。"
            });
            num++;
            _baseSettings.Add(new ComboBoxSystemSetting("key3")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.BaseSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "ComboBox形式の項目",
                ComboList = new[] { "value1", "value2" },
                Value = "value1",
                Note = "説明を表示する。"
            });
            num++;
            _baseSettings.Add(new BoolSystemSetting("key4")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.BaseSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "Bool形式の項目",
                Value = "true",
                Note = "説明を表示する。"
            });
            num++;
            _baseSettings.Add(new DigitSystemSetting("key5", 10, 100)
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.AdvanceSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "グリッドの表示件数",
                Value = "10",
                Note = "画面に表示するグリッドの件数を設定します。(10～100)"
            });
            num++;
            _baseSettings.Add(new TextSystemSetting("key6")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.AdvanceSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "テキスト形式の項目",
                Value = "",
                Note = "説明を表示する。"
            });
            num++;
            _baseSettings.Add(new ComboBoxSystemSetting("key7")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.AdvanceSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "ComboBox形式の項目",
                ComboList = new[] { "value1", "value2" },
                Value = "value1",
                Note = "説明を表示する。"
            });
            num++;
            _baseSettings.Add(new BoolSystemSetting("key8")
            {
                IsPasysOnly = true,
                SettingGroup = SystemSettingGroupEnum.AdvanceSetting,
                Flag = num,
                OrganizationID = "",
                SystemSettingName = "Bool形式の項目",
                Value = "true",
                Note = "説明を表示する。"
            });
            num++;
            //_baseSettings.Add(new ComboBoxSystemSetting("key9")
            //{
            //    IsPasysOnly = true,
            //    SettingGroup = SystemSettingGroupEnum.AdvanceSetting,
            //    Flag = num,
            //    OrganizationID = "",
            //    SystemSettingName = "グリッドの色",
            //    ComboList = new[] { "Clear", "Red", "Yellow", "Blue" },
            //    Value = "Clear",
            //    Note = "説明を表示する。"
            //});
            //num++;

            var sts=_baseSettings.ToArray();
            foreach (var st in sts)
            {
                var s = st.Clone();
                s.Flag = num;
                s.IsPasysOnly = false;
                s.OrganizationID = organizationID;
                _baseSettings.Add(s); 
                num++;
            }  

            var list = GetSettings(organizationID);
            foreach (var dic in list)
            {
                var key = Converts.ToTryString(dic["SystemSettingKey"]);
                var orgId = Converts.ToTryString(dic["OrganizationID"]);
                var v = Converts.ToTryString(dic["Value"]);

                var setting = _baseSettings.Find(m => m.SystemSettingKey.Equals(key) && m.OrganizationID.Equals(orgId));
                if (setting != null)
                {
                    setting.Value = v;
                }
            }

            return _baseSettings;
        }


        public List<SelectListItem> GetOrganizationList()
        {
            var selectList = new List<SelectListItem>();
            using (var utility = DbUtility.GetInstance())
            {
                const string sql = @"SELECT 
                                    OrganizationID,
                                    OfficeName
                                FROM m_organization;";

                var list = utility.ExecuteReader(sql);
                if (list.Any()) 
                {
                    foreach (var item in list) 
                    {
                        selectList.Add(new SelectListItem()
                        {
                            Value = Converts.ToTryString(item["OrganizationID"], ""),
                            Text = Converts.ToTryString(item["OfficeName"], ""),
                            Selected = false
                        });
                    }
                }
            }

            return selectList;
        }


        private IEnumerable<Dictionary<string, object>> GetSettings(string organizationID)
        {

            using (var utility = DbUtility.GetInstance())
            {
                const string sql = @" 
                                SELECT `t_systemsetting`.`SystemSettingKey`,
                                    `t_systemsetting`.`OrganizationID`,
                                    `t_systemsetting`.`Value`
                                FROM `t_systemsetting`
                                where OrganizationID=@OrganizationID or OrganizationID='' or OrganizationID is null ;
                                ";


                utility.AddParameter("OrganizationID", organizationID);
                return utility.ExecuteReader(sql);
            }

        }

        public void SaveSetting(List<SystemSettingModel> settings)
        {

            string sql1 = @" 
                               UPDATE t_systemsetting
                                SET    
                                Value = @Value
                                WHERE SystemSettingKey = @SystemSettingKey and OrganizationID =@OrganizationID;";

            string sql2 = @" 
                              INSERT INTO t_systemsetting
                                (SystemSettingKey,
                                OrganizationID,
                                Value)
                                VALUES (
                                    @SystemSettingKey,
                                    @OrganizationID,
                                    @Value
                                );";


            using (var utility = DbUtility.GetInstance())
            {
                try
                {
                    utility.BeginTransaction();
                    foreach (var setting in settings)
                    {
                        utility.AddParameter("OrganizationID", Converts.ToTryString(setting.OrganizationID,""));
                        utility.AddParameter("Value", setting.Value);
                        utility.AddParameter("SystemSettingKey", Converts.ToTryString(setting.SystemSettingKey, ""));
                        var effRow = utility.ExecuteNonQuery(sql1);

                        if (effRow > 0)
                        {
                            continue;
                        }

                        utility.AddParameter("OrganizationID", Converts.ToTryString(setting.OrganizationID, ""));
                        utility.AddParameter("Value", setting.Value);
                        utility.AddParameter("SystemSettingKey", Converts.ToTryString(setting.SystemSettingKey, ""));
                        utility.ExecuteNonQuery(sql2);
                    }
                    utility.Commit();
                }
                catch (Exception ex)
                {
                    utility.Rollback();
                    throw ex;
                }
            }



        }

    }
}

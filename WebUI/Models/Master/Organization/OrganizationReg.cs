using System;
using System.Collections.Generic;
using CameraMap.Models.Base;
using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using SharedUtilitys.Models;
using System.Web.WebPages.Html;
using System.Text;

namespace CameraMap.Models.Master.Organization
{
    public class OrganizationReg : BaseModel
    {
        public static OrganizationReg GetInstance()
        {
            return new OrganizationReg();
        }

        public List<OrganizationModel> Items { get; set; }

        public void SetOrganizationList()
        {
            var lstResult = new List<OrganizationModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT `m_organization`.`OrganizationID`,
                        `m_organization`.`OrganizationCode`,
                        `m_organization`.`OfficeName`,
                        `m_organization`.`OfficeNameEn`,
                        `m_organization`.`StaffName`,
                        `m_organization`.`StaffNameEn`,
                        `m_organization`.`Postal`,
                        `m_organization`.`StateCode`,
                        `m_organization`.`Address1`,
                        `m_organization`.`Address2`,
                        `m_organization`.`AddressEn`,
                        `m_organization`.`MailAddress`,
                        `m_organization`.`PhoneNumber`,
                        `m_organization`.`FaxNumber`,
                        `m_organization`.`StartDate`,
                        `m_organization`.`EndDate`,
                        `m_organization`.`BatchSaveWithDayMode`,
                        `m_organization`.`FarmPic`,
                        `m_organization`.`FarmNote`,
                        `m_organization`.`FishinggroundPic`,
                        `m_organization`.`FishinggroundNote`,
                        `m_organization`.`LastUserID`,
                        `m_organization`.`LastUpdatetime`
                    FROM `m_organization`
                    Order by `m_organization`.`OfficeName`;
                    ";
                utility.ExecuteReaderModelList(sql, lstResult);
            }

            Items = lstResult;
        }


        public OrganizationModel GetOrganizationModel(string OrganizationID)
        {
            var model = new OrganizationModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = string.Format(@"
                                    select 
                                        OrganizationID, 
                                        OrganizationCode,
                                        OfficeName, 
                                        OfficeNameEn, 
                                        StaffName, 
                                        StaffNameEn, 
                                        Postal, 
                                        StateCode, 
                                        Address1, 
                                        Address2, 
                                        AddressEn, 
                                        MailAddress, 
                                        PhoneNumber, 
                                        FaxNumber,
                                        BatchSaveWithDayMode,
                                        FarmPic,
                                        FarmNote,
                                        FishinggroundPic,
                                        FishinggroundNote, 
                                        LastUserID, 
                                        LastUpdatetime 
                                    from m_organization
                                    where OrganizationID = @OrganizationID", OrganizationID);
                utility.AddParameter("OrganizationID", OrganizationID);
                utility.ExecuteReaderModel(sql, model);
            }
            model.StateCodeList = GetSelectItem();

            return  model;
        }

        public List<SelectListItem> GetSelectItem()
        {
            var OrgList = new List<SelectListItem>();
            //var test = new SelectListItem
            //{
            //    Value = "",
            //    Text = ""
            //};
            //OrgList.Add(test);

            var test1 = new SelectListItem
            {
                Value = "1",
                Text = "北海道"
            };
            OrgList.Add(test1);

            var test2 = new SelectListItem
                        {
                            Value = "2",
                            Text = "青森県"
                        };
            OrgList.Add(test2);
            var test3 = new SelectListItem
            {
                Value = "3",
                Text = "岩手県"
            };
            OrgList.Add(test3);
            var test4 = new SelectListItem
            {
                Value = "4",
                Text = "宮城県"
            };
            OrgList.Add(test4);
            var test5 = new SelectListItem
            {
                Value = "5",
                Text = "秋田県"
            };
            OrgList.Add(test5);
            var test6 = new SelectListItem
            {
                Value = "6",
                Text = "山形県"
            };
            OrgList.Add(test6);
            var test7 = new SelectListItem
            {
                Value = "7",
                Text = "福島県"
            };
            OrgList.Add(test7);
            var test8 = new SelectListItem
            {
                Value = "8",
                Text = "茨城県"
            };
            OrgList.Add(test8);
            var test9 = new SelectListItem
            {
                Value = "9",
                Text = "栃木県"
            };
            OrgList.Add(test9);
            var test10 = new SelectListItem
            {
                Value = "10",
                Text = "群馬県"
            };
            OrgList.Add(test10);
            var test11 = new SelectListItem
            {
                Value = "11",
                Text = "埼玉県"
            };
            OrgList.Add(test11);
            var test12 = new SelectListItem
            {
                Value = "12",
                Text = "千葉県"
            };
            OrgList.Add(test12);
            var test13 = new SelectListItem
            {
                Value = "13",
                Text = "東京都"
            };
            OrgList.Add(test13);
            var test14 = new SelectListItem
            {
                Value = "14",
                Text = "神奈川県"
            };
            OrgList.Add(test14);
            var test15 = new SelectListItem
            {
                Value = "15",
                Text = "新潟県"
            };
            OrgList.Add(test15);
            var test16 = new SelectListItem
            {
                Value = "16",
                Text = "富山県"
            };
            OrgList.Add(test16);
            var test17 = new SelectListItem
            {
                Value = "17",
                Text = "石川県"
            };
            OrgList.Add(test17);
            var test18 = new SelectListItem
            {
                Value = "18",
                Text = "福井県"
            };
            OrgList.Add(test18);
            var test19 = new SelectListItem
            {
                Value = "19",
                Text = "山梨県"
            };
            OrgList.Add(test19);
            var test20 = new SelectListItem
                        {
                            Value = "20",
                            Text = "長野県"
                        };
            OrgList.Add(test20);
            var test21 = new SelectListItem
                        {
                            Value = "21",
                            Text = "岐阜県"
                        };
            OrgList.Add(test21);
            var test22 = new SelectListItem
                        {
                            Value = "22",
                            Text = "静岡県"
                        };
            OrgList.Add(test22);
            var test23 = new SelectListItem
                        {
                            Value = "23",
                            Text = "愛知県"
                        };
            OrgList.Add(test23);
            var test24 = new SelectListItem
                        {
                            Value = "24",
                            Text = "三重県"
                        };
            OrgList.Add(test24);
            var test25 = new SelectListItem
                        {
                            Value = "25",
                            Text = "滋賀県"
                        };
            OrgList.Add(test25);
            var test26 = new SelectListItem
                        {
                            Value = "26",
                            Text = "京都府"
                        };
            OrgList.Add(test26);
            var test27 = new SelectListItem
                        {
                            Value = "27",
                            Text = "大阪府"
                        };
            OrgList.Add(test27);
            var test28 = new SelectListItem
                        {
                            Value = "28",
                            Text = "兵庫県"
                        };
            OrgList.Add(test28);
            var test29 = new SelectListItem
                        {
                            Value = "29",
                            Text = "奈良県"
                        };
            OrgList.Add(test29);
            var test30 = new SelectListItem
                        {
                            Value = "30",
                            Text = "和歌山県"
                        };
            OrgList.Add(test30);
            var test31 = new SelectListItem
                        {
                            Value = "31",
                            Text = "鳥取県"
                        };
            OrgList.Add(test31);
            var test32 = new SelectListItem
                        {
                            Value = "32",
                            Text = "島根県"
                        };
            OrgList.Add(test32);
            var test33 = new SelectListItem
                        {
                            Value = "33",
                            Text = "岡山県"
                        };
            OrgList.Add(test33);
            var test34 = new SelectListItem
                        {
                            Value = "34",
                            Text = "広島県"
                        };
            OrgList.Add(test34);
            var test35 = new SelectListItem
                        {
                            Value = "35",
                            Text = "山口県"
                        };
            OrgList.Add(test35);
            var test36 = new SelectListItem
                        {
                            Value = "36",
                            Text = "徳島県"
                        };
            OrgList.Add(test36);
            var test37 = new SelectListItem
                        {
                            Value = "37",
                            Text = "香川県"
                        };
            OrgList.Add(test37);
            var test38 = new SelectListItem
                        {
                            Value = "38",
                            Text = "愛媛県"
                        };
            OrgList.Add(test38);
            var test39 = new SelectListItem
                        {
                            Value = "39",
                            Text = "高知県"
                        };
            OrgList.Add(test39);
            var test40 = new SelectListItem
                        {
                            Value = "40",
                            Text = "福岡県"
                        };
            OrgList.Add(test40);
            var test41 = new SelectListItem
                        {
                            Value = "41",
                            Text = "佐賀県"
                        };
            OrgList.Add(test41);
            var test42 = new SelectListItem
                        {
                            Value = "42",
                            Text = "長崎県"
                        };
            OrgList.Add(test42);
            var test43 = new SelectListItem
                        {
                            Value = "43",
                            Text = "熊本県"
                        };
            OrgList.Add(test43);
            var test44 = new SelectListItem
                        {
                            Value = "44",
                            Text = "大分県"
                        };
            OrgList.Add(test44);
            var test45 = new SelectListItem
                        {
                            Value = "45",
                            Text = "宮崎県"
                        };
            OrgList.Add(test45);
            var test46 = new SelectListItem
                        {
                            Value = "46",
                            Text = "鹿児島県"
                        };
            OrgList.Add(test46);
            var test47 = new SelectListItem
                        {
                            Value = "47",
                            Text = "沖縄県"
                        };
            OrgList.Add(test47);
            return OrgList;
        }


        //public void InsertOrganizationModel(OrganizationModel model)
        //{
        //    using (var utility = DbUtility.GetInstance())
        //    {
        //        utility.BeginTransaction();

        //        var sql = string.Format(@"INSERT INTO m_organization VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
        //             model.OrganizationID, model.OfficeName, model.OfficeNameEn, model.StaffName, model.StaffNameEn, model.Postal, model.StateCode, model.Address1, model.Address2, model.AddressEn, model.MailAddress, model.PhoneNumber, model.FaxNumber, model.StartDate, model.EndDate, model.LastUserID, model.LastUpdatetime);

        //        utility.ExecuteNonQuery(sql);

        //        utility.Commit();
        //    }
        //}

        public void UpdateOrganizationModel(OrganizationModel model)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                var sql = @"
                            UPDATE `m_organization`
                            SET
                            `OrganizationCode` = @OrganizationCode,
                            `OfficeName` = @OfficeName,
                            `OfficeNameEn` = @OfficeNameEn,
                            `StaffName` = @StaffName,
                            `StaffNameEn` = @StaffNameEn,
                            `Postal` = @Postal,
                            `StateCode` = @StateCode,
                            `Address1` = @Address1,
                            `Address2` = @Address2,
                            `AddressEn` = @AddressEn,
                            `MailAddress` = @MailAddress,
                            `PhoneNumber` = @PhoneNumber,
                            `FaxNumber` = @FaxNumber,                            
                            `LastUserID` = @LastUserID,
                            `BatchSaveWithDayMode` = @BatchSaveWithDayMode,
                            `FarmPic` = @FarmPic,
                            `FarmNote` = @FarmNote,
                            `FishinggroundPic` = @FishinggroundPic,
                            `FishinggroundNote` = @FishinggroundNote,
                            `LastUpdatetime` = now()
                            WHERE `OrganizationID` = @OrganizationID;
                            ";

                utility.AddParameter("OrganizationID", model.OrganizationID);
                utility.AddParameter("OrganizationCode", model.OrganizationCode);
                utility.AddParameter("OfficeName", model.OfficeName);
                utility.AddParameter("OfficeNameEn", model.OfficeNameEn);
                utility.AddParameter("StaffName", model.StaffName);
                utility.AddParameter("StaffNameEn", model.StaffNameEn);
                utility.AddParameter("Postal", model.Postal);
                utility.AddParameter("StateCode", model.StateCode);
                utility.AddParameter("Address1", model.Address1);
                utility.AddParameter("Address2", model.Address2);
                utility.AddParameter("AddressEn", model.AddressEn);
                utility.AddParameter("MailAddress", model.MailAddress);
                utility.AddParameter("PhoneNumber", model.PhoneNumber);
                utility.AddParameter("FaxNumber", model.FaxNumber);
                //utility.AddParameter("StartDate", model.StartDate);
                //utility.AddParameter("EndDate", model.EndDate);
                utility.AddParameter("BatchSaveWithDayMode", model.BatchSaveWithDayMode);
                utility.AddParameter("FarmPic", model.FarmPic);
                utility.AddParameter("FarmNote", model.FarmNote);
                utility.AddParameter("FishinggroundPic", model.FishinggroundPic);
                utility.AddParameter("FishinggroundNote", model.FishinggroundNote);
                utility.AddParameter("LastUserID", model.LastUserID);
                //utility.AddParameter("LastUpdatetime", model.LastUpdatetime);
                utility.ExecuteNonQuery(sql);
                
                utility.Commit();
            }
        }

        public void AddNewOrganizationModel(OrganizationModel model)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                var sql = @"
                            INSERT INTO `m_organization`
                            (`OrganizationID`,
                            `OrganizationCode`,
                            `OfficeName`,
                            `OfficeNameEn`,
                            `StaffName`,
                            `StaffNameEn`,
                            `Postal`,
                            `StateCode`,
                            `Address1`,
                            `Address2`,
                            `AddressEn`,
                            `MailAddress`,
                            `PhoneNumber`,
                            `FaxNumber`,                            
                            `BatchSaveWithDayMode`,                            
                            `FarmPic`,
                            `FarmNote`,
                            `FishinggroundPic`,
                            `FishinggroundNote`, 
                            `LastUserID`,
                            `LastUpdatetime`)
                            VALUES
                            (uuid_short(),
                            @OrganizationCode,
                            @OfficeName,
                            @OfficeNameEn,
                            @StaffName,
                            @StaffNameEn,
                            @Postal,
                            @StateCode,
                            @Address1,
                            @Address2,
                            @AddressEn,
                            @MailAddress,
                            @PhoneNumber,
                            @FaxNumber,                            
                            @BatchSaveWithDayMode,                            
                            @FarmPic,
                            @FarmNote,
                            @FishinggroundPic,
                            @FishinggroundNote, 
                            @LastUserID,
                            now());
                        ";

                //utility.AddParameter("OrganizationID", model.OrganizationID);
                utility.AddParameter("OrganizationCode", model.OrganizationCode);
                utility.AddParameter("OfficeName", model.OfficeName);
                utility.AddParameter("OfficeNameEn", model.OfficeNameEn);
                utility.AddParameter("StaffName", model.StaffName);
                utility.AddParameter("StaffNameEn", model.StaffNameEn);
                utility.AddParameter("Postal", model.Postal);
                utility.AddParameter("StateCode", model.StateCode);
                utility.AddParameter("Address1", model.Address1);
                utility.AddParameter("Address2", model.Address2);
                utility.AddParameter("AddressEn", model.AddressEn);
                utility.AddParameter("MailAddress", model.MailAddress);
                utility.AddParameter("PhoneNumber", model.PhoneNumber);
                utility.AddParameter("FaxNumber", model.FaxNumber);
                //utility.AddParameter("StartDate", model.StartDate);
                //utility.AddParameter("EndDate", model.EndDate);
                utility.AddParameter("BatchSaveWithDayMode", model.BatchSaveWithDayMode);
                utility.AddParameter("FarmPic", model.FarmPic);
                utility.AddParameter("FarmNote", model.FarmNote);
                utility.AddParameter("FishinggroundPic", model.FishinggroundPic);
                utility.AddParameter("FishinggroundNote", model.FishinggroundNote);
                utility.AddParameter("LastUserID", model.LastUserID);
                //utility.AddParameter("LastUpdatetime", model.LastUpdatetime);

                utility.ExecuteNonQuery(sql);


                utility.Commit();
            }
        }


    }
}
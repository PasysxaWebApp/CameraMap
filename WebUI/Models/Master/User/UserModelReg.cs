using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedUtilitys.Models;
using SharedUtilitys.DataBases;
using System.Text;
using PacificSystem.Utility;
using System.Web.Mvc;
using CameraMap.Enums;
using CameraMap.Models.Master.Organization;
namespace CameraMap.Models.Master.User
{
    public class UserModelReg : BaseModel
    {
        public static UserModelReg GetInstance()
        {
            return new UserModelReg();
        }

        public List<UserModel> Items { get; set; }

        public void SetUserList(SystemRollEnum roll, string organizationID)
        {
            var lstResult = new List<UserModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                if (roll == SystemRollEnum.SysAdmin)
                {
                    sql.AppendLine("SELECT  ");
                    sql.AppendLine("    `m_user`.`Guid`, ");
                    sql.AppendLine("    `m_user`.`OrganizationID`, ");
                    sql.AppendLine("    `organization`.`OfficeName`, ");
                    sql.AppendLine("    `organization`.`OfficeNameEn`, ");
                    sql.AppendLine("    `m_user`.`LoginName`, ");
                    sql.AppendLine("    `m_user`.`Password`, ");
                    sql.AppendLine("    `m_user`.`UserName`, ");
                    sql.AppendLine("    `m_user`.`UserNameKana`, ");
                    sql.AppendLine("    `m_user`.`UserNameEn`, ");
                    sql.AppendLine("    `m_user`.`Authority`, ");
                    sql.AppendLine("    `m_user`.`DisplayNo`, ");
                    sql.AppendLine("    `m_user`.`DisplayFlag`, ");
                    sql.AppendLine("    `m_user`.`LastUserID`, ");
                    sql.AppendLine("    `m_user`.`LastUpdatetime` ");
                    sql.AppendLine(" FROM m_user ");
                    sql.AppendLine(" INNER JOIN `m_organization` as `organization` on `organization`.`OrganizationID`=`m_user`.`OrganizationID` ");
                    sql.AppendLine(" WHERE Authority >= 2");
                    sql.AppendLine(" Order By `m_user`.DisplayNo ASC;");
                }
                else
                {
                    sql.AppendLine("SELECT  ");
                    sql.AppendLine("    `m_user`.`Guid`, ");
                    sql.AppendLine("    `m_user`.`OrganizationID`, ");
                    sql.AppendLine("    `organization`.`OfficeName`, ");
                    sql.AppendLine("    `organization`.`OfficeNameEn`, ");
                    sql.AppendLine("    `m_user`.`LoginName`, ");
                    sql.AppendLine("    `m_user`.`Password`, ");
                    sql.AppendLine("    `m_user`.`UserName`, ");
                    sql.AppendLine("    `m_user`.`UserNameKana`, ");
                    sql.AppendLine("    `m_user`.`UserNameEn`, ");
                    sql.AppendLine("    `m_user`.`Authority`, ");
                    sql.AppendLine("    `m_user`.`DisplayNo`, ");
                    sql.AppendLine("    `m_user`.`DisplayFlag`, ");
                    sql.AppendLine("    `m_user`.`LastUserID`, ");
                    sql.AppendLine("    `m_user`.`LastUpdatetime` ");
                    sql.AppendLine(" FROM m_user ");
                    sql.AppendLine(" INNER JOIN `m_organization` as `organization` on `organization`.`OrganizationID`=`m_user`.`OrganizationID` ");
                    sql.AppendLine(" WHERE Authority <= 2 and `m_user`.OrganizationID = ?OrganizationID");
                    //sql.AppendLine(" AND DisplayFlag = 1");
                    sql.AppendLine("Order By `m_user`.DisplayNo ASC;");
                }
                utility.AddParameter("OrganizationID", organizationID);
                utility.ExecuteReaderModelList(sql.ToString(), lstResult);
            }

            Items = lstResult;
        }

        public UserModel GetUserModel(long guid)
        {
            return GetUserModel("", guid);
        }

        public UserModel GetUserModel(string organizationId, long guid)
        {
            var model = new UserModel();
            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("Guid", guid);
                utility.AddParameter("OrganizationID", organizationId);
                var sql = new StringBuilder();
                sql.AppendLine("SELECT  ");
                sql.AppendLine("    `m_user`.`Guid`, ");
                sql.AppendLine("    `m_user`.`OrganizationID`, ");
                sql.AppendLine("    `organization`.`OfficeName`, ");
                sql.AppendLine("    `organization`.`OfficeNameEn`, ");
                sql.AppendLine("    `m_user`.`LoginName`, ");
                sql.AppendLine("    `m_user`.`Password`, ");
                sql.AppendLine("    `m_user`.`UserName`, ");
                sql.AppendLine("    `m_user`.`UserNameKana`, ");
                sql.AppendLine("    `m_user`.`UserNameEn`, ");
                sql.AppendLine("    `m_user`.`Authority`, ");
                sql.AppendLine("    `m_user`.`DisplayNo`, ");
                sql.AppendLine("    `m_user`.`DisplayFlag`, ");
                sql.AppendLine("    `m_user`.`LastUserID`, ");
                sql.AppendLine("    `m_user`.`LastUpdatetime` ");
                sql.AppendLine(" FROM m_user ");
                sql.AppendLine(" INNER JOIN `m_organization` as `organization` on `organization`.`OrganizationID`=`m_user`.`OrganizationID` ");
                sql.AppendLine(" Where Guid = ?Guid");
                if (!string.IsNullOrEmpty(organizationId))
                {
                    sql.AppendLine(" And m_user.OrganizationID = ?OrganizationID");
                }
                utility.ExecuteReaderModel(sql.ToString(), model);
            }

            FillUnionOrganizations(model);

            return model;
        }

        public void FillUnionOrganizations(UserModel model)
        {
            model.UnionOrganizations = GetUnionOrganizations(model.Guid);
            //            using (var utility = DbUtility.GetInstance())
            //            {
            //                utility.AddParameter("Guid", model.Guid);
            //                var sql = new StringBuilder();
            //                sql.AppendLine(@"SELECT 
            //                                    uu.unionId as UnionId,
            //                                    uu.userId as UserId,
            //                                    org.OrganizationID,
            //                                    org.OfficeName 
            //                                FROM m_userunion as uu 
            //                                right join m_organization org 
            //                                    on org.OrganizationID=uu.OrganizationID and uu.userId=?Guid 
            //                                order by org.OrganizationID; ");
            //                utility.ExecuteReaderModelList(sql.ToString(), model.UnionOrganizations);
            //            }
        }

        public static List<UnionOrganization> GetUnionOrganizations(long guid)
        {
            var unionOrganizations = new List<UnionOrganization>();
            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("Guid", guid);
                var sql = new StringBuilder();
                sql.AppendLine(@"SELECT 
                                    uu.unionId as UnionId,
                                    uu.userId as UserId,
                                    org.OrganizationID,
                                    org.OfficeName,
                                    org.OfficeNameEn 
                                FROM m_userunion as uu 
                                right join m_organization org 
                                    on org.OrganizationID=uu.OrganizationID and uu.userId=?Guid 
                                order by org.OrganizationID; ");
                utility.ExecuteReaderModelList(sql.ToString(), unionOrganizations);
                return unionOrganizations;
            }
        }

        public UserModel GetUserModelByLoginName(string organizationId, string loginName)
        {
            var model = new UserModel();
            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("LoginName", loginName);
                utility.AddParameter("OrganizationID", organizationId);
                var sql = new StringBuilder();
                sql.AppendLine("SELECT  ");
                sql.AppendLine("    `m_user`.`Guid`, ");
                sql.AppendLine("    `m_user`.`OrganizationID`, ");
                sql.AppendLine("    `organization`.`OfficeName`, ");
                sql.AppendLine("    `organization`.`OfficeNameEn`, ");
                sql.AppendLine("    `m_user`.`LoginName`, ");
                sql.AppendLine("    `m_user`.`Password`, ");
                sql.AppendLine("    `m_user`.`UserName`, ");
                sql.AppendLine("    `m_user`.`UserNameKana`, ");
                sql.AppendLine("    `m_user`.`UserNameEn`, ");
                sql.AppendLine("    `m_user`.`Authority`, ");
                sql.AppendLine("    `m_user`.`DisplayNo`, ");
                sql.AppendLine("    `m_user`.`DisplayFlag`, ");
                sql.AppendLine("    `m_user`.`LastUserID`, ");
                sql.AppendLine("    `m_user`.`LastUpdatetime` ");
                sql.AppendLine(" FROM m_user ");
                sql.AppendLine(" INNER JOIN `m_organization` as `organization` on `organization`.`OrganizationID`=`m_user`.`OrganizationID` ");
                sql.AppendLine(" Where LoginName = ?LoginName");
                sql.AppendLine(" And m_user.OrganizationID = ?OrganizationID");
                utility.ExecuteReaderModel(sql.ToString(), model);
            }

            return model;
        }

        private long GetUUID(DbUtility utility)
        {
            string sql = @" select uuid_short() as UUID;";
            var list = utility.ExecuteReader(sql);
            foreach (var dic in list)
            {
                return Convert.ToInt64(dic["UUID"]);
            }
            return 0;
        }


        public void EditUser(UserModel model,SystemRollEnum roll)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();
                try
                {
                    var uuid = model.Guid;
                    if (model.Guid == 0)
                    {
                        uuid = GetUUID(utility);
                    }

                    utility.AddParameter("Guid", model.Guid);
                    utility.AddParameter("NewGuid", uuid);
                    utility.AddParameter("OrganizationID", model.OrganizationID);
                    utility.AddParameter("LoginName", model.LoginName);
                    utility.AddParameter("Password", model.Password);
                    utility.AddParameter("UserName", model.UserName);
                    utility.AddParameter("UserNameKana", model.UserNameKana);
                    utility.AddParameter("UserNameEn", model.UserNameEn);
                    utility.AddParameter("Authority", model.Authority);
                    utility.AddParameter("DisplayFlag", model.DisplayFlag ? 1 : 0);
                    utility.AddParameter("LastUserID", model.LastUserID);
                    var sql = new StringBuilder();
                    if (model.Guid > 0)
                    {
                        sql.AppendLine("UPDATE `m_user`");
                        sql.AppendLine("SET ");
                        sql.AppendLine("`LoginName` = @LoginName,");
                        sql.AppendLine("`Password` = @Password,");
                        sql.AppendLine("`UserName` = @UserName,");
                        sql.AppendLine("`UserNameKana` = @UserNameKana,");
                        sql.AppendLine("`UserNameEn` = @UserNameEn,");
                        sql.AppendLine("`Authority` = @Authority,");
                        sql.AppendLine("`OrganizationID` = @OrganizationID,");
                        sql.AppendLine("`DisplayFlag` = @DisplayFlag,");
                        sql.AppendLine("`LastUserID` = @LastUserID,");
                        sql.AppendLine("`LastUpdatetime` = now()");
                        sql.AppendLine("WHERE Guid = @Guid");
                    }
                    else
                    {
                        sql.AppendLine("INSERT INTO `m_user`");
                        sql.AppendLine("(`Guid`,");
                        sql.AppendLine("`OrganizationID`,");
                        sql.AppendLine("`LoginName`,");
                        sql.AppendLine("`Password`,");
                        sql.AppendLine("`UserName`,");
                        sql.AppendLine("`UserNameKana`,");
                        sql.AppendLine("`UserNameEn`,");
                        sql.AppendLine("`Authority`,");
                        sql.AppendLine("`DisplayNo`,");
                        sql.AppendLine("`DisplayFlag`,");
                        sql.AppendLine("`LastUserID`,");
                        sql.AppendLine("`LastUpdatetime`)");
                        sql.AppendLine("VALUES");
                        sql.AppendLine("(");
                        sql.AppendLine("?NewGuid,");
                        sql.AppendLine("?OrganizationID,");
                        sql.AppendLine("?LoginName,");
                        sql.AppendLine("?Password,");
                        sql.AppendLine("?UserName,");
                        sql.AppendLine("?UserNameKana,");
                        sql.AppendLine("?UserNameEn,");
                        sql.AppendLine("?Authority,");
                        sql.AppendLine("(SELECT ifnull(max(DisplayNo),0) + 1 from m_user myuser),");
                        sql.AppendLine("@DisplayFlag,");
                        sql.AppendLine("?LastUserID,");
                        sql.AppendLine("now()");
                        sql.AppendLine(");");
                    }

                    utility.ExecuteNonQuery(sql.ToString());
                    if (roll == SystemRollEnum.SysAdmin)
                    {
                        if (model.Guid > 0)
                        {
                            sql.Clear();
                            sql.Append(@"Delete from `m_userunion` where userId=@userId;");
                            utility.AddParameter("userId", model.Guid);
                            utility.ExecuteNonQuery(sql.ToString());
                        }
                        else
                        {
                            model.Guid = uuid;
                        }

                        foreach (var uo in model.UnionOrganizations)
                        {
                            if (!uo.Checked)
                            {
                                continue;
                            }
                            sql.Clear();
                            sql.Append(@"INSERT INTO `m_userunion`
                                    (`unionId`,
                                    `userId`,
                                    `organizationId`)
                                 VALUES
                                    (uuid_short(),
                                    @userId,
                                    @OrganizationID);");
                            utility.AddParameter("userId", model.Guid);
                            utility.AddParameter("OrganizationID", uo.OrganizationID);
                            utility.ExecuteNonQuery(sql.ToString());
                        }
                    }
                    utility.Commit();
                }
                catch (Exception)
                {
                    utility.Rollback();
                }
            }
        }

        public void DeleteUser(string organizationId, long guid)
        {
            if (!DeleteCheck(organizationId, guid))
            {
                return;
            }

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("OrganizationID", organizationId);
                utility.AddParameter("Guid", guid);
                var sql = new StringBuilder();
                sql.AppendLine("DELETE FROM m_user");
                sql.AppendLine("Where Guid = ?Guid");
                if (!string.IsNullOrEmpty(organizationId))
                {
                    sql.AppendLine("and OrganizationID=?OrganizationID");
                }
                utility.ExecuteNonQuery(sql.ToString());
            }
        }

        public bool DeleteCheck(string organizationId, long guid)
        {
            bool bl = true;
            if (guid == 0)
            {
                return false;
            }

            var tables = new List<string>();
            tables.Add("SELECT * FROM m_accountstitle where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_adjustreason where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_baitstatus where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_checkitem where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_deadreason where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_dispose where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_dosageset where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_dosagesetitem where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_dosagesharpe where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_environment where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_feed where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_feeddiv where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_feedkind where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishgroup where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishingground where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishkind where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishspecies where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishspeciesgroup where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_fishspeciesshape where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_inviteuser where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_inviteuserbrowse where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_medicine where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_mpset where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_mpsetitem where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_net where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_nutrient where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_organization where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_publicfishkind where  LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_seedintrorecode where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_seedkind where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_seedorigin where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_settingdata where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_sharpe where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_shelfregistrationitem where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_shipmentto where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_stockto where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_transport where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_user where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM m_workitem where LastUserID=?Guid limit 1;");

            tables.Add("SELECT * FROM t_accesslog where UserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_adjust where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_check where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_dead where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_disaster where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_division where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_dosage where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_environment where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_farmrecord where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_fishspecies where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_fishspeciescost where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_indirectcost where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_news where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_notification where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_schedule where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_shipment where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_tablehistory where LastUserID=?Guid limit 1;");
            tables.Add("SELECT * FROM t_work where LastUserID=?Guid limit 1;");


            using (var utility = DbUtility.GetInstance())
            {
                foreach (var sql in tables)
                {
                    utility.AddParameter("OrganizationID", organizationId);
                    utility.AddParameter("Guid", guid);
                    var rl = utility.ExecuteReader(sql.ToString());
                    if (rl.Count() > 0)
                    {
                        bl = false;
                        break;
                    }
                }
            }
           
            return bl;
        }
    }
}
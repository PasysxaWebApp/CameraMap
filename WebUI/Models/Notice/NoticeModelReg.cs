using CameraMap.Enums;
using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Notice
{
    public class NoticeModelReg
    {
        public NoticeModel GetModel()
        {
            return null;
        }

        public static List<NoticeModel> GetNoticeListForEdit(SystemRollEnum roll, string organizationID, int PageIndex, int PageSize)
        {
            var lstResult = new List<NoticeModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        `t_notice`.`NoticeID`,
                        `t_notice`.`OrganizationID`,
                        `t_notice`.`Title`,
                        `t_notice`.`ContentType`,
                        `t_notice`.`ContentDesc`,
                        `t_notice`.`ContentTxt`,
                        `t_notice`.`CreateUserID`,
                        `t_notice`.`CreateDateTime`,
                        `t_notice`.`LastUserID`,
                        `t_notice`.`LastUpdatetime`,
                        `t_notice`.`Sticky`,
                        `t_notice`.`NoticeDateTime`,
                        createUser.UserName as CreateUserName,
                        updateUser.UserName as UpdateUserName
                    FROM `t_notice` 
                    left join m_user createUser on createUser.Guid=`t_notice`.`CreateUserID`
                    left join m_user updateUser on updateUser.Guid=`t_notice`.`LastUserID`
                    where ");
                if (roll == SystemRollEnum.SysAdmin)
                {
                    sql.Append(@"
                    (`t_notice`.`OrganizationID` is null or `t_notice`.`OrganizationID`=@OrganizationID)");
                }
                else
                {
                    sql.Append(@"
                    `t_notice`.`OrganizationID`=@OrganizationID");
                }

                sql.Append(@"
                    order by `t_notice`.`Sticky` desc,`t_notice`.`DisplayNo`,`t_notice`.`NoticeDateTime` desc ");

                sql.AppendFormat(@"
                    limit {0},{1} ", PageIndex * PageSize, PageSize);

                sql.Append(@";");

                utility.AddParameter("OrganizationID", organizationID);
                utility.ExecuteReaderModelList(sql.ToString(), lstResult);

                return lstResult;
            }

        }

        public static List<NoticeModel> GetNoticeListForOrder(SystemRollEnum roll, string organizationID)
        {
            var lstResult = new List<NoticeModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        `t_notice`.`NoticeID`,
                        `t_notice`.`OrganizationID`,
                        `t_notice`.`Title`,
                        `t_notice`.`ContentType`,
                        `t_notice`.`ContentDesc`,
                        `t_notice`.`ContentTxt`,
                        `t_notice`.`CreateUserID`,
                        `t_notice`.`CreateDateTime`,
                        `t_notice`.`LastUserID`,
                        `t_notice`.`LastUpdatetime`,
                        `t_notice`.`Sticky`,
                        `t_notice`.`NoticeDateTime`,
                        createUser.UserName as CreateUserName,
                        updateUser.UserName as UpdateUserName
                    FROM `t_notice` 
                    left join m_user createUser on createUser.Guid=`t_notice`.`CreateUserID`
                    left join m_user updateUser on updateUser.Guid=`t_notice`.`LastUserID`
                    where ");
                if (roll == SystemRollEnum.SysAdmin)
                {
                    sql.Append(@"
                    (`t_notice`.`OrganizationID` is null or `t_notice`.`OrganizationID`=@OrganizationID)");
                }
                else
                {
                    sql.Append(@"
                    `t_notice`.`OrganizationID`=@OrganizationID");
                }

                sql.Append(@"
                     and `t_notice`.`Sticky`=1 ");

                sql.Append(@"
                    order by `t_notice`.`Sticky` desc,`t_notice`.`DisplayNo`,`t_notice`.`NoticeDateTime` desc ");

                sql.Append(@";");

                utility.AddParameter("OrganizationID", organizationID);
                utility.ExecuteReaderModelList(sql.ToString(), lstResult);

                return lstResult;
            }
        }


        //public static int GetNoticeListForEditPageCount(SystemRollEnum roll, string organizationID, int PageSize)
        //{
        //    var noticeCount=GetNoticeListForEditCount( roll,  organizationID);
        //    var pageCount = noticeCount / PageSize;
        //    if (noticeCount % PageSize != 0)
        //    {
        //        pageCount++;
        //    }
        //    return pageCount;
        //}

        public static int GetNoticeListForEditCount(SystemRollEnum roll, string organizationID)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        Count(`t_notice`.`NoticeID`) as NoticeCount
                    FROM `t_notice` 
                    where ");
                if (roll == SystemRollEnum.SysAdmin)
                {
                    sql.Append(@"
                    (`t_notice`.`OrganizationID` is null or `t_notice`.`OrganizationID`=@OrganizationID)");
                }
                else
                {
                    sql.Append(@"
                    `t_notice`.`OrganizationID`=@OrganizationID");
                }

                sql.Append(@";");

                utility.AddParameter("OrganizationID", organizationID);
                var list = utility.ExecuteReader(sql.ToString());
                foreach (var dic in list)
                {
                    var noticeCount = Converts.ToTryInt(dic["NoticeCount"], 0);
                    return noticeCount;
                }
            }
            return 0;
        }

        public static List<NoticeModel> GetNoticeList(string organizationID,int PageIndex,int PageSize)
        {
            var lstResult = new List<NoticeModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        `t_notice`.`NoticeID`,
                        `t_notice`.`OrganizationID`,
                        `t_notice`.`Title`,
                        `t_notice`.`ContentType`,
                        `t_notice`.`ContentDesc`,
                        `t_notice`.`ContentTxt`,
                        `t_notice`.`CreateUserID`,
                        `t_notice`.`CreateDateTime`,
                        `t_notice`.`LastUserID`,
                        `t_notice`.`LastUpdatetime`,
                        `t_notice`.`NoticeDateTime`,
                        createUser.UserName as CreateUserName,
                        updateUser.UserName as UpdateUserName
                    FROM `t_notice` 
                    left join m_user createUser on createUser.Guid=`t_notice`.`CreateUserID`
                    left join m_user updateUser on updateUser.Guid=`t_notice`.`LastUserID`
                    where ");
                if (string.IsNullOrEmpty(organizationID))
                {
                    sql.Append(@"
                    `t_notice`.`OrganizationID` is null");
                }
                else
                {
                    sql.Append(@"
                    (`t_notice`.`OrganizationID` is null || `t_notice`.`OrganizationID`=@OrganizationID)");
                }

                sql.Append(@"
                    order by `t_notice`.`Sticky` desc,`t_notice`.`DisplayNo`,`t_notice`.`NoticeDateTime` desc ,`t_notice`.`NoticeID` desc");

                sql.AppendFormat(@"
                    limit {0},{1} ", PageIndex * PageSize, PageSize);

                sql.Append(@";");

                utility.AddParameter("OrganizationID", organizationID);
                utility.ExecuteReaderModelList(sql.ToString(), lstResult);

                return lstResult;
            }


        }

        public static int GetNoticeListPageCount(string organizationID, int PageSize)
        {
            var pageCount = 0;
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT 
                        Count(`t_notice`.`NoticeID`) as NoticeCount
                    FROM `t_notice`
                    where ");
                if (string.IsNullOrEmpty(organizationID))
                {
                    sql.Append(@"
                    `t_notice`.`OrganizationID` is null");
                }
                else
                {
                    sql.Append(@"
                    (`t_notice`.`OrganizationID` is null || `t_notice`.`OrganizationID`=@OrganizationID)");
                }

                sql.Append(@";");

                utility.AddParameter("OrganizationID", organizationID);
                var list = utility.ExecuteReader(sql.ToString());
                foreach (var dic in list)
                {
                    var noticeCount = Converts.ToTryInt(dic["NoticeCount"], 0);
                    pageCount = noticeCount / PageSize;
                    if (noticeCount % PageSize != 0)
                    {
                        pageCount++;
                    }
                    break;
                }
            }
            return pageCount;
        }


        public static NoticeModel GetNoticeModel(long noticeID)
        {
            var model = new NoticeModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                sql.Append(@"
                    SELECT `t_notice`.`NoticeID`,
                        `t_notice`.`OrganizationID`,
                        `t_notice`.`Title`,
                        `t_notice`.`NoticeDateTime`,
                        `t_notice`.`AttachmentFile1` as AttachmentFileID1,
                        `t_notice`.`ContentType`,
                        `t_notice`.`ContentDesc`,
                        `t_notice`.`ContentTxt`,
                        `t_notice`.`StartDate`,
                        `t_notice`.`EndDate`,
                        `t_notice`.`CreateUserID`,
                        `t_notice`.`CreateDateTime`,
                        `t_notice`.`LastUserID`,
                        `t_notice`.`LastUpdatetime`,
                        `t_notice`.`Sticky`,
                        createUser.UserName as CreateUserName,
                        updateUser.UserName as UpdateUserName,
                        files.FileName as AttachmentFileFileName1
                    FROM `t_notice` 
                    left join m_user createUser on createUser.Guid=`t_notice`.`CreateUserID`
                    left join m_user updateUser on updateUser.Guid=`t_notice`.`LastUserID`
                    left join t_files files     on files.FilesID=`t_notice`.`AttachmentFile1`
                    where `t_notice`.`NoticeID`=@NoticeID;
                ");

                utility.AddParameter("NoticeID", noticeID);
                utility.ExecuteReaderModel(sql.ToString(), model);
                return model;
            }
        }

        public static long GetUUID()
        {
            using (var utility = DbUtility.GetInstance())
            {
                string sql = @" select uuid_short() as UUID;";

                var list = utility.ExecuteReader(sql);
                foreach (var dic in list)
                {
                    return Convert.ToInt64(dic["UUID"]);
                }
                return 0;
            }
        }

        public static bool Save(NoticeModel model, SystemRollEnum roll)
        {
            var noticeID = model.NoticeID;
            var displayNo = 0;
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();
                if (model.NoticeID == 0)
                {
                    sql.Append(@"
                    INSERT INTO `t_notice`
                        (`NoticeID`, ");
                    if (roll != SystemRollEnum.SysAdmin)
                    {
                        sql.Append(@"
                        `OrganizationID`,");
                    }
                    sql.Append(@"
                        `Sticky`,
                        `DisplayNo`,
                        `Title`,
                        `NoticeDateTime`,
                        `AttachmentFile1`,
                        `ContentType`,
                        `ContentDesc`,
                        `ContentTxt`,
                        `StartDate`,
                        `EndDate`,
                        `CreateUserID`,
                        `CreateDateTime`,
                        `LastUserID`,
                        `LastUpdatetime`)
                        VALUES
                        (@NoticeID,");
                    if (roll != SystemRollEnum.SysAdmin)
                    {
                        sql.Append(@"
                        @OrganizationID,");
                    }
                    sql.Append(@"
                        @Sticky,
                        @DisplayNo,
                        @Title,
                        @NoticeDateTime,
                        @AttachmentFile1,
                        @ContentType,
                        @ContentDesc,
                        @ContentTxt,
                        @StartDate,
                        @EndDate,
                        @CreateUserID,
                        @CreateDateTime,
                        @LastUserID,
                        @LastUpdatetime);");

                    noticeID = GetUUID();
                }
                else
                {
                    sql.Append(@"
                        UPDATE `t_notice`
                        SET
                        `Sticky` = @Sticky,
                        `DisplayNo` = @DisplayNo,
                        `Title` = @Title,
                        `NoticeDateTime` = @NoticeDateTime,");
                    if (model.AttachmentFileID1 > 0)
                    {
                        sql.Append(@"
                        `AttachmentFile1` = @AttachmentFile1,");
                    }
                    else if (model.AttachmentFileID1 == -1)
                    {
                        sql.Append(@"
                        `AttachmentFile1` = 0,");
                    }
                    if (roll == SystemRollEnum.SysAdmin)
                    {
                        sql.Append(@"
                        OrganizationID=null,");
                    }
                    sql.Append(@"
                        `ContentType` = @ContentType,
                        `ContentDesc` = @ContentDesc,
                        `ContentTxt` = @ContentTxt,
                        `LastUserID` = @LastUserID,
                        `LastUpdatetime` = @LastUpdatetime
                        WHERE `NoticeID` = @NoticeID and CreateUserID=@CreateUserID;");
                    if (model.Sticky)
                    {
                        displayNo = model.DisplayNo;
                    }
                }

                utility.AddParameter("NoticeID", noticeID);
                
                utility.AddParameter("OrganizationID", model.OrganizationID);
                utility.AddParameter("Sticky", model.Sticky);
                utility.AddParameter("DisplayNo", displayNo);
                utility.AddParameter("Title", model.Title);
                utility.AddParameter("NoticeDateTime", model.NoticeDateTime);
                utility.AddParameter("AttachmentFile1", model.AttachmentFileID1);
                utility.AddParameter("ContentType", model.ContentType);
                utility.AddParameter("ContentDesc", model.ContentDesc);
                utility.AddParameter("ContentTxt", model.ContentTxt);
                utility.AddParameter("StartDate", model.StartDate);
                utility.AddParameter("EndDate", model.EndDate);
                utility.AddParameter("CreateUserID", model.CreateUserID);
                utility.AddParameter("CreateDateTime", model.CreateDateTime);
                utility.AddParameter("LastUserID", model.LastUserID);
                utility.AddParameter("LastUpdatetime", model.LastUpdatetime);
                var effRow = utility.ExecuteNonQuery(sql.ToString());
                if (effRow == 1 && model.NoticeID == 0)
                {
                    model.NoticeID = noticeID;
                }
                return effRow == 1;
            }
        }

        public static bool Delete(long noticeID,long userID)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = new StringBuilder();

                sql.Append(@"
                        Delete from `t_notice`                       
                        WHERE `NoticeID` = @NoticeID 
                        and CreateUserID=@CreateUserID;");

                utility.AddParameter("NoticeID", noticeID);
                utility.AddParameter("CreateUserID", userID);
                var effRow = utility.ExecuteNonQuery(sql.ToString());
                return effRow == 1;
            }
        }


    }
}

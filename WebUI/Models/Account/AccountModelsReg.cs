using CameraMap.Enums;
using CameraMap.Models.Master.Account;
using CameraMap.Models.Master.User;
using CameraMap.Sessions;
using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Account
{
    class AccountModelsReg
    {
        public static AccountModelsReg GetInstance()
        {
            return new AccountModelsReg();
        }

        public UserModel GetUserModel(LogOnModel logOnmodel)
        {
            using (var utility = DbUtility.GetInstance())
            {
                StringBuilder sql = new StringBuilder();
                    sql.Append( @" 
                                SELECT `m_user`.`Guid`,
                                    `m_user`.`OrganizationID`,
                                    `m_user`.`LoginName`,
                                    `m_user`.`Password`,
                                    `m_user`.`UserName`,
                                    `m_user`.`UserNameKana`,
                                    `m_user`.`UserNameEn`,
                                    `m_user`.`Authority`,
                                    `m_user`.`DisplayNo`,
                                    `m_user`.`DisplayFlag`,
                                    `m_user`.`LastUserID`,
                                    `m_user`.`LastUpdatetime`
                                FROM `m_user`
                                where   LoginName=@LoginName
                                and     Password=@Password ");
                if (!string.IsNullOrEmpty(logOnmodel.OrganizationID))
                {
                    sql.Append(@" 
                                and OrganizationID=@OrganizationID;");
                }
                
                utility.AddParameter("LoginName", logOnmodel.UserID);
                utility.AddParameter("Password", logOnmodel.Password);
                utility.AddParameter("OrganizationID", logOnmodel.OrganizationID);
                UserModel model = new UserModel();
                utility.ExecuteReaderModel(sql.ToString(), model);
                if (model.Guid <= 0)
                {
                    return new UserModel();
                }                

                if (model.Authority ==(int)SystemRollEnum.SysAdmin)
                {
                    return model;
                }
                else
                {
                    if (string.IsNullOrEmpty(logOnmodel.OrganizationID))
                    {
                        UserModelReg.GetInstance().FillUnionOrganizations(model);
                        if (model.UnionOfficeUser)
                        {
                            model.Authority = (int)SystemRollEnum.UnionOfficeUser;
                            return model;
                        }
                        else {
                            return new UserModel();
                        }
                    }
                    else {
                        return model;
                    }
                }
                 
            }

        }

        /// <summary>
        /// add logs
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddAccessLog(UserModel model)
        {
            try
            {
                using (var utility = DbUtility.GetInstance())
                {
                    string sql = @"
                             insert into `t_accesslog`
                             (`Guid`,`OrganizationID`,`userId` ,`Detail`,`LoggingDate`) VALUES
                            (uuid_short(),@OrganizationID,@userId,@Detail,now())
                              ";

                    utility.AddParameter("OrganizationID", model.OrganizationID);
                    utility.AddParameter("userId", model.Guid);
                    utility.AddParameter("Detail", "");

                    var result = utility.ExecuteNonQuery(sql);
                    if (result > 0)
                    {
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}

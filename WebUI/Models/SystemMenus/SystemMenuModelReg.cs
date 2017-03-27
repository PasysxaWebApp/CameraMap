using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.SystemMenus
{
    public class SystemMenuModelReg
    {
        public static List<SystemMenuModel> GetMenus(int UserRole)
        {

            var list = new List<SystemMenuModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var Sql = new StringBuilder();

                Sql.AppendFormat(" SELECT ");
                Sql.AppendFormat("      M1.MenuID, M1.MenuName, M1.ParentID, ifnull(M1.ActionName,'') as ActionName , ifnull(M1.ControllerName,'') as ControllerName , M2.UserRole,ifnull(M1.CssClass,'') as CssClass  ");
                Sql.AppendFormat(" FROM ");
                Sql.AppendFormat("      m_systemmenu M1  ");
                Sql.AppendFormat(" INNER JOIN ");
                Sql.AppendFormat("      m_systemmenu_roll M2 ON  ");
                Sql.AppendFormat("      M2.MenuID = M1.MenuID  ");
                //Sql.AppendFormat(" WHERE ");
                //Sql.AppendFormat("      M2.UserRole=?UserRole ");
                Sql.AppendFormat(" ORDER BY M2.DisplayNo,M1.MenuID ");


                utility.AddParameter("UserRole", UserRole);
                utility.ExecuteReaderModelList(Sql.ToString(), list);
            }

            return list;
        }

    }
}

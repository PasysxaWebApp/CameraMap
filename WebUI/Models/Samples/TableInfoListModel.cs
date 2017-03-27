using System;
using System.Collections.Generic;
using CameraMap.Models.Base;
using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using SharedUtilitys.Models;

namespace CameraMap.Models.Samples
{
    public class TableInfoListModel : BaseModel
    {
        public static TableInfoListModel GetInstance()
        {
            return new TableInfoListModel();
        }

        public IEnumerable<string> GetTableNames()
        {
            var result = new List<String>();

            using (var utility = DbUtility.GetInstance())
            {
                const string sql = @"SHOW TABLES FROM CameraMap";

                foreach (var item in utility.ExecuteReader(sql))
	            {
                    result.Add(Converts.ToTryString(item["Tables_in_cloudfarm"]));
	            }

                return result;
            }
        }
    }
}
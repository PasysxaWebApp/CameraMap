using System.Collections.Generic;
using System.Linq;
using PacificSystem.Utility;
using SharedUtilitys.Attributes;
using SharedUtilitys.DataBases;
using SharedUtilitys.Models;

namespace CameraMap.Models.Samples
{
    public class TableInfoModel : BaseModel
    {
        public string TableName { get; set; }
        [AutoProperty]
        public string Engine { get; set; }
        [AutoProperty]
        public string Collation { get; set; }
        public int TableRowCount { get; set; }

        public List<ColumnInfoModel> ColumnInfos { get; set; }

        public TableInfoModel(string tableName)
        {
            TableName = tableName;

            SetDictionary(GetTableStatus(tableName));
            TableRowCount = GetTableRowCount(TableName);
            ColumnInfos = GetColumnInfos(TableName);
        }

        public Dictionary<string, object> GetTableStatus(string tableName)
        {
            using (var utility = DbUtility.GetInstance())
            {
                const string sql = @"SHOW TABLE STATUS FROM CameraMap WHERE name = @tableName ";

                utility.AddParameter("tableName", tableName);

                return utility.ExecuteReader(sql).First();
            }
        }

        public int GetTableRowCount(string tableName)
        {
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"SELECT COUNT(*) AS TableRowCount FROM " + tableName;

                return Converts.ToTryInt(utility.ExecuteReader(sql).First()["TableRowCount"]);
            }
        }

        public List<ColumnInfoModel> GetColumnInfos(string tableName)
        {
            var result = new List<ColumnInfoModel>();

            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"SHOW COLUMNS FROM " + tableName;

                foreach (var item in utility.ExecuteReader(sql))
                {
                    result.Add(new ColumnInfoModel(item));
                }

                return result;
            }
        }
    }
}
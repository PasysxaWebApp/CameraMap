using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Files
{
    public class FileModelReg
    {
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

        public static Int64 Insert(string fileName, byte[] fileData)
        {
            var sql = @"
            INSERT INTO `t_files`
            (`FilesID`,
            `FileName`,
            `FileData`)
            VALUES
            (@FilesID,
            @FileName,
            @FileData);
            ";
            var fileID = GetUUID();
            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("FilesID", fileID);
                utility.AddParameter("FileName", fileName);
                utility.AddParameter("FileData", fileData);
                var effRow = utility.ExecuteNonQuery(sql);
                if (effRow == 1)
                {
                    return fileID;
                }
                else
                    return 0;
            }
        }

        public static bool Update(long fileID, string fileName, byte[] fileData)
        {
            var sql = @"
             UPDATE `t_files`
                SET
                `FileName` = @FileName,
                `FileData` =@FileData
                WHERE `FilesID` = @FilesID;";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("FilesID", fileID);
                utility.AddParameter("FileName", fileName);
                utility.AddParameter("FileData", fileData);
                var effRow = utility.ExecuteNonQuery(sql);
                return effRow == 1;
            }
        }

        public static FileModel GetFileModelByID(long fileID)
        {
            FileModel model = new FileModel();
            var sql = @"
                 SELECT `t_files`.`FilesID` as FileID,
                    `t_files`.`FileName`,
                    `t_files`.`FileData`
                FROM `t_files`
                WHERE `t_files`.`FilesID` = @FilesID;";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("FilesID", fileID);
                utility.ExecuteReaderModel(sql, model);
                return model;
            }
        }

    }
}

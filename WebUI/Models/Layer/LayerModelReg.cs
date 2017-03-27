using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Layer
{
    class LayerModelReg
    {
        public static List<LayerModel> GetModels(string OrganizationID)
        {
            var models = new List<LayerModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT m_layers.Id,
                        m_layers.OrganizationID,
                        m_layers.LayerKey,
                        m_layers.LayerName,
                        m_layers.IconFile,
                        t_files.FileName as IconFileName,
                        m_layers.DisplayNo,
                        m_layers.DisplayFlag,
                        m_layers.LastUserID,
                        m_layers.LastUpdatetime
                    FROM m_layers left join t_files on t_files.FilesID=m_layers.IconFile
                    Where m_layers.OrganizationID=@OrganizationID;
                ";

                utility.AddParameter("OrganizationID", OrganizationID);
                utility.ExecuteReaderModelList(sql, models);
            }
            return models;
        }

        public static LayerModel GetModel(long Id)
        {
            var model = new LayerModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT m_layers.Id,
                        m_layers.OrganizationID,
                        m_layers.LayerKey,
                        m_layers.LayerName,
                        m_layers.IconFile,
                        t_files.FileName as IconFileName,
                        m_layers.DisplayNo,
                        m_layers.DisplayFlag,
                        m_layers.LastUserID,
                        m_layers.LastUpdatetime
                    FROM m_layers left join t_files on t_files.FilesID=m_layers.IconFile
                    Where m_layers.Id=@Id;
                ";

                utility.AddParameter("Id", Id);
                utility.ExecuteReaderModel(sql, model);
            }
            return model;
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


        public static bool Add(LayerModel item)
        {
            var sql = @"
                   INSERT INTO m_layers
                    (Id,
                    OrganizationID,
                    LayerKey,
                    LayerName,
                    IconFile,
                    DisplayNo,
                    DisplayFlag,
                    LastUserID,
                    LastUpdatetime)
                    VALUES
                    (@Id,
                    @OrganizationID,
                    @LayerKey,
                    @LayerName,
                    @IconFile,
                    0,
                    1,
                    @LastUserID,
                    now());
                ";
            var id = GetUUID();
            using (var utility = DbUtility.GetInstance())
            {
                try
                {
                    utility.AddParameter("Id",id  );
                    utility.AddParameter("OrganizationID", item.OrganizationID);
                    utility.AddParameter("LayerKey", item.LayerKey);
                    utility.AddParameter("LayerName", item.LayerName);
                    utility.AddParameter("IconFile", item.IconFile);
                    utility.AddParameter("LastUserID", item.LastUserID);
                    utility.ExecuteNonQuery(sql);
                    item.Id = id;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool Delete(LayerModel item)
        {
            var sql = @" 
                    DELETE FROM m_devices WHERE LayerId=@Id;
                    DELETE FROM m_layers WHERE Id=@Id;
                    DELETE FROM cameramap.t_files WHERE FilesID=@IconFile ; 
                    ";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("Id", item.Id);
                utility.AddParameter("IconFile", item.IconFile);
                return utility.ExecuteNonQuery(sql.ToString()) >= 1;
            }
        }

        public static bool Update(LayerModel item)
        {
            var sql = @" 
                UPDATE m_layers
                SET
                LayerName = @LayerName,
                IconFile = @IconFile,
                LastUserID = @LastUserID,
                LastUpdatetime = now()
                WHERE Id = @Id;";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("Id", item.Id);
                utility.AddParameter("LayerName", item.LayerName);
                utility.AddParameter("IconFile", item.IconFile);
                utility.AddParameter("LastUserID", item.LastUserID);
                return utility.ExecuteNonQuery(sql) == 1;
            }
        }
    }
}

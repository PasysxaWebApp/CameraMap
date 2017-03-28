using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Device
{
    class LineModelReg
    {
        public static List<LineModel> GetModels(long LayerId)
        {
            var models = new List<LineModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                        SELECT 
	                        m_lines.LineId,
                            m_lines.OrganizationID,
                            m_lines.LayerId,
                            m_lines.LineKey,
                            m_lines.LineName,
                            m_lines.Note,
                            m_lines.Points,
                            m_lines.DisplayNo,
                            m_lines.DisplayFlag,
                            m_lines.LastUserID,
                            m_lines.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_lines
                        inner join m_layers on m_layers.Id=m_lines.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        where m_lines.LayerId=@LayerId
                        ;";

                utility.AddParameter("LayerId", LayerId);
                utility.ExecuteReaderModelList(sql, models);
            }
            return models;
        }

        public static List<LineModel> GetModels(string LayerIds)
        {
            var lays = LayerIds.Split(new char[] { ',' });
            var formatedLayerIds = lays.Select(m => ConvertToInt64(m)).Aggregate((c, n) => c + "," + n);
            if (string.IsNullOrEmpty(formatedLayerIds))
            {
                return null;
            }
            var models = new List<LineModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = string.Format(@"
                        SELECT 
	                        m_lines.LineId,
                            m_lines.OrganizationID,
                            m_lines.LayerId,
                            m_lines.LineKey,
                            m_lines.LineName,
                            m_lines.Note,
                            m_lines.Points,
                            m_lines.DisplayNo,
                            m_lines.DisplayFlag,
                            m_lines.LastUserID,
                            m_lines.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_lines
                        inner join m_layers on m_layers.Id=m_lines.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        where m_lines.LayerId in ({0})
                        ;", formatedLayerIds);

                utility.ExecuteReaderModelList(sql, models);
            }
            return models;
        }

        private static string ConvertToInt64(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "0";
            }
            long l;
            if (long.TryParse(str, out l))
            {
                return l.ToString();
            }
            else
            {
                return "0";
            }
        }

        public static LineModel GetModel(long LineId)
        {
            var model = new LineModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                        SELECT 
	                        m_lines.LineId,
                            m_lines.OrganizationID,
                            m_lines.LayerId,
                            m_lines.LineKey,
                            m_lines.LineName,
                            m_lines.Note,
                            m_lines.Points,
                            m_lines.DisplayNo,
                            m_lines.DisplayFlag,
                            m_lines.LastUserID,
                            m_lines.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_lines
                        inner join m_layers on m_layers.Id=m_lines.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        Where m_lines.LineId=@LineId;
                ";

                utility.AddParameter("LineId", LineId);
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

        public static bool Add(LineModel item)
        {
            var sql = @"
                    INSERT INTO m_lines
                    (LineId,
                    OrganizationID,
                    LayerId,
                    LineKey,
                    LineName,
                    Note,
                    Points,
                    DisplayNo,
                    DisplayFlag,
                    LastUserID,
                    LastUpdatetime)
                    VALUES
                    (@LineId,
                    @OrganizationID,
                    @LayerId,
                    @LineKey,
                    @LineName,
                    @Note,
                    @Points,
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
                    utility.AddParameter("LineId", id);
                    utility.AddParameter("OrganizationID", item.OrganizationID);
                    utility.AddParameter("LayerId", item.LayerId);
                    utility.AddParameter("LineKey", item.LineKey);
                    utility.AddParameter("LineName", item.LineName);
                    utility.AddParameter("Note", item.Note);
                    utility.AddParameter("Points", item.Points);
                    utility.AddParameter("LastUserID", item.LastUserID);
                    utility.ExecuteNonQuery(sql);
                    item.LineId = id;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool Delete(LineModel item)
        {
            var sql = @" 
                    DELETE FROM m_lines WHERE LineId=@LineId;
                    ";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("LineId", item.LineId);
                return utility.ExecuteNonQuery(sql.ToString()) >= 1;
            }
        }

        public static bool Update(LineModel item)
        {
            var sql = @" 
                    UPDATE m_lines
                    SET
                        LineKey = @LineKey,
                        LineName = @LineName,
                        Note = @Note,
                        Points = @Points,
                        LastUserID = @LastUserID,
                        LastUpdatetime = now()
                    WHERE LineId = @LineId;
                    ";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("LineId", item.LineId);
                utility.AddParameter("LineKey", item.LineKey);
                utility.AddParameter("LineName", item.LineName);
                utility.AddParameter("Note", item.Note);
                utility.AddParameter("Points", item.Points);
                utility.AddParameter("LastUserID", item.LastUserID);
                return utility.ExecuteNonQuery(sql) == 1;
            }
        }

    }
}

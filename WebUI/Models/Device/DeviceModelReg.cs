using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Device
{
    class DeviceModelReg
    {
        public static List<DeviceModel> GetModels(long LayerId)
        {
            var models = new List<DeviceModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                        SELECT 
	                        m_devices.DeviceId,
                            m_devices.OrganizationID,
                            m_devices.LayerId,
                            m_devices.DeviceKey,
                            m_devices.DeviceName,
                            m_devices.DeviceUrl,
                            m_devices.Note,
                            m_devices.Lat,
                            m_devices.Lng,
                            m_devices.DisplayNo,
                            m_devices.DisplayFlag,
                            m_devices.LastUserID,
                            m_devices.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_devices
                        inner join m_layers on m_layers.Id=m_devices.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        where m_devices.LayerId=@LayerId
                        ;";

                utility.AddParameter("LayerId", LayerId);
                utility.ExecuteReaderModelList(sql, models);
            }
            return models;
        }

        public static List<DeviceModel> GetModels(string LayerIds)
        {
            var lays = LayerIds.Split(new char[] { ',' });
            var formatedLayerIds = lays.Select(m => ConvertToInt64(m)).Aggregate((c, n) => c + "," + n);
            if (string.IsNullOrEmpty(formatedLayerIds))
            {
                return null;
            }
            var models = new List<DeviceModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = string.Format(@"
                        SELECT 
	                        m_devices.DeviceId,
                            m_devices.OrganizationID,
                            m_devices.LayerId,
                            m_devices.DeviceKey,
                            m_devices.DeviceName,
                            m_devices.DeviceUrl,
                            m_devices.Note,
                            m_devices.Lat,
                            m_devices.Lng,
                            m_devices.DisplayNo,
                            m_devices.DisplayFlag,
                            m_devices.LastUserID,
                            m_devices.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_devices
                        inner join m_layers on m_layers.Id=m_devices.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        where m_devices.LayerId in ({0})
                        ;", formatedLayerIds);

                utility.ExecuteReaderModelList(sql, models);
            }
            return models;
        }

        private static string ConvertToInt64(string str) {
            if (string.IsNullOrEmpty(str))
            {
                return "0";
            }
            long l;
            if (long.TryParse(str, out l))
            {
                return l.ToString();
            }
            else {
                return "0";
            }
        }

        public static DeviceModel GetModel(long DeviceId)
        {
            var model = new DeviceModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                        SELECT 
	                        m_devices.DeviceId,
                            m_devices.OrganizationID,
                            m_devices.LayerId,
                            m_devices.DeviceKey,
                            m_devices.DeviceName,
                            m_devices.DeviceUrl,
                            m_devices.Note,
                            m_devices.Lat,
                            m_devices.Lng,
                            m_devices.DisplayNo,
                            m_devices.DisplayFlag,
                            m_devices.LastUserID,
                            m_devices.LastUpdatetime,
                            m_layers.LayerKey,
                            m_layers.LayerName,
                            m_layers.IconFile,
                            t_files.FileName as IconFileName 
                        FROM m_devices
                        inner join m_layers on m_layers.Id=m_devices.LayerId
                        left join t_files on t_files.FilesID=m_layers.IconFile
                        Where m_devices.DeviceId=@DeviceId;
                ";

                utility.AddParameter("DeviceId", DeviceId);
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


        public static bool Add(DeviceModel item)
        {
            var sql = @"
                    INSERT INTO m_devices
                    (DeviceId,
                    OrganizationID,
                    LayerId,
                    DeviceKey,
                    DeviceName,
                    DeviceUrl,
                    Note,
                    Lat,
                    Lng,
                    DisplayNo,
                    DisplayFlag,
                    LastUserID,
                    LastUpdatetime)
                    VALUES
                    (@DeviceId,
                    @OrganizationID,
                    @LayerId,
                    @DeviceKey,
                    @DeviceName,
                    @DeviceUrl,
                    @Note,
                    @Lat,
                    @Lng,
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
                    utility.AddParameter("DeviceId", id);
                    utility.AddParameter("OrganizationID", item.OrganizationID);
                    utility.AddParameter("LayerId", item.LayerId);
                    utility.AddParameter("DeviceKey", item.DeviceKey);
                    utility.AddParameter("DeviceName", item.DeviceName);
                    utility.AddParameter("DeviceUrl", item.DeviceUrl);
                    utility.AddParameter("Note", item.Note);
                    utility.AddParameter("Lat", item.Lat);
                    utility.AddParameter("Lng", item.Lng);
                    utility.AddParameter("LastUserID", item.LastUserID);
                    utility.ExecuteNonQuery(sql);
                    item.DeviceId = id;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static bool Delete(DeviceModel item)
        {
            var sql = @" 
                    DELETE FROM m_devices WHERE DeviceId=@DeviceId;
                    ";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("DeviceId", item.DeviceId);
                return utility.ExecuteNonQuery(sql.ToString()) >= 1;
            }
        }

        public static bool Update(DeviceModel item)
        {
            var sql = @" 
                    UPDATE m_devices
                    SET
                        DeviceKey = @DeviceKey,
                        DeviceName = @DeviceName,
                        DeviceUrl = @DeviceUrl,
                        Note = @Note,
                        Lat = @Lat,
                        Lng = @Lng,
                        LastUserID = @LastUserID,
                        LastUpdatetime = now()
                    WHERE DeviceId = @DeviceId;
                    ";

            using (var utility = DbUtility.GetInstance())
            {
                utility.AddParameter("DeviceId", item.DeviceId);
                utility.AddParameter("DeviceKey", item.DeviceKey);
                utility.AddParameter("DeviceName", item.DeviceName);
                utility.AddParameter("DeviceUrl", item.DeviceUrl);
                utility.AddParameter("Note", item.Note);
                utility.AddParameter("Lat", item.Lat);
                utility.AddParameter("Lng", item.Lng);
                utility.AddParameter("LastUserID", item.LastUserID);
                return utility.ExecuteNonQuery(sql) == 1;
            }
        }
    }
}

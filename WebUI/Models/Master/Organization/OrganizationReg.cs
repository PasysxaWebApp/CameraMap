using System;
using System.Collections.Generic;
using CameraMap.Models.Base;
using PacificSystem.Utility;
using SharedUtilitys.DataBases;
using SharedUtilitys.Models;
using System.Web.WebPages.Html;
using System.Text;

namespace CameraMap.Models.Master.Organization
{
    public class OrganizationReg : BaseModel
    {
        public static OrganizationReg GetInstance()
        {
            return new OrganizationReg();
        }

        public List<OrganizationModel> GetOrganizationList()
        {
            var lstResult = new List<OrganizationModel>();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = @"
                    SELECT 
                        m_organization.OrganizationID,
                        m_organization.OrganizationCode,
                        m_organization.OfficeName,
                        m_organization.OfficeNameEn,
                        m_organization.StaffName,
                        m_organization.StaffNameEn,
                        m_organization.Postal,
                        m_organization.StateCode,
                        m_organization.Address1,
                        m_organization.Address2,
                        m_organization.AddressEn,
                        m_organization.MailAddress,
                        m_organization.PhoneNumber,
                        m_organization.FaxNumber,
                        m_organization.StartDate,
                        m_organization.EndDate,
                        m_organization.BatchSaveWithDayMode,
                        m_organization.FarmPic,
                        m_organization.FarmNote,
                        m_organization.FishinggroundPic,
                        m_organization.FishinggroundNote,
                        m_organization.CenterLat,
                        m_organization.CenterLng,
                        m_organization.DefaultZoom,
                        m_organization.MinZoom,
                        m_organization.MaxZoom,
                        m_organization.FullCountyName,
                        m_organization.RectangleL,
                        m_organization.RectangleT,
                        m_organization.RectangleR,
                        m_organization.RectangleB,
                        m_organization.LastUserID,
                        m_organization.LastUpdatetime
                    FROM m_organization
                    Order by m_organization.OfficeName;
                    ";
                utility.ExecuteReaderModelList(sql, lstResult);
            }

            return lstResult;
        }


        public OrganizationModel GetOrganizationModel(string OrganizationID)
        {
            var model = new OrganizationModel();
            using (var utility = DbUtility.GetInstance())
            {
                var sql = string.Format(@"
                                    select 
                                        OrganizationID, 
                                        OrganizationCode,
                                        OfficeName, 
                                        OfficeNameEn, 
                                        StaffName, 
                                        StaffNameEn, 
                                        Postal, 
                                        StateCode, 
                                        Address1, 
                                        Address2, 
                                        AddressEn, 
                                        MailAddress, 
                                        PhoneNumber, 
                                        FaxNumber,
                                        BatchSaveWithDayMode,
                                        FarmPic,
                                        FarmNote,
                                        FishinggroundPic,
                                        FishinggroundNote, 
                                        CenterLat,
                                        CenterLng,
                                        DefaultZoom,
                                        MinZoom,
                                        MaxZoom,
                                        FullCountyName,
                                        RectangleL,
                                        RectangleT,
                                        RectangleR,
                                        RectangleB,
                                        LastUserID, 
                                        LastUpdatetime 
                                    from m_organization
                                    where OrganizationID = @OrganizationID", OrganizationID);
                utility.AddParameter("OrganizationID", OrganizationID);
                utility.ExecuteReaderModel(sql, model);
            }

            return  model;
        }

        public void UpdateOrganizationModel(OrganizationModel model)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                var sql = @"
                            UPDATE m_organization
                            SET
                            OrganizationCode = @OrganizationCode,
                            OfficeName = @OfficeName,
                            OfficeNameEn = @OfficeNameEn,
                            StaffName = @StaffName,
                            StaffNameEn = @StaffNameEn,
                            Postal = @Postal,
                            StateCode = @StateCode,
                            Address1 = @Address1,
                            Address2 = @Address2,
                            AddressEn = @AddressEn,
                            MailAddress = @MailAddress,
                            PhoneNumber = @PhoneNumber,
                            FaxNumber = @FaxNumber,                            
                            LastUserID = @LastUserID,
                            BatchSaveWithDayMode = @BatchSaveWithDayMode,
                            FarmPic = @FarmPic,
                            FarmNote = @FarmNote,
                            FishinggroundPic = @FishinggroundPic,
                            FishinggroundNote = @FishinggroundNote,
                            CenterLat = @CenterLat,
                            CenterLng = @CenterLng,
                            DefaultZoom = @DefaultZoom,
                            MinZoom = @MinZoom,
                            MaxZoom = @MaxZoom,
                            FullCountyName = @FullCountyName,
                            RectangleL = @RectangleL,
                            RectangleT = @RectangleT,
                            RectangleR = @RectangleR,
                            RectangleB = @RectangleB,
                            LastUpdatetime = now()
                            WHERE OrganizationID = @OrganizationID;
                            ";

                utility.AddParameter("OrganizationID", model.OrganizationID);
                utility.AddParameter("OrganizationCode", model.OrganizationCode);
                utility.AddParameter("OfficeName", model.OfficeName);
                utility.AddParameter("OfficeNameEn", model.OfficeNameEn);
                utility.AddParameter("StaffName", model.StaffName);
                utility.AddParameter("StaffNameEn", model.StaffNameEn);
                utility.AddParameter("Postal", model.Postal);
                utility.AddParameter("StateCode", model.StateCode);
                utility.AddParameter("Address1", model.Address1);
                utility.AddParameter("Address2", model.Address2);
                utility.AddParameter("AddressEn", model.AddressEn);
                utility.AddParameter("MailAddress", model.MailAddress);
                utility.AddParameter("PhoneNumber", model.PhoneNumber);
                utility.AddParameter("FaxNumber", model.FaxNumber);
                //utility.AddParameter("StartDate", model.StartDate);
                //utility.AddParameter("EndDate", model.EndDate);
                utility.AddParameter("BatchSaveWithDayMode", model.BatchSaveWithDayMode);
                utility.AddParameter("FarmPic", model.FarmPic);
                utility.AddParameter("FarmNote", model.FarmNote);
                utility.AddParameter("FishinggroundPic", model.FishinggroundPic);
                utility.AddParameter("FishinggroundNote", model.FishinggroundNote);

                utility.AddParameter("CenterLat", model.CenterLat);
                utility.AddParameter("CenterLng", model.CenterLng);
                utility.AddParameter("DefaultZoom", model.DefaultZoom);
                utility.AddParameter("MinZoom", model.MinZoom);
                utility.AddParameter("MaxZoom", model.MaxZoom);
                utility.AddParameter("FullCountyName", model.FullCountyName);
                utility.AddParameter("RectangleL", model.RectangleL);
                utility.AddParameter("RectangleT", model.RectangleT);
                utility.AddParameter("RectangleR", model.RectangleR);
                utility.AddParameter("RectangleB", model.RectangleB);


                utility.AddParameter("LastUserID", model.LastUserID);
                //utility.AddParameter("LastUpdatetime", model.LastUpdatetime);
                utility.ExecuteNonQuery(sql);
                
                utility.Commit();
            }
        }

        public void AddNewOrganizationModel(OrganizationModel model)
        {
            using (var utility = DbUtility.GetInstance())
            {
                utility.BeginTransaction();

                var sql = @"
                            INSERT INTO m_organization
                            (OrganizationID,
                            OrganizationCode,
                            OfficeName,
                            OfficeNameEn,
                            StaffName,
                            StaffNameEn,
                            Postal,
                            StateCode,
                            Address1,
                            Address2,
                            AddressEn,
                            MailAddress,
                            PhoneNumber,
                            FaxNumber,                            
                            BatchSaveWithDayMode,                            
                            FarmPic,
                            FarmNote,
                            FishinggroundPic,
                            FishinggroundNote, 
                            CenterLat,
                            CenterLng,
                            DefaultZoom,
                            MinZoom,
                            MaxZoom,
                            FullCountyName,
                            RectangleL,
                            RectangleT,
                            RectangleR,
                            RectangleB,
                            LastUserID,
                            LastUpdatetime)
                            VALUES
                            (uuid_short(),
                            @OrganizationCode,
                            @OfficeName,
                            @OfficeNameEn,
                            @StaffName,
                            @StaffNameEn,
                            @Postal,
                            @StateCode,
                            @Address1,
                            @Address2,
                            @AddressEn,
                            @MailAddress,
                            @PhoneNumber,
                            @FaxNumber,                            
                            @BatchSaveWithDayMode,                            
                            @FarmPic,
                            @FarmNote,
                            @FishinggroundPic,
                            @FishinggroundNote, 
                            @CenterLat,
                            @CenterLng,
                            @DefaultZoom,
                            @MinZoom,
                            @MaxZoom,
                            @FullCountyName,
                            @RectangleL,
                            @RectangleT,
                            @RectangleR,
                            @RectangleB,
                            @LastUserID,
                            now());
                        ";

                //utility.AddParameter("OrganizationID", model.OrganizationID);
                utility.AddParameter("OrganizationCode", model.OrganizationCode);
                utility.AddParameter("OfficeName", model.OfficeName);
                utility.AddParameter("OfficeNameEn", model.OfficeNameEn);
                utility.AddParameter("StaffName", model.StaffName);
                utility.AddParameter("StaffNameEn", model.StaffNameEn);
                utility.AddParameter("Postal", model.Postal);
                utility.AddParameter("StateCode", model.StateCode);
                utility.AddParameter("Address1", model.Address1);
                utility.AddParameter("Address2", model.Address2);
                utility.AddParameter("AddressEn", model.AddressEn);
                utility.AddParameter("MailAddress", model.MailAddress);
                utility.AddParameter("PhoneNumber", model.PhoneNumber);
                utility.AddParameter("FaxNumber", model.FaxNumber);
                //utility.AddParameter("StartDate", model.StartDate);
                //utility.AddParameter("EndDate", model.EndDate);
                utility.AddParameter("BatchSaveWithDayMode", model.BatchSaveWithDayMode);
                utility.AddParameter("FarmPic", model.FarmPic);
                utility.AddParameter("FarmNote", model.FarmNote);
                utility.AddParameter("FishinggroundPic", model.FishinggroundPic);
                utility.AddParameter("FishinggroundNote", model.FishinggroundNote);

                utility.AddParameter("CenterLat", model.CenterLat);
                utility.AddParameter("CenterLng", model.CenterLng);
                utility.AddParameter("DefaultZoom", model.DefaultZoom);
                utility.AddParameter("MinZoom", model.MinZoom);
                utility.AddParameter("MaxZoom", model.MaxZoom);
                utility.AddParameter("FullCountyName", model.FullCountyName);
                utility.AddParameter("RectangleL", model.RectangleL);
                utility.AddParameter("RectangleT", model.RectangleT);
                utility.AddParameter("RectangleR", model.RectangleR);
                utility.AddParameter("RectangleB", model.RectangleB);


                utility.AddParameter("LastUserID", model.LastUserID);
                //utility.AddParameter("LastUpdatetime", model.LastUpdatetime);

                utility.ExecuteNonQuery(sql);


                utility.Commit();
            }
        }


    }
}
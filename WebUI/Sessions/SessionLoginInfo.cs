﻿using System.Web;
using CameraMap.Enums;
using CameraMap.Sessions.Base;
using System.Web.SessionState;
using System.Linq;

namespace CameraMap.Sessions
{
    public class SessionLoginInfo : BaseSessionModel
    {
        public string OrganizationID { get; set; }
        public long LoginID { get; set; }
        public string UserName { get; set; }
        public SystemRollEnum SystemRoll { get; set; }
        public bool UnionOfficeUser { get; set; }
        public string UnionOfficeIds { get; set; }

        private SessionLoginInfo(HttpSessionStateBase sessionBase)
            : base( new HttpSessionStateBaseSessionModel( sessionBase))
        {
            Reload();
        }

        private SessionLoginInfo(HttpSessionState sessionBase)
            : base(new HttpSessionStateSessionModel(sessionBase))
        {
            Reload();
        }

        public static SessionLoginInfo GetInstance(HttpSessionStateBase session)
        {
            return new SessionLoginInfo(session);
        }

        public static SessionLoginInfo GetInstance(HttpSessionState session)
        {
            return new SessionLoginInfo(session);
        }
    }
}
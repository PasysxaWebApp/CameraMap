using System;

namespace CameraMap.Controllers
{
    public class SystemLogManager
    {
        public static SystemLogManager GetInstance()
        {
            return new SystemLogManager();
        }

        public void SetSystemErrorLog(string systemName, string organizationKey, long loginID, string loginUserName, string errorMessage, string stackTrace)
        {
            var systemName_ = systemName;
            var loginID_ = loginID;
            var loginUserName_ = loginUserName;
            var errorMessage_ = string.Format("{1}{0}{2}", Environment.NewLine, errorMessage, stackTrace);

            var organizationKey_ = organizationKey;
            try
            {
                //UserCtrlModeReg.GetInstance().AddErrorLogs(systemName_, loginID_, errorMessage_, organizationKey_);
            }
            catch (Exception) { }

        }

        public void SetSystemAccessLog(string systemName, string organizationKey, long loginID, string loginUserName, string operation, string ipAddress, string userAgent)
        {
        }
    }
}

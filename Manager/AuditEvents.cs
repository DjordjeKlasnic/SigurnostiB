using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public enum AuditEventTypes
    {
        AuthenticationFailure =0,
        AuthenticationSuccess = 1,
        AuthorizationFailed = 2,
        AuthorizationSuccess = 3,
        UpdateDatabaseSucces = 4,
        UpdateDatabaseFailure = 5
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccess
        {
            get
            {
                // TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
            }
        }

        public static string AuthenticationFailed
        {
            get
            {
                // TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationFailure.ToString());
            }
        }

        public static string AuthorizationSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
            }
        }

        public static string UpdateDatabaseSucces
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.UpdateDatabaseSucces.ToString());
            }
        }

        public static string UpdateDatabaseFailure
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.UpdateDatabaseFailure.ToString());
            }
        }
    }
}

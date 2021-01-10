using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class CustomPrincipal : IPrincipal
    {
        IIdentity identity = null;
        public CustomPrincipal(IIdentity windowsIdentity)
        {
            identity = windowsIdentity;
        }
        public IIdentity Identity
        {
            get { return identity; }
        }

        public bool IsInRole(string permission)
        {
            string group = Identity.Name;

            string[] groupPart = group.Split('=', ';', ' ');
            if (groupPart[3] == permission)
                return true;
            return false;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace InfinniPlatform.WebApi.Middleware
{
    public sealed class RequestPathConstructor
    {
        public PathString GetVersionPath()
        {
            return new PathString("/_version_");
        }

        public PathString GetBaseApplicationPath()
        {
            return new PathString("/_version_/_application_");
        }

        public PathString GetUserPath()
        {
            return new PathString(GetBaseApplicationPath() + "/User");
        }

        public PathString GetRolePath()
        {
            return new PathString(GetBaseApplicationPath() + "/Role");
        }

        public PathString GetSpecifiedUserPath()
        {
            return new PathString(GetUserPath() + "/_userName_");
        }


        public PathString GetSpecifiedRolePath()
        {
            return new PathString(GetRolePath() + "/_roleName_");
        }

        public PathString GetSpecifiedUserRolePath()
        {
            return new PathString(GetSpecifiedUserPath() + "/Roles/_roleName_");
        }

        public PathString GetSpecifiedUserClaimPath()
        {
            return new PathString(GetSpecifiedUserPath() + "/Claims/_claimType_");
        }
    }
}

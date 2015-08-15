using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Cache
{
    public class SecurityCache
    {
        public IEnumerable<dynamic> UserRoles { get; set; }

        public IEnumerable<dynamic> Users { get; set; }

        public IEnumerable<dynamic> Acl { get; set; }

        public IEnumerable<dynamic> Roles { get; set; }

        public IEnumerable<dynamic> Versions { get; set; } 
    }
}

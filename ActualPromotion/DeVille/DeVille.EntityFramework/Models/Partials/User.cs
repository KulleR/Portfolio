using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.EntityDataModel
{
    public partial class User : IEntity
    {
        public int ID
        {
            get { return this.Id; }
        }

        public bool InRoles(string roles)
        {
            if (string.IsNullOrWhiteSpace(roles))
            {
                return false;
            }

            var rolesArray = roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var role in rolesArray)
            {
                var hasRole = this.Role.RoleName.Equals(role);
                if (hasRole)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

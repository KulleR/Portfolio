using Deville.EntityDataModel;
using System.Linq;
using System.Security.Principal;

namespace Deville.Core.Context
{
    /// <summary>
    /// Перопределяет основные функциональные возможности аутентифицированного участника
    /// </summary>
    public class IUserProvider
    {
        public User User { get; set; }
    }
}

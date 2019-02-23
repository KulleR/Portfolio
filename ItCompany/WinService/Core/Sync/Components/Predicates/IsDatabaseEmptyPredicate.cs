using System.Linq;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Repositories;

namespace Surveys.Service.Core.Sync.Components.Predicates
{
    public class IsDatabaseEmptyPredicate : IPredicate
    {
        private static readonly ILog Logger = LogManager.GetLogger<IsDatabaseEmptyPredicate>();

        [Dependency]
        public ICompanyRepository CompanyRepository { get; set; }

        public bool Test()
        {
            bool isEmpty = true;
            CompanyRepository.Wrap(()=> { isEmpty = !CompanyRepository.Query().Any(); });
            if (isEmpty)
            {
                Logger.Debug("Database is empty");
            }
            return isEmpty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalalEcodes.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HalalEcodes.Data.Repositories
{
    public class EcodeRepository : Repository<Ecode>, IEcodeRepository
    {
        public EcodeRepository(EcodeContext databaseContext) : base(databaseContext) { }

        public Ecode GetByCode(string code)
        {
            return GetAll().Include(c => c.Category).FirstOrDefault(c => c.Code == code);
        }
    }
}

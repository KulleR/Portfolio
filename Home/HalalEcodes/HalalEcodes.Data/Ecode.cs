using HalalEcodes.Data.Enums;
using HalalEcodes.Data.Models;

namespace HalalEcodes.Data
{
    public partial class Ecode : IEntity
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
        public string Ingredients { get; set; }
        public EcodeStatus Status { get; set; }
        public string StatusDesc { get; set; }
        public long EuApprouved { get; set; }
        public long UsApprouved { get; set; }
        public string MainIngredient { get; set; }
        public long ContainsAlcohol { get; set; }
        public long IsToxic { get; set; }

        public Category Category { get; set; }
    }
}

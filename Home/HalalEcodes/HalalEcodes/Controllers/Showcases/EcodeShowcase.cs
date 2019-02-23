using HalalEcodes.Data.Enums;

namespace HalalEcodes.Controllers.Showcases
{
    public class EcodeShowcase
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string Ingredients { get; set; }
        public EcodeStatus Status { get; set; }
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }
        public bool EuApprouved { get; set; }
        public bool UsApprouved { get; set; }
        public string MainIngredient { get; set; }
        public bool ContainsAlcohol { get; set; }
        public bool IsToxic { get; set; }
    }
}

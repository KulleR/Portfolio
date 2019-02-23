using HalalEcodes.Core.Attributes;
using HalalEcodes.Rescources;

namespace HalalEcodes.Data.Enums
{
    public enum EcodeStatus
    {
        [LocalizedDescription("Halal", typeof(Localization))]
        Halal = 0,
        [LocalizedDescription("Haram", typeof(Localization))]
        Haram = 1,
        [LocalizedDescription("Mashbouh", typeof(Localization))]
        Mashbouh = 2,
        [LocalizedDescription("Unknown", typeof(Localization))]
        Unknown = 3
    }
}

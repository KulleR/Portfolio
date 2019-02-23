using System;
using System.ComponentModel;
using System.Reflection;

namespace HalalEcodes.Core.Attributes
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        public LocalizedDescriptionAttribute(string displayNameKey, Type resourceTypename) :
            base(resourceTypename.GetProperty(displayNameKey)?.GetValue(resourceTypename.GetProperty(displayNameKey).DeclaringType).ToString())
        { }
    }
}

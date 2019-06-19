using System;
using System.ComponentModel;

namespace Fate.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var attributes = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length == 0 ?
                null : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}

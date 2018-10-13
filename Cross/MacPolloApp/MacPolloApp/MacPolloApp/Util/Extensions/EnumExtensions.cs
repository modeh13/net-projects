using MacPolloApp.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MacPolloApp.Util.Extensions
{
    public static class EnumExtensions
    {
        private static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetRuntimeField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }

        public static IEnumerable<EnumValueDataAttribute> ConvertEnumToList<TEnum>() where TEnum : struct // can't constrain to enums so closest thing
        {
            return Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                       .Select(val => val.GetAttribute<EnumValueDataAttribute>())
                       .ToList();
        }

        public static EnumValueDataAttribute GetEnumAttribute(Enum value)
        {
            return value.GetAttribute<EnumValueDataAttribute>();
        }

        public static EnumValueDataAttribute GetEnumByType<TEnum>(string value) where TEnum : struct // can't constrain to enums so closest thing
        {
            return Enum.GetValues(typeof(TEnum)).Cast<Enum>()
                       .Select(val => val.GetAttribute<EnumValueDataAttribute>())
                       .ToList().FirstOrDefault(v => v.KeyValue.Equals(value));
        }
    }
}
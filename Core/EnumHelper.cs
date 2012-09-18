using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Core
{
    public static class EnumHelper
    {
        private static readonly StringDictionary _EnumDescriptions = new StringDictionary();

        public static string GetEnumValueDescription<T>(T enumValue) where T : struct
        {
            Type enumType = typeof (T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Value must be an enumeration.", "enumValue");
            }

            string key = enumType.AssemblyQualifiedName + ":" + enumValue;

            if (!_EnumDescriptions.ContainsKey(key))
            {
                var info = enumType.GetField(enumValue.ToString());

                if (info != null)
                {
                    var attributes =
                        (DescriptionAttribute[]) info.GetCustomAttributes(typeof (DescriptionAttribute), false);

                    if (attributes.Length > 0)
                    {
                        _EnumDescriptions.Add(key, attributes[0].Description);
                        return _EnumDescriptions[key];
                    }
                }
                else
                {
                    _EnumDescriptions.Add(key, null);
                }
            }

            return _EnumDescriptions[key];
        }

        public static T ParseEnumDescription<T>(string description) where T : struct
        {
            var enumType = typeof (T);

            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("This method is only valid for Enumerations");
            }

            T value = default(T);
            var fields = enumType.GetFields();

            foreach (var field in fields)
            {
                var attributes =
                    (DescriptionAttribute[]) field.GetCustomAttributes(typeof (DescriptionAttribute), false);

                if (attributes.Length <= 0) 
                    continue;

                if (string.Compare(attributes[0].Description, description, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                value = (T) field.GetRawConstantValue();
                break;
            }

            return value;
        }

        public static string GetDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof (DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
        
        public static T ParseEnumValueDescription<T>(string description) where T : struct
        {
            var enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("This method is only valid for Enumerations");
            }

            T value = default(T);
            var fields = enumType.GetFields();

            foreach (var field in fields)
            {
                if (string.Compare(field.Name, description, StringComparison.OrdinalIgnoreCase) != 0)
                    continue;

                value = (T)field.GetRawConstantValue();
                break;
            }

            return value;
        }
    }
}
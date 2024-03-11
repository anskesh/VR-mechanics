using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace QualitySettings.Utility
{
    public static class EnumUtility
    {
        public static string NormalizeEnumName(Type type, string name)
        {
            FieldInfo fieldInfo = type.GetField(name);
            InspectorNameAttribute inspectorNameAttribute = (InspectorNameAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(InspectorNameAttribute));
            return inspectorNameAttribute != null ? inspectorNameAttribute.displayName : ConvertEnumValueName(name);
        }

        private static string ConvertEnumValueName(string value)
        {
            StringBuilder convertedString = new StringBuilder();

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '_') continue;

                if (char.IsUpper(value[i]))
                    convertedString.Append(' ');
                
                convertedString.Append(value[i]);
            }

            return convertedString.ToString().TrimStart();
        }
    }
}
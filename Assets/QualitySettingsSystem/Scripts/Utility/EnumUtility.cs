using System;
using System.Reflection;
using UnityEngine;

namespace QualitySettings.Utility
{
    public static class EnumUtility
    {
        public static string ConvertEnumValueToString(Type type, string value)
        {
            FieldInfo fieldInfo = type.GetField(value);
            InspectorNameAttribute inspectorNameAttribute =
                (InspectorNameAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(InspectorNameAttribute));

            return inspectorNameAttribute != null ? inspectorNameAttribute.displayName : ConvertEnumValueName(value);
        }

        private static string ConvertEnumValueName(string value)
        {
            string convertedString = "";
            
            foreach (var symbol in value)
            {
                if (symbol == '_') continue;

                if (char.IsUpper(symbol))
                    convertedString += " ";
                
                convertedString += symbol;
            }

            convertedString = convertedString.TrimStart();
            return convertedString;
        }
    }
}
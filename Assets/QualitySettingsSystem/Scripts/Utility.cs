﻿using System;
using System.Reflection;
using UnityEngine;

namespace QualitySettings
{
    public static class Utility
    {
        public static string ConvertValueToString<T>(string value) where T:Enum
        {
            FieldInfo fieldInfo = typeof(T).GetField(value);
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
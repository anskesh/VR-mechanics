using System;
using System.Reflection;
using UnityEngine.Rendering.Universal;

namespace QualitySettings.Utility
{
    public static class URPUtility
    {
        public static void SetFieldValue(this UniversalRenderPipelineAsset asset, string fieldName, object value)
        {
            Type type = asset.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName,  BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null)
                throw new Exception($"Field {fieldName} not found");
            
            fieldInfo.SetValue(asset, value);
        }
        
        public static void SetPropertyValue(this UniversalRenderPipelineAsset asset, string propertyName, object value)
        {
            Type type = asset.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propertyName,  BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new Exception($"Property {propertyName} not found");
            
            propertyInfo.SetValue(asset, value);
        }
    }
}
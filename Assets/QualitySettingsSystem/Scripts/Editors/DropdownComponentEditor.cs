using System;
using System.Collections.Generic;
using System.Reflection;
using QualitySettings.UIComponents;
using UnityEditor;
using UnityEngine.Rendering.Universal;

namespace QualitySettings.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(DropdownComponent))]
    public class DropdownComponentEditor : Editor
    {
        private Type[] _enumTypes;
        private string[] _enumTypesString;

        private int _selectedIndex;
        private DropdownComponent _dropdownComponent;

        private void OnEnable()
        {
            _dropdownComponent = (DropdownComponent) target;
            FindEnumTypes();

            _selectedIndex = GetIndex(_dropdownComponent.EnumType);
            UpdateType(_enumTypes[_selectedIndex]);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            _selectedIndex = GetIndex(_dropdownComponent.EnumType);
            _selectedIndex = EditorGUILayout.Popup("Enum Type", _selectedIndex, _enumTypesString);

            Type selectedType = _enumTypes[_selectedIndex];

            if (_dropdownComponent.EnumType != selectedType) 
                UpdateType(selectedType);
            
            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateType(Type type)
        {
            _dropdownComponent.UpdateType(type);
            EditorUtility.SetDirty(_dropdownComponent);

            if (_dropdownComponent.TryGetComponent(out LockingDropdownComponent lockingDropdownComponent))
                lockingDropdownComponent.OnValidate();
        }

        private void FindEnumTypes()
        {
            List<Type> types = new();
            Type urpScriptType = typeof(UniversalRenderPipelineAsset);
            FieldInfo[] fields = urpScriptType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
                if (field.FieldType.IsEnum) types.Add(field.FieldType);

            _enumTypes = types.ToArray();
            _enumTypesString = GetEnumTypeNames();
        }

        private string[] GetEnumTypeNames()
        {
            List<string> typeNames = new();

            foreach (Type type in _enumTypes)
                typeNames.Add(type.Name);

            return typeNames.ToArray();
        }

        private int GetIndex(Type type)
        {
            for (int i = 0; i < _enumTypes.Length; i++)
                if (_enumTypes[i] == type) return i;

            return 0;
        }
    }
#endif
}
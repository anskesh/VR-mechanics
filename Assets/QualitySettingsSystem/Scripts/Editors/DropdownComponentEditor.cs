using System;
using System.Collections.Generic;
using System.Reflection;
using QualitySettings.UIComponents;
using QualitySettings.Utility;
using UnityEditor;
using UnityEngine;
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

        private bool[] _excludedValues;
        private int[] _enumValues;
        private string[] _enumNames;
        private bool _needExcludedUpdate;
        
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
            else
                _dropdownComponent.ExcludedValues.CopyTo(_excludedValues, 0);
            
            DrawExcludedToggles(_excludedValues);
            
            if (_needExcludedUpdate)
                UpdateExcluded();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawExcludedToggles(bool[] values)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label("Excluded values", EditorStyles.boldLabel);
            
            for (int i = 0; i < _enumValues.Length; i++)
            {
                bool isExcluded = values[i];
                bool newExcluded = GUILayout.Toggle(isExcluded, _enumNames[i]);

                if (newExcluded == isExcluded) 
                    continue;
        
                values[i] = newExcluded;
                _needExcludedUpdate = true;
            }

            GUILayout.EndVertical();
        }
        
        private void UpdateType(Type type)
        {
            _dropdownComponent.UpdateType(type);
            EditorUtility.SetDirty(_dropdownComponent);

            UpdateEnumValues();
            UpdateLockingComponent();
        }

        private void UpdateExcluded()
        {
            _needExcludedUpdate = false;
            _dropdownComponent.UpdateExcluded(_excludedValues);
            EditorUtility.SetDirty(_dropdownComponent);
            UpdateLockingComponent();
        }

        private void UpdateLockingComponent()
        {
            if (_dropdownComponent.TryGetComponent(out LockingDropdownComponent lockingDropdownComponent))
                lockingDropdownComponent.OnValidate();
        }

        private void UpdateEnumValues()
        {
            Type selectedType = _enumTypes[_selectedIndex];
            _enumValues = (int[]) Enum.GetValues(selectedType);
            string[] names = Enum.GetNames(selectedType);
            _enumNames = new string[names.Length];

            for (int i = 0; i < names.Length; i++)
                _enumNames[i] = EnumUtility.ConvertEnumValueToString(selectedType, names[i]);
            
            _excludedValues = new bool[_enumValues.Length];
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
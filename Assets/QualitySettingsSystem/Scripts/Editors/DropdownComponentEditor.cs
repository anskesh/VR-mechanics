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
    [CustomEditor(typeof(EnumDropdownComponent))]
    public class DropdownComponentEditor : Editor
    {
        private List<Type> _enumTypes;
        private string[] _enumTypesString;

        private int _selectedIndex;
        private EnumDropdownComponent _enumDropdownComponent;

        private int[] _enumValues;
        private string[] _enumNames;
        private bool _needUpdateExcludedValues;
        
        private void OnEnable()
        {
            _enumDropdownComponent = (EnumDropdownComponent) target;
            SetEnumTypes();

            _selectedIndex = GetEnumTypeIndex(_enumDropdownComponent.EnumType);
            ChangeEnumType(_enumTypes[_selectedIndex]);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            _selectedIndex = GetEnumTypeIndex(_enumDropdownComponent.EnumType);
            _selectedIndex = EditorGUILayout.Popup("Enum Type", _selectedIndex, _enumTypesString);
            Type selectedType = _enumTypes[_selectedIndex];
            
            if (_enumDropdownComponent.EnumType != selectedType)
                ChangeEnumType(selectedType);
            
            DrawExcludedToggles(_enumDropdownComponent.ExcludedEnumValues);
            
            if (_needUpdateExcludedValues)
                ChangeDropdownValues();
            
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
                
                if (newExcluded == isExcluded) continue;
        
                values[i] = newExcluded;
                _needUpdateExcludedValues = true;
            }

            GUILayout.EndVertical();
        }
        
        private void ChangeEnumType(Type selectedType)
        {
            _enumDropdownComponent.ChangeEnumType(selectedType);
            ChangeExcludedValues(selectedType);
            ChangeDropdownValues();
        }

        private void ChangeDropdownValues()
        {
            _needUpdateExcludedValues = false;
            _enumDropdownComponent.ChangeDropdownValues(_enumValues, _enumNames);
            EditorUtility.SetDirty(_enumDropdownComponent);
            
            if (_enumDropdownComponent.TryGetComponent(out LockingDropdownComponent lockingDropdownComponent))
                lockingDropdownComponent.OnValidate();
        }

        private void ChangeExcludedValues(Type selectedType)
        {
            _enumValues = (int[]) Enum.GetValues(selectedType);
            _enumNames = Enum.GetNames(selectedType);

            for (int i = 0; i < _enumNames.Length; i++)
                _enumNames[i] = EnumUtility.NormalizeEnumName(selectedType, _enumNames[i]);
        }
        
        private void SetEnumTypes()
        {
            _enumTypes = new List<Type>();
            Type urpScriptType = typeof(UniversalRenderPipelineAsset);
            FieldInfo[] fields = urpScriptType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                    _enumTypes.Add(field.FieldType);
            }

            _enumTypesString = new string[_enumTypes.Count];

            for (int i = 0; i < _enumTypes.Count; i++)
                _enumTypesString[i] = _enumTypes[i].Name;
        }

        private int GetEnumTypeIndex(Type type)
        {
            for (int i = 0; i < _enumTypes.Count; i++)
            {
                if (_enumTypes[i] == type)
                    return i;
            }

            return 0;
        }
    }
#endif
}
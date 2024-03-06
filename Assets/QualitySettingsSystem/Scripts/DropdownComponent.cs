using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace QualitySettings
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownComponent : UIComponent
    {
        public Action<int> ValueChangedEvent;

        public Type EnumType { get; private set; }
        public TMP_Dropdown Dropdown => _dropdown;

        [SerializeField, HideInInspector] private bool _valuesEqualsIndexes = true;
        [SerializeField, HideInInspector] private string _typeName;

        [SerializeField, HideInInspector] private TMP_Dropdown _dropdown;
        [SerializeField, HideInInspector] private OptionData[] _optionData;

        protected override void OnValidate()
        {
            base.OnValidate();

            _dropdown = Selectable as TMP_Dropdown;
        }

        protected override void Awake()
        {
            base.Awake();

            _dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_dropdown)
                _dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }

        public void UpdateType()
        {
            UpdateType(_typeName, Type.GetType(_typeName));
        }

        public void UpdateType(string typeName, Type type)
        {
            _typeName = typeName;
            EnumType = type;
            InitializeValues();
        }

        public Enum GetValue()
        {
            int enumValue = _optionData[Dropdown.value].Value;
            return (Enum) Enum.ToObject(EnumType, enumValue);
        }

        public void ChangeValue(Enum enumerable)
        {
            int enumValue = Convert.ToInt16(enumerable);
            Dropdown.value = _optionData[GetEnumIndex(enumValue)].Value;
        }

        private void InitializeValues()
        {
            Array values = Enum.GetValues(EnumType);
            Array names = Enum.GetNames(EnumType);

            Dropdown.options.Clear();
            _optionData = new OptionData[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                int enumValue = (int) values.GetValue(i);
                string name = Utility.ConvertEnumValueToString(EnumType, (string) names.GetValue(i));
                OptionData data = new OptionData(name, enumValue);

                if (data.Value != i)
                    _valuesEqualsIndexes = false;

                Dropdown.options.Add(new TMP_Dropdown.OptionData(data.Text));
                _optionData[i] = data;
            }
        }

        private int GetEnumIndex(int enumValue)
        {
            if (_valuesEqualsIndexes)
                return enumValue;

            for (int i = 0; i < _optionData.Length; i++)
                if (_optionData[i].Value == enumValue)
                    return i;

            return 0;
        }

        private void OnValueChanged(int value)
        {
            ValueChangedEvent?.Invoke(value);
        }

        [Serializable]
        private class OptionData
        {
            public string Text => _text;
            public int Value => _value;

            [SerializeField] private string _text;
            [SerializeField] private int _value;

            public OptionData(string text, int value)
            {
                _text = text;
                _value = value;
            }
        }
    }
    
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
            _dropdownComponent.UpdateType();
            FindEnumTypes();

            _selectedIndex = GetIndex(_dropdownComponent.EnumType);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            _selectedIndex = EditorGUILayout.Popup("Enum Type", _selectedIndex, _enumTypesString);

            Type selectedType = _enumTypes[_selectedIndex];

            if (selectedType != _dropdownComponent.EnumType)
            {
                _dropdownComponent.UpdateType(selectedType.AssemblyQualifiedName, selectedType);
                EditorUtility.SetDirty(_dropdownComponent);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void FindEnumTypes()
        {
            List<Type> types = new();
            Type urpScriptType = typeof(UniversalRenderPipelineAsset);
            FieldInfo[] fields =
                urpScriptType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                    types.Add(field.FieldType);
            }

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
                if (_enumTypes[i] == type)
                    return i;

            return 0;
        }
    }
#endif
}
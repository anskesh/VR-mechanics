using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class EnumDropdownComponent : UIComponent
    {
        public Action<int> ValueChangedEvent;

        public IReadOnlyList<TMP_Dropdown.OptionData> Options { get { return _dropdown.options; } }
        public Type EnumType { get { return _enumType ??= Type.GetType(_enumTypeName); }}
        public bool[] ExcludedEnumValues { get { return _excludedEnumValues; } }
        
        [SerializeField, HideInInspector] private string _enumTypeName = "UnityEngine.Rendering.Universal.RendererType, Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        [SerializeField, HideInInspector] private TMP_Dropdown _dropdown;
        
        [SerializeField, HideInInspector] private List<int> _enumValues;
        [SerializeField, HideInInspector] private bool[] _excludedEnumValues;

        private Dictionary<int, int> _enumValuesIndexes;
        private Type _enumType;

        protected override void OnValidate()
        {
            base.OnValidate();
            _dropdown = Selectable as TMP_Dropdown;
        }

        protected void Awake()
        {
            _dropdown.onValueChanged.AddListener(OnValueChanged);
            _enumValuesIndexes = new Dictionary<int, int>(_enumValues.Count);
            
            for (int i = 0; i < _enumValues.Count; i++)
                _enumValuesIndexes[_enumValues[i]] = i;
        }

        protected void OnDestroy()
        {
            if (_dropdown) 
                _dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }

        public void ChangeValue(Enum enumerable)
        {
            int enumValue = Convert.ToInt32(enumerable);
            _dropdown.value = _enumValues[_enumValuesIndexes[enumValue]];
        }
        
        public T GetEnumValue<T>()
        {
            int enumValue = _enumValues[_dropdown.value];
            return (T) Enum.ToObject(EnumType, enumValue);
        }
        
        public int GetIntValue()
        {
            return _enumValues[_dropdown.value];
        }
    
        public void ChangeEnumType(Type type)
        {
            _enumTypeName = type.AssemblyQualifiedName;
            _enumType = type;
        }
        
        public void ChangeDropdownValues(int[] enumValues, string[] enumNames)
        {
            _enumValues.Clear();
            
            if (_excludedEnumValues.Length != enumValues.Length)
                _excludedEnumValues = new bool[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                if (!_excludedEnumValues[i])
                    _enumValues.Add(enumValues[i]);
            }
            
            ChangeDropdownOptions(enumNames, _enumValues.Count);
        }

        private void ChangeDropdownOptions(string[] names, int capacity)
        {
            _dropdown.options.Clear();

            for (int i = 0; i < capacity; i++)
                _dropdown.options.Add(new TMP_Dropdown.OptionData(names[i]));
        }

        private void OnValueChanged(int value)
        {
            ValueChangedEvent?.Invoke(value);
        }
    }
}
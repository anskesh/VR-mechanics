using System;
using System.Collections;
using System.Collections.Generic;
using QualitySettings.Utility;
using TMPro;
using UnityEngine;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownComponent : UIComponent
    {
        public Action<int> ValueChangedEvent;

        public IReadOnlyList<TMP_Dropdown.OptionData> Options => _dropdown.options;
        public Type EnumType => _enumType ??= Type.GetType(_typeEnumName);
        public bool[] ExcludedValues => _excludedValues;
        
        [SerializeField, HideInInspector] private string _typeEnumName = "UnityEngine.Rendering.Universal.RendererType, Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        [SerializeField, HideInInspector] private TMP_Dropdown _dropdown;
        
        [SerializeField, HideInInspector] private List<int> _values;
        [SerializeField, HideInInspector] private bool[] _excludedValues;

        private Dictionary<int, int> _valuesIndexes;
        private Type _enumType;

        protected override void OnValidate()
        {
            base.OnValidate();
            
            _dropdown = Selectable as TMP_Dropdown;
        }

        protected override void Awake()
        {
            base.Awake();

            _dropdown.onValueChanged.AddListener(OnValueChanged);
            _valuesIndexes = new Dictionary<int, int>(_values.Count);
            
            for (int i = 0; i < _values.Count; i++)
                _valuesIndexes[_values[i]] = i;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_dropdown) _dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }
    
        public void UpdateType(Type type)
        {
            _typeEnumName = type.AssemblyQualifiedName;
            _enumType = type;
            UpdateValues();
        }

        public void UpdateExcluded(bool[] excludedValues)
        {
            excludedValues.CopyTo(_excludedValues, 0);
            UpdateValues();
        }

        public Enum GetEnum()
        {
            int enumValue = _values[_dropdown.value];
            return (Enum) Enum.ToObject(EnumType, enumValue);
        }

        public int GetValue()
        {
            return _values[_dropdown.value];
        }

        public void ChangeValue(Enum enumerable)
        {
            int enumValue = Convert.ToInt16(enumerable);
            _dropdown.value = _values[_valuesIndexes[enumValue]];
        }
        
        private void UpdateValues()
        {
            Array values = Enum.GetValues(EnumType);
            Array names = Enum.GetNames(EnumType);
            
            int capacity = values.Length;
            _values ??= new List<int>(capacity);
            
            if (_excludedValues.Length != capacity)
                _excludedValues = new bool[capacity];

            int capacityWithExcluded = GetWithExcludedCapacity();
            List<string> convertedNames = new List<string>(capacityWithExcluded);

            int index = 0;
            int enumValueIndex = 0;
            while (index < capacity && enumValueIndex < capacity)
            {
                if (_excludedValues[enumValueIndex])
                {
                    enumValueIndex++;
                    continue;
                }
                
                int enumValue = (int) values.GetValue(enumValueIndex);
                string enumName = EnumUtility.ConvertEnumValueToString(EnumType, (string) names.GetValue(enumValueIndex));
                convertedNames.Add(enumName);
                
                if (_values.Count <= index) _values.Add(enumValue);
                else _values[index] = enumValue;

                enumValueIndex++;
                index++;
            }
            
            RemoveExtraValues(_values, capacityWithExcluded);

            for (int i = 0; i < _values.Count; i++)
            {
                if (_dropdown.options.Count <= i)
                    _dropdown.options.Add(new TMP_Dropdown.OptionData(convertedNames[i]));
                else
                    _dropdown.options[i].text = convertedNames[i];
            }
            
            RemoveExtraValues(_dropdown.options, capacityWithExcluded);
        }
        
        private void RemoveExtraValues(IList list, int capacity)
        {
            if (list.Count <= capacity) return;
            
            for (int i = capacity - 1; i < list.Count; i++)
                list.RemoveAt(list.Count - 1);
        }

        private int GetWithExcludedCapacity()
        {
            int count = 0;
            
            for (int i = 0; i < _excludedValues.Length; i++)
                if (!_excludedValues[i]) count++;
            
            return count;
        }

        private void OnValueChanged(int value)
        {
            ValueChangedEvent?.Invoke(value);
        }
    }
}
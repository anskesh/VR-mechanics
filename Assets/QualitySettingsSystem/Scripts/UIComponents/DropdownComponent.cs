using System;
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
        
        [SerializeField, HideInInspector] private string _typeEnumName = "UnityEngine.Rendering.Universal.RendererType, Unity.RenderPipelines.Universal.Runtime, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        [SerializeField, HideInInspector] private TMP_Dropdown _dropdown;
        [SerializeField, HideInInspector] private List<int> _values;

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
            
            for (int i = 0; i < capacity; i++)
            {
                int enumValue = (int) values.GetValue(i);
                string enumName = EnumUtility.ConvertEnumValueToString(EnumType, (string) names.GetValue(i));
                
                if (_values.Count <= i)
                {
                    _values.Add(enumValue);
                    _dropdown.options.Add(new TMP_Dropdown.OptionData(enumName));
                }
                else
                {
                    _values[i] = enumValue;
                    _dropdown.options[i].text = enumName;
                }
            }
            
            RemoveExtraValues(capacity);
        }

        private void RemoveExtraValues(int capacity)
        {
            if (_values.Count <= capacity && _dropdown.options.Count <= capacity) return;
            
            for (int i = capacity - 1; i < _values.Count; i++)
                _values.RemoveAt(_values.Count - 1);
            
            for (int i = capacity - 1; i < _dropdown.options.Count; i++)
                _dropdown.options.RemoveAt(_dropdown.options.Count - 1);
        }

        private void OnValueChanged(int value)
        {
            ValueChangedEvent?.Invoke(value);
        }
    }
}
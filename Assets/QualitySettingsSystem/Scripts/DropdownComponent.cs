using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace QualitySettings
{
    public class DropdownComponent : TMP_Dropdown
    {
        private List<OptionData> _optionData;
        private Type _type;
        
        public void InitializeValues<T>() where T:Enum
        {
            _type = typeof(T);
            Array values = Enum.GetValues(typeof(T));
            Array names = Enum.GetNames(typeof(T));

            _optionData = new (values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                string name = Utility.ConvertValueToString<T>((string) names.GetValue(i));
                OptionData data = new OptionData(name, (int) values.GetValue(i));
                _optionData.Add(data);
            }
            
            _optionData.Sort();

            foreach (OptionData optionData in _optionData)
                options.Add(new TMP_Dropdown.OptionData(optionData.Text));
        }

        public void ChangeValue(Enum enumerable)
        {
            for (int i = 0; i < _optionData.Count; i++)
            {
                if (_optionData[i].Value != Convert.ToInt16(enumerable))
                    continue;

                value = _optionData[i].Value;
                return;
            }
        }
        
        public Enum GetValue()
        {
            int enumValue = _optionData[value].Value;
            return (Enum) Enum.ToObject(_type, enumValue);
        }

        public new class OptionData : IComparable<OptionData>
        {
            public string Text { get; private set; }
            public int Value { get; private set; }
            
            public OptionData(string text, int value)
            {
                Text = text;
                Value = value;
            }

            public int CompareTo(OptionData other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                
                return Value.CompareTo(other.Value);
            }
        }
    }
}
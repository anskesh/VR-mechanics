using System;
using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleComponent : UIComponent
    {
        public Action<bool> ValueChangedEvent;
        
        public bool IsOn
        {
            get => Toggle.isOn;
            set => Toggle.isOn = value;
        }
        
        public Toggle Toggle => _toggle;
        
        [SerializeField, HideInInspector] private Toggle _toggle;

        protected override void OnValidate()
        {
            base.OnValidate();
            
            _toggle = Selectable as Toggle;
        }

        protected override void Awake()
        {
            base.Awake();
            
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (_toggle)
                _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            ValueChangedEvent?.Invoke(value);
        }
    }
}
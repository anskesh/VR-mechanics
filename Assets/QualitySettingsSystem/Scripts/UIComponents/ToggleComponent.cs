using System;
using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleComponent : UIComponent
    {
        public Action<bool> ValueChangedEvent;
        
        public bool IsOn
        {
            get { return _toggle.isOn; }
            set { _toggle.isOn = value; }
        }
        
        [SerializeField, HideInInspector] private Toggle _toggle;

        protected override void OnValidate()
        {
            base.OnValidate();
            _toggle = Selectable as Toggle;
        }

        protected void Awake()
        {
            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected void OnDestroy()
        {
            if (_toggle) 
                _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            ValueChangedEvent?.Invoke(value);
        }
    }
}
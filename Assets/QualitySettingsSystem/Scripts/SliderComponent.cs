using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings
{
    [RequireComponent(typeof(Slider))]
    public class SliderComponent : UIComponent
    {
        public Action<float> ValueChangedEvent;
        
        public Slider Slider => _slider;
        public int ValuesCount => (int) (_slider.maxValue - _slider.minValue);
        public int MinValue => (int) _slider.minValue;
        
        public float Value
        {
            get => Slider.value;
            set => Slider.value = value;
        }

        
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _textValue;

        protected override void OnValidate()
        {
            base.OnValidate();
            
            _slider = Selectable as Slider;
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            OnSliderValueChanged(_slider.value);
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (Slider)
                Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
        
        private void OnSliderValueChanged(float value)
        {
            if (!Slider.wholeNumbers)
                value = (float) Math.Round(value, 1);

            Slider.value = value;
            
            _textValue.text = value.ToString();
            ValueChangedEvent?.Invoke(value);
        }
    }
}
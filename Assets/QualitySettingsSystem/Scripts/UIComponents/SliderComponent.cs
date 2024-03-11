using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(Slider))]
    public class SliderComponent : UIComponent
    {
        public Action<float> ValueChangedEvent;
        
        public Slider Slider => _slider;
        public int ValuesCount => (int) (_slider.maxValue - _slider.minValue) + 1;
        public int MinValue => (int) _slider.minValue;
        
        public float Value
        {
            get => Slider.value;
            set => Slider.value = value;
        }
        
        [SerializeField] private TextMeshProUGUI _textValue;
        [SerializeField, HideInInspector] private Slider _slider;

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
            
            if (Slider) Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }

        public void ChangeMinMax(float min, float max)
        {
            _slider.minValue = min;
            _slider.maxValue = max;

            _slider.value = Math.Clamp(_slider.value, min, max);
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
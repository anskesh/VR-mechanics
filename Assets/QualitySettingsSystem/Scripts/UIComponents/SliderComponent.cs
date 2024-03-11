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

        public float Value
        {
            get { return _slider.value;}
            set { _slider.value = value; }
        }
        public Slider Slider {get { return _slider; }}
        public int ValuesCount {get { return (int) (_slider.maxValue - _slider.minValue) + 1; }}
        public int MinValue {get {return (int) _slider.minValue;}}
        
        [SerializeField] private TextMeshProUGUI _textValue;
        [SerializeField, HideInInspector] private Slider _slider;

        protected override void OnValidate()
        {
            base.OnValidate();
            _slider = Selectable as Slider;
        }
        
        protected void Awake()
        {
            OnSliderValueChanged(_slider.value);
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected void OnDestroy()
        {
            if (Slider) 
                Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
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
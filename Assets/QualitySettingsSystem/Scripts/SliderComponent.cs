using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings
{
    public class SliderComponent : Slider
    {
        [SerializeField] private TextMeshProUGUI _textValue;

        protected override void Awake()
        {
            base.Awake();
            onValueChanged.AddListener(OnSliderValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onValueChanged.RemoveListener(OnSliderValueChanged);
        }
        
        private void OnSliderValueChanged(float value)
        {
            _textValue.text = value.ToString();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SliderComponent))]
    public class SliderComponentEditor : Editor {}
#endif
}
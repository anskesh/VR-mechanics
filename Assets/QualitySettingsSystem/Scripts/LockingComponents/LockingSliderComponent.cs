using UnityEngine;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(SliderComponent))]
    public class LockingSliderComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private SliderComponent _sliderComponent;

        protected override void Awake()
        {
            base.Awake();
            _sliderComponent.ValueChangedEvent += OnValueChanged;
        }

        protected void OnDestroy()
        {
            _sliderComponent.ValueChangedEvent -= OnValueChanged;
        }
        
        protected override void OnComponentFounded()
        {
            _sliderComponent = Component as SliderComponent;
        }

        protected override void SetCapacity()
        {
            Capacity = _sliderComponent.ValuesCount;
        }

        protected override string GetBlockingInfoName(int index)
        {
            return (index + _sliderComponent.MinValue).ToString();
        }

        private void OnValueChanged(float value)
        {
            int index = (int) value;
            
            if (CurrentIndex == index)
                return;
            
            Unlock(CurrentIndex);
            Lock(index);
            
            CurrentIndex = index;
        }
    }
}
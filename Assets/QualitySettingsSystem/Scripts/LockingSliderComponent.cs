using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(SliderComponent))]
    public class LockingSliderComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private SliderComponent _sliderComponent;

        protected override void Awake()
        {
            base.Awake();

            _sliderComponent.ValueChangedEvent += OnValueChanged;
            ActivateLock(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _sliderComponent.ValueChangedEvent -= OnValueChanged;
        }
        
        protected override void UpdateValues()
        {
            if (!_sliderComponent.Slider.wholeNumbers) return;
            
            int capacity = _sliderComponent.ValuesCount;
            BlockingInfos ??= new List<BlockingInfo>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                string valueName = (i + _sliderComponent.MinValue).ToString();
                
                if (BlockingInfos.Count <= i)
                    BlockingInfos.Add(new BlockingInfo(valueName));
                else
                    BlockingInfos[i].SetName(valueName);
            }
            
            RemoveExtraValues(capacity);
        }
        
        protected override void OnComponentFounded()
        {
            _sliderComponent = Component as SliderComponent;
        }

        private void OnValueChanged(float value)
        {
            int index = (int) value;
            
            if (CurrentIndex == index)
                return;
            
            DeactivateLock(CurrentIndex);
            ActivateLock(index);
            
            CurrentIndex = index;
        }
    }
}
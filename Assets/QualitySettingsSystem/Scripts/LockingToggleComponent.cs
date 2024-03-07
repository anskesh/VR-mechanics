using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(ToggleComponent))]
    public class LockingToggleComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private ToggleComponent _toggleComponent;

        protected override void Awake()
        {
            base.Awake();

            _toggleComponent.ValueChangedEvent += OnValueChanged;
            ActivateLock(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _toggleComponent.ValueChangedEvent -= OnValueChanged;
        }

        protected override void UpdateValues()
        {
            if (BlockingInfos?.Count >= 2)
                return;
            
            BlockingInfos = new List<BlockingInfo>(2);

            BlockingInfo blockingInfo = new ("is On");
            BlockingInfo blockingInfo2 = new ("is Off");
            
            BlockingInfos.Add(blockingInfo);
            BlockingInfos.Add(blockingInfo2);
        }
        
        protected override void OnComponentFounded()
        {
           _toggleComponent = Component as ToggleComponent;
        }

        private void OnValueChanged(bool value)
        {
            int index = value ? 0 : 1;
            
            if (CurrentIndex == index)
                return;
            
            DeactivateLock(CurrentIndex);
            ActivateLock(index);
            
            CurrentIndex = index;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(ToggleComponent))]
    public class BlockingToggleComponent : BlockingComponent<bool>
    {
        [SerializeField, HideInInspector] private ToggleComponent _toggleComponent;

        protected override void Awake()
        {
            base.Awake();

            LastValue = true;
            _toggleComponent.ValueChangedEvent += OnValueChanged;
            ChangeCurrentComponentsInteractable(0, false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _toggleComponent.ValueChangedEvent -= OnValueChanged;
        }

        protected override void UpdateValues()
        {
            if (BlockingInfos.Count >= 2)
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

        protected override void OnValueChanged(bool value)
        {
            if (LastValue == value)
                return;
            
            int index = value ? 0 : 1;
            ChangeCurrentComponentsInteractable((index + 1) % 2, true);
            ChangeCurrentComponentsInteractable(index, false);
            
            LastValue = value;
        }
    }
}
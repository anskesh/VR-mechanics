using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(DropdownComponent))]
    public class LockingDropdownComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private DropdownComponent _dropdownComponent;

        protected override void Awake()
        {
            base.Awake();
            _dropdownComponent.ValueChangedEvent += OnValueChanged;
            ActivateLock(CurrentIndex);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dropdownComponent.ValueChangedEvent -= OnValueChanged;
        }
        
        protected override void UpdateValues()
        {
            int capacity = _dropdownComponent.Options.Count;
            BlockingInfos ??= new List<BlockingInfo>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                string valueName = _dropdownComponent.Options[i].text;
                
                if (BlockingInfos.Count <= i)
                    BlockingInfos.Add(new BlockingInfo(valueName));
                else
                    BlockingInfos[i].SetName(valueName);
            }
            
            RemoveExtraValues(capacity);
        }

        protected override void OnComponentFounded()
        {
            _dropdownComponent = Component as DropdownComponent;
        }

        private void OnValueChanged(int value)
        {
            if (CurrentIndex == value)
                return;

            DeactivateLock(CurrentIndex);
            ActivateLock(value);
            CurrentIndex = value;
        }
    }
}
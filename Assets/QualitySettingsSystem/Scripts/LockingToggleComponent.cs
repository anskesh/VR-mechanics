﻿using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(ToggleComponent))]
    public class LockingToggleComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private ToggleComponent _toggleComponent;
        
        private readonly string[] _toggleValues = {"is On", "is Off"};

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
            int capacity = _toggleValues.Length;
            BlockingInfos ??= new List<BlockingInfo>(capacity);
            
            for (int i = 0; i < capacity; i++)
            {
                if (BlockingInfos.Count <= i)
                    BlockingInfos.Add(new BlockingInfo(_toggleValues[i]));
                else
                    BlockingInfos[i].SetName(_toggleValues[i]);
            }
            
            RemoveExtraValues(capacity);
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
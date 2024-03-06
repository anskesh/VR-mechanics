using System;
using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    [RequireComponent(typeof(DropdownComponent))]
    public class BlockingDropdownComponent : BlockingComponent<int>
    {
        [SerializeField, HideInInspector] private DropdownComponent _dropdownComponent;
        [SerializeField, HideInInspector] private string _enumTypeName;
        
        private Type _enumType;

        protected override void Awake()
        {
            base.Awake();
            
            _dropdownComponent.ValueChangedEvent += OnValueChanged;
            ChangeCurrentComponentsInteractable(LastValue, false);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _dropdownComponent.ValueChangedEvent -= OnValueChanged;
        }
        
        protected override void UpdateValues()
        {
            UpdateType();
        }

        protected override void OnComponentFounded()
        {
            _dropdownComponent = Component as DropdownComponent;
        }

        protected override void OnValueChanged(int value)
        {
            if (LastValue == value)
                return;

            ChangeCurrentComponentsInteractable(LastValue, true);
            ChangeCurrentComponentsInteractable(value, false);
            LastValue = value;
        }

        private void UpdateType()
        {
            if (_dropdownComponent.EnumType == null)
                return;
            
            if (_enumTypeName == _dropdownComponent.EnumType.AssemblyQualifiedName || _dropdownComponent.EnumType == null) 
                return;
            
            _enumType = _dropdownComponent.EnumType;
            _enumTypeName = _enumType.AssemblyQualifiedName;
            InitializeValues();
        }

        private void InitializeValues()
        {
            Array values = Enum.GetValues(_enumType);
            BlockingInfos = new List<BlockingInfo>(values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                if (_dropdownComponent.Dropdown.options.Count <= i)
                    return;
                
                BlockingInfo blockingInfo = new (_dropdownComponent.Dropdown.options[i].text);
                BlockingInfos.Add(blockingInfo);
            }
        }
    }
}
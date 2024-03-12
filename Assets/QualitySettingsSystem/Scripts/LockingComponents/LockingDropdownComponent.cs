using UnityEngine;

namespace QualitySettings.UIComponents
{
    [RequireComponent(typeof(EnumDropdownComponent))]
    public class LockingDropdownComponent : LockingComponent
    {
        [SerializeField, HideInInspector] private EnumDropdownComponent _dropdownComponent;

        protected override void Awake()
        {
            base.Awake();
            _dropdownComponent.ValueChangedEvent += OnValueChanged;
        }

        protected void OnDestroy()
        {
            _dropdownComponent.ValueChangedEvent -= OnValueChanged;
        }

        protected override void OnComponentFounded()
        {
            _dropdownComponent = Component as EnumDropdownComponent;
        }

        protected override void SetCapacity()
        {
            Capacity = _dropdownComponent.Options.Count;
        }

        protected override string GetBlockingInfoName(int index)
        {
            return _dropdownComponent.Options[index].text;
        }

        private void OnValueChanged(int value)
        {
            if (CurrentIndex == value)
                return;

            Unlock(CurrentIndex);
            Lock(value);
            CurrentIndex = value;
        }
    }
}
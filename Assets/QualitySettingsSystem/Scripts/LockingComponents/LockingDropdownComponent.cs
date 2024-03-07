using UnityEngine;

namespace QualitySettings.UIComponents
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

        protected override void OnComponentFounded()
        {
            _dropdownComponent = Component as DropdownComponent;
        }

        protected override void SetCapacity()
        {
            Capacity = _dropdownComponent.Options.Count;
        }

        protected override string GetName(int index)
        {
            return _dropdownComponent.Options[index].text;
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
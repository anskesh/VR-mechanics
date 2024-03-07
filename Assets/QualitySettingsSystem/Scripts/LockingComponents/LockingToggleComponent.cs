using UnityEngine;

namespace QualitySettings.UIComponents
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
        
        protected override void OnComponentFounded()
        {
           _toggleComponent = Component as ToggleComponent;
        }

        protected override void SetCapacity()
        {
            Capacity = _toggleValues.Length;
        }

        protected override string GetName(int index)
        {
            return _toggleValues[index];
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
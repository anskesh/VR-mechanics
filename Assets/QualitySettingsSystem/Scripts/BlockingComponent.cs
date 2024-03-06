using System;
using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings
{
    public abstract class BlockingComponent<T> : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected UIComponent Component;
        [SerializeField] protected List<BlockingInfo> BlockingInfos;

        protected T LastValue;

        protected virtual void OnValidate()
        {
            Component = GetComponent<UIComponent>();
            OnComponentFounded();
            UpdateValues();
        }

        protected virtual void Awake() {}
        protected virtual void OnDestroy() {}

        protected abstract void UpdateValues();
        protected abstract void OnComponentFounded();
        protected abstract void OnValueChanged(T value);

        protected void ChangeCurrentComponentsInteractable(int index, bool isInteractable)
        {
            foreach (UIComponent component in BlockingInfos[index].Components)
                component.SetInteractable(isInteractable);
        }
        
        [Serializable]
        protected class BlockingInfo
        {
            public string Name => _name;
            public List<UIComponent> Components => _components;

            [SerializeField] private string _name;
            [SerializeField] private List<UIComponent> _components = new ();

            public BlockingInfo(string name)
            {
                _name = name;
            }
        }
    }
}
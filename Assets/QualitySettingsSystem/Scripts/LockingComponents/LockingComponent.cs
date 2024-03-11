using System;
using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings.UIComponents
{
    public abstract class LockingComponent : MonoBehaviour
    {
        [SerializeField] protected List<LockingComponent> BlockingChildComponents = new ();
        [SerializeField] protected List<BlockingInfo> BlockingInfos;

        [SerializeField, HideInInspector] protected UIComponent Component;

        protected int CurrentIndex;
        protected int Capacity = 0;

        public void OnValidate()
        {
            if (!Component)
            {
                Component = GetComponent<UIComponent>();
                OnComponentFounded();
            }
            
            SetCapacity();
            UpdateValues();
        }

        protected virtual void Awake() {}
        protected virtual void OnDestroy() {}
        protected abstract void OnComponentFounded();
        protected abstract void SetCapacity();
        protected abstract string GetName(int index);

        protected void ActivateLock(int index)
        {
            ChangeInteractable(index, false);
        }

        protected void DeactivateLock(int index)
        {
            ChangeInteractable(index, true);

            foreach (LockingComponent childComponent in BlockingChildComponents)
                childComponent.ActivateLock(childComponent.CurrentIndex);
        }
        
        private void UpdateValues()
        {
            BlockingInfos ??= new List<BlockingInfo>(Capacity);

            for (int i = 0; i < Capacity; i++)
            {
                string valueName = GetName(i);
                
                if (BlockingInfos.Count <= i)
                    BlockingInfos.Add(new BlockingInfo(valueName));
                else
                    BlockingInfos[i].UpdateName(valueName);
            }
            
            RemoveExtraValues(Capacity);
        }

        private void RemoveExtraValues(int capacity)
        {
            if (BlockingInfos.Count <= capacity) return;

            for (int i = capacity - 1; i < BlockingInfos.Count; i++)
                BlockingInfos.RemoveAt(BlockingInfos.Count - 1);
        }

        private void ChangeInteractable(int index, bool isInteractable)
        {
            foreach (UIComponent component in BlockingInfos[index].Components)
                component.SetInteractable(isInteractable);
        }

        [Serializable]
        protected class BlockingInfo
        {
            public IReadOnlyList<UIComponent> Components => _components;

            [SerializeField] private string _name;
            [SerializeField] private List<UIComponent> _components = new();

            public BlockingInfo(string name)
            {
                _name = name;
            }

            public void UpdateName(string name)
            {
                _name = name;
            }
        }
    }
}
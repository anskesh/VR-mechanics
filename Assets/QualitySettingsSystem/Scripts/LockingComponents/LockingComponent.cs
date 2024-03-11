using System;
using System.Collections.Generic;
using UnityEngine;

namespace QualitySettings.UIComponents
{
    public abstract class LockingComponent : MonoBehaviour
    {
        [SerializeField] protected List<LockingComponent> LockingChildComponents = new();
        [SerializeField] protected List<LockingInfo> LockingInfoList;

        [SerializeField, HideInInspector] protected UIComponent Component;

        protected int CurrentIndex;
        protected int Capacity = 0;

        protected virtual void Awake()
        {
            Unlock(CurrentIndex);
        }

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

        protected abstract void OnComponentFounded();
        protected abstract void SetCapacity();
        protected abstract string GetBlockingInfoName(int index);

        protected void Unlock(int index)
        {
            SetInteractable(index, false);
        }

        protected void Lock(int index)
        {
            SetInteractable(index, true);

            foreach (LockingComponent childComponent in LockingChildComponents)
                childComponent.Unlock(childComponent.CurrentIndex);
        }

        private void UpdateValues()
        {
            LockingInfoList ??= new List<LockingInfo>(Capacity);

            for (int i = 0; i < Capacity; i++)
            {
                string infoName = GetBlockingInfoName(i);

                if (i >= LockingInfoList.Count)
                    LockingInfoList.Add(new LockingInfo(infoName));
                else
                    LockingInfoList[i].SetName(infoName);
            }

            RemoveExtraValues(Capacity);
        }

        private void RemoveExtraValues(int capacity)
        {
            if (LockingInfoList.Count <= capacity) return;

            for (int i = capacity - 1; i < LockingInfoList.Count; i++)
                LockingInfoList.RemoveAt(LockingInfoList.Count - 1);
        }

        private void SetInteractable(int index, bool isInteractable)
        {
            foreach (UIComponent component in LockingInfoList[index].Components)
                component.SetInteractable(isInteractable);
        }

        [Serializable]
        protected class LockingInfo
        {
            public IReadOnlyList<UIComponent> Components { get { return _components;} }

            [SerializeField] private string _name;
            [SerializeField] private List<UIComponent> _components = new();

            public LockingInfo(string name)
            {
                _name = name;
            }

            public void SetName(string name)
            {
                _name = name;
            }
        }
    }
}
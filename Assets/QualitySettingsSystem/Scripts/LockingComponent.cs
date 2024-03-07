using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QualitySettings
{
    public abstract class LockingComponent : MonoBehaviour
    {
        [SerializeField] protected List<LockingComponent> BlockingChildComponents = new List<LockingComponent>();
        [SerializeField] protected List<BlockingInfo> BlockingInfos;

        [SerializeField, HideInInspector] protected UIComponent Component;

        protected int CurrentIndex;

        public void Initialize()
        {
            Component = GetComponent<UIComponent>();
            OnComponentFounded();
            UpdateValues();
        }

        protected virtual void Awake() {}
        protected virtual void OnDestroy() {}

        protected abstract void UpdateValues();
        protected abstract void OnComponentFounded();

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

        private void ChangeInteractable(int index, bool isInteractable)
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
            [SerializeField] private List<UIComponent> _components = new();

            public BlockingInfo(string name)
            {
                _name = name;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(LockingComponent), true)]
    public class LockingComponentEditor : Editor
    {
        private void OnEnable()
        {
            LockingComponent component = (LockingComponent) target;
            component.Initialize();
            EditorUtility.SetDirty(component);
        }
    }
#endif
}
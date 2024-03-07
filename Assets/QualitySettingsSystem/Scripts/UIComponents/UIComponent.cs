using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings.UIComponents
{
    public class UIComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected Selectable Selectable;

        protected virtual void Awake() {}
        protected virtual void OnDestroy() {}

        protected virtual void OnValidate()
        {
            Selectable = GetComponent<Selectable>();
        }

        public void SetInteractable(bool isInteractable)
        {
            Selectable.interactable = isInteractable;
        }
    }
}
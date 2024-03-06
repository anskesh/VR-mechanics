using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings
{
    public class UIComponent : MonoBehaviour
    {
        protected Selectable Selectable;

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
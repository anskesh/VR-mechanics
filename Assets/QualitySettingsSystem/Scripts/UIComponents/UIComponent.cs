using UnityEngine;
using UnityEngine.UI;

namespace QualitySettings.UIComponents
{
    public abstract class UIComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected Selectable Selectable;

        protected virtual void OnValidate()
        {
            Selectable = GetComponent<Selectable>();
        }

        public void SetInteractable(bool isInteractable)
        {
            if (isInteractable == Selectable.interactable)
                return;
            
            Selectable.interactable = isInteractable;
        }
    }
}
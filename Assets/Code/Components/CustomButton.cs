using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace Assets.Code.Components
{
    public class CustomButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public bool IsSelected { get; private set; }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
        }
    }
}

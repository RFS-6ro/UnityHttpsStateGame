using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GUI.Input
{
    public class InputListener : MonoBehaviour, IDragHandler
    {
        public event Action<Vector2> InputUpdateEvent;

        public void OnDrag(PointerEventData eventData)
        {
            InputUpdateEvent?.Invoke(eventData.delta);
        }
    }
}

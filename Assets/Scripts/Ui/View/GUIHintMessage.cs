using UnityEngine;
using UnityEngine.UI;

namespace GUI.View
{
    public class GUIHintMessage : MonoBehaviour
    {
        [SerializeField] private TextView _view;

        private void Awake()
        {
            if (_view == null)
            {
                _view = GetComponentInChildren<TextView>();
            }

            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void SetMessage(string message)
        {
            _view.SetText(message);
        }

        public void SetTargetPosition(Vector2 position)
        {
            transform.localPosition = position;
        }

        public void SetTargetPositionAndMessage(string message, Vector2 position)
        {
            SetMessage(message);
            SetTargetPosition(position);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

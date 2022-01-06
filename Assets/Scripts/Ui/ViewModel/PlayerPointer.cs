using Core.Data;
using GUI.View;
using UnityEngine;
using Utils.Extensions;

namespace GUI.ViewModel
{
	public class PlayerPointer : MonoBehaviour, IGUIListener, IGUIInitializer
	{
		private Camera _camera;
		private RectTransform _canvasRectangle;

		private GUIManager _guiManager;
		private GUIHintMessage _hint;

		private bool _isEnabled;
		public bool IsEnabled => _isEnabled;
		public Transform Target { get; private set; }

		public event System.Action<Transform> OnTargetChanged;

		public void Enable()
		{
			_isEnabled = true;
			_guiManager?.RegisterListener(this);
			if (_hint != null)
			{
				_hint.Show();
			}
		}

		private void Disable()
		{
			_isEnabled = false;
			_guiManager?.UnregisterListener(this);

			if (_hint != null)
			{
				_hint.Hide();
			}
		}

		public void InitializeWithRoot(GUIManager guiManager)
		{
			_guiManager = guiManager;

			if (_hint == null)
			{
				_hint = _guiManager.GetHint();
			}

			_camera = _guiManager.Camera;
			_canvasRectangle = _guiManager.CanvasRectangle;
			SetTarget(transform);
			_hint.Show();
		}

		public void UpdateGUI()
		{
			if (Target == null || _hint == null)
			{
				return;
			}

			//TODO: Remove duplicate at the opposite coords
			Vector2 screenPointForNotification = _camera.WorldToScreenPoint(Target.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
				_canvasRectangle,
				screenPointForNotification,
				null,
				out Vector2 localCanvasPosition);
			_hint.SetTargetPosition(localCanvasPosition);
		}

		public void SetTarget(Transform target) => SetTarget(target.GetOrAddComponent<Entity>());
		public void SetTarget(GameObject target) => SetTarget(target.GetOrAddComponent<Entity>());

		public void SetTarget(Entity targetEntity)
		{
#if UNITY_EDITOR
			if (Target != null)
			{
				Debug.Log("Switching targeting player");
			}
#endif

			Target = targetEntity.transform;
			OnTargetChanged.SafeInvoke(Target);

			if (_hint == null)
			{
				return;
			}

			_hint.SetMessage(targetEntity.Name);

			if (_isEnabled)
			{
				_hint.Show();
			}
			else
			{
				_hint.Hide();
			}
		}
	}
}

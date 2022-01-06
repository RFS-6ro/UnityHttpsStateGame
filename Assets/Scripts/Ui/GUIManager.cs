using System.Collections;
using Core.Networking;
using Core.Networking.Protocols;
using GUI.View;
using GUI.ViewModel;
using GUI.ViewModel.DebugGUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Utils.Network;

namespace GUI
{
	public class GUIManager : MonoBehaviour
	{
		//TODO: Move version in config
		private const string VERSION = "v0.3.5";

		private List<IGUIListener> _listeners;

#if DEBUG
		private DebugViewModel _debugViewModel;
#endif

		[SerializeField] private Camera _camera;
		[SerializeField] private RectTransform _canvasRectangle;

		[SerializeField] private Transform _notificationHolder;

		[SerializeField] private GUIHintMessage _hintPrefab;

		public Camera Camera => _camera;
		public RectTransform CanvasRectangle => _canvasRectangle;

		public void Initialize(
#if DEBUG
			DebugViewModel debugViewModel
#endif
		)
		{
#if DEBUG
			_debugViewModel = debugViewModel;
			HttpProtocol.RetrieveData(
				$"{NetworkManager.SERVER_URL}/version",
				(data) => _debugViewModel.Initialize(data.ReadString(), VERSION),
				null
			);
#endif

			_listeners = ComponentUtils.FindObjectsWithInterface<IGUIListener>().ToList();

			ComponentUtils.FindObjectsWithInterface<IGUIInitializer>()
				.ToList()?
				.ForEach((initializableListener) => initializableListener.InitializeWithRoot(this));
		}

		private void LateUpdate()
		{
			//optimize: lots of event calls could negativelly affect on perfomance
			_listeners?.ForEach((listener) =>
			{
				if (listener.IsEnabled)
				{
					listener.UpdateGUI();
				}
			});
		}

		public void RegisterListener(IGUIListener listener)
		{
			if (_listeners != null &&
			    _listeners.Contains(listener) == false)
			{
				_listeners.Add(listener);

                if (listener is IGUIInitializer initializableListener)
                {
					initializableListener.InitializeWithRoot(this);
				}
			}
		}

		public void UnregisterListener(IGUIListener listener)
		{
			if (_listeners != null &&
			    _listeners.Contains(listener))
			{
				_listeners.Remove(listener);
			}
		}

		public GUIHintMessage GetHint()
		{
			//TODO: replace with pool
			GUIHintMessage hint = Instantiate(_hintPrefab, _notificationHolder);

			return hint;
		}
	}
}

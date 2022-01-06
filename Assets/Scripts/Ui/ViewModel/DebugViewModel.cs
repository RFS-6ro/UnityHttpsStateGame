#if DEBUG
using GUI.View;
using UnityEngine;
namespace GUI.ViewModel.DebugGUI
{
	public class DebugViewModel : MonoBehaviour
	{
		[SerializeField] private TextView _serverVersion;
		[SerializeField] private TextView _clientVersion;
		[SerializeField] private TextView _debugText;

		public void Initialize(string serverVersion, string clientVersion)
		{
			_serverVersion.SetText(serverVersion);
			_clientVersion.SetText(clientVersion);
		}

		public void SetDebug(string text)
		{
			_debugText.SetText(text);
		}
	}
}
#endif
